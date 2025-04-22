﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using Hikari.Common.Hardware.Components;

namespace Hikari.Common.Hardware.Retrieval
{
    // https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-memorystatusex

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MEMORYSTATUSEX()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }

    internal class WindowsHardwareInfoRetrieval : HardwareInfoRetrievalBase, IHardwareInfo
    {
        private readonly MEMORYSTATUSEX _memoryStatusEx = new MEMORYSTATUSEX();

        private readonly MemoryStatusInfo _memoryStatus = new ();

        public bool UseAsteriskInWMI { get; set; }

        private readonly string _managementScope = "root\\cimv2";
        private readonly System.Management.EnumerationOptions _enumerationOptions = new System.Management.EnumerationOptions() { ReturnImmediately = true, Rewindable = false, Timeout = System.Management.EnumerationOptions.InfiniteTimeout };

        private readonly Version? _osVersion;

        public WindowsHardwareInfoRetrieval(TimeSpan? enumerationOptionsTimeout = null)
        {
            if (enumerationOptionsTimeout == null)
                enumerationOptionsTimeout = System.Management.EnumerationOptions.InfiniteTimeout;

            _enumerationOptions = new System.Management.EnumerationOptions() { ReturnImmediately = true, Rewindable = false, Timeout = enumerationOptionsTimeout.Value };

            _osVersion = GetOsVersionByWmi() ?? GetOsVersionByRtlGetVersion();
        }

        // https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public MemoryStatusInfo GetMemoryStatus()
        {
            if (GlobalMemoryStatusEx(_memoryStatusEx))
            {
                _memoryStatus.TotalPhysical = _memoryStatusEx.ullTotalPhys;
                _memoryStatus.AvailablePhysical = _memoryStatusEx.ullAvailPhys;
                _memoryStatus.TotalPageFile = _memoryStatusEx.ullTotalPageFile;
                _memoryStatus.AvailablePageFile = _memoryStatusEx.ullAvailPageFile;
                _memoryStatus.TotalVirtual = _memoryStatusEx.ullTotalVirtual;
                _memoryStatus.AvailableVirtual = _memoryStatusEx.ullAvailVirtual;
                _memoryStatus.AvailableExtendedVirtual = _memoryStatusEx.ullAvailExtendedVirtual;
            }

            return _memoryStatus;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int RtlGetVersion([In, Out] ref OSVERSIONINFOEX lpVersionInformation);

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;

            public ushort wServicePackMajor;
            public ushort wServicePackMinor;
            public ushort wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        public static Version? GetOsVersionByRtlGetVersion()
        {
            var info = new OSVERSIONINFOEX();
            info.dwOSVersionInfoSize = (uint)Marshal.SizeOf(info);

            var result = RtlGetVersion(ref info);

            return (result == 0) // STATUS_SUCCESS
                ? new Version((int)info.dwMajorVersion, (int)info.dwMinorVersion, (int)info.dwBuildNumber)
                : null;
        }

        public static Version? GetOsVersionByWmi()
        {
            using (var searcher = new ManagementObjectSearcher(new SelectQuery("Win32_OperatingSystem")))
            {
                var os = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
                var properties = os?.Properties.Cast<PropertyData>().ToArray();

                var osTypeValue = (ushort)(properties?.SingleOrDefault(x => (x.Name == "OSType") && (x.Type == CimType.UInt16))?.Value ?? 0);
                if (osTypeValue == 18) // WINNT
                {
                    var versionValue = properties?.SingleOrDefault(x => x.Name == "Version")?.Value as string;
                    if (versionValue != null)
                        return new Version(versionValue);
                }

                return null;
            }
        }

        public static T GetPropertyValue<T>(object obj) where T : struct
        {
            return (obj == null) ? default(T) : (T)obj;
        }

        public static T[] GetPropertyArray<T>(object obj)
        {
            return (obj is T[] array) ? array : Array.Empty<T>();
        }

        public static string GetPropertyString(object obj)
        {
            return (obj is string str) ? str : string.Empty;
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.management.managementpath.defaultpath?view=netframework-4.8

        public List<BatteryInfo> GetBatteryList()
        {
            List<BatteryInfo> batteryList = new List<BatteryInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_Battery"
                                                  : "SELECT FullChargeCapacity, DesignCapacity, BatteryStatus, EstimatedChargeRemaining, EstimatedRunTime, ExpectedLife, MaxRechargeTime, TimeOnBattery, TimeToFullCharge FROM Win32_Battery";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                BatteryInfo battery = new BatteryInfo
                {
                    FullChargeCapacity = GetPropertyValue<uint>(mo["FullChargeCapacity"]),
                    DesignCapacity = GetPropertyValue<uint>(mo["DesignCapacity"]),
                    BatteryStatus = GetPropertyValue<ushort>(mo["BatteryStatus"]),
                    EstimatedChargeRemaining = GetPropertyValue<ushort>(mo["EstimatedChargeRemaining"]),
                    EstimatedRunTime = GetPropertyValue<uint>(mo["EstimatedRunTime"]),
                    ExpectedLife = GetPropertyValue<uint>(mo["ExpectedLife"]),
                    MaxRechargeTime = GetPropertyValue<uint>(mo["MaxRechargeTime"]),
                    TimeOnBattery = GetPropertyValue<uint>(mo["TimeOnBattery"]),
                    TimeToFullCharge = GetPropertyValue<uint>(mo["TimeToFullCharge"])
                };

                batteryList.Add(battery);
            }

            return batteryList;
        }

        public List<BiosInfo> GetBiosList()
        {
            List<BiosInfo> biosList = new List<BiosInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_BIOS"
                                                  : "SELECT Caption, Description, Manufacturer, Name, ReleaseDate, SerialNumber, SoftwareElementID, Version FROM Win32_BIOS";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                BiosInfo bios = new BiosInfo()
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    Name = GetPropertyString(mo["Name"]),
                    ReleaseDate = GetPropertyString(mo["ReleaseDate"]),
                    SerialNumber = GetPropertyString(mo["SerialNumber"]),
                    SoftwareElementID = GetPropertyString(mo["SoftwareElementID"]),
                    Version = GetPropertyString(mo["Version"])
                };

                biosList.Add(bios);
            }

            return biosList;
        }

        public List<CpuInfo> GetCpuList(bool includePercentProcessorTime = true)
        {
            List<CpuInfo> cpuList = new List<CpuInfo>();

            List<CpuCoreInfo> cpuCoreList = new List<CpuCoreInfo>();

            ulong percentProcessorTime = 0ul;

            if (includePercentProcessorTime)
            {
                string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name != '_Total'"
                                                      : "SELECT Name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name != '_Total'";
                using ManagementObjectSearcher percentProcessorTimeMOS = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

                foreach (ManagementObject mo in percentProcessorTimeMOS.Get())
                {
                    CpuCoreInfo core = new CpuCoreInfo
                    {
                        Name = GetPropertyString(mo["Name"]),
                        PercentProcessorTime = GetPropertyValue<ulong>(mo["PercentProcessorTime"])
                    };

                    cpuCoreList.Add(core);
                }

                string QueryString = UseAsteriskInWMI ? "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'"
                                                      : "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'";
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(_managementScope, QueryString, _enumerationOptions);

                foreach (ManagementObject mo in managementObjectSearcher.Get())
                {
                    percentProcessorTime = GetPropertyValue<ulong>(mo["PercentProcessorTime"]);
                }
            }

            bool isAtLeastWin8 = (_osVersion?.Major == 6 && _osVersion?.Minor >= 2) || (_osVersion?.Major > 6);

            string query = UseAsteriskInWMI ? "SELECT * FROM Win32_Processor"
                                            : isAtLeastWin8 ? "SELECT Caption, CurrentClockSpeed, Description, L2CacheSize, L3CacheSize, Manufacturer, MaxClockSpeed, Name, NumberOfCores, NumberOfLogicalProcessors, ProcessorId, SecondLevelAddressTranslationExtensions, SocketDesignation, VirtualizationFirmwareEnabled, VMMonitorModeExtensions FROM Win32_Processor"
                                                            : "SELECT Caption, CurrentClockSpeed, Description, L2CacheSize, L3CacheSize, Manufacturer, MaxClockSpeed, Name, NumberOfCores, NumberOfLogicalProcessors, ProcessorId, SocketDesignation FROM Win32_Processor";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                CpuInfo cpu = new CpuInfo()
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    CurrentClockSpeed = GetPropertyValue<uint>(mo["CurrentClockSpeed"]),
                    Description = GetPropertyString(mo["Description"]),
                    L2CacheSize = GetPropertyValue<uint>(mo["L2CacheSize"]),
                    L3CacheSize = GetPropertyValue<uint>(mo["L3CacheSize"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    MaxClockSpeed = GetPropertyValue<uint>(mo["MaxClockSpeed"]),
                    Name = GetPropertyString(mo["Name"]),
                    NumberOfCores = GetPropertyValue<uint>(mo["NumberOfCores"]),
                    NumberOfLogicalProcessors = GetPropertyValue<uint>(mo["NumberOfLogicalProcessors"]),
                    ProcessorId = GetPropertyString(mo["ProcessorId"]),
                    SocketDesignation = GetPropertyString(mo["SocketDesignation"]),
                    PercentProcessorTime = percentProcessorTime,
                    CpuCoreList = cpuCoreList
                };

                if (isAtLeastWin8)
                {
                    cpu.SecondLevelAddressTranslationExtensions = GetPropertyValue<bool>(mo["SecondLevelAddressTranslationExtensions"]);
                    cpu.VirtualizationFirmwareEnabled = GetPropertyValue<bool>(mo["VirtualizationFirmwareEnabled"]);
                    cpu.VMMonitorModeExtensions = GetPropertyValue<bool>(mo["VMMonitorModeExtensions"]);
                }

                cpuList.Add(cpu);
            }

            return cpuList;
        }

        public override List<Hikari.Common.Hardware.Components.DriveInfo> GetDriveList()
        {
            List<Hikari.Common.Hardware.Components.DriveInfo> driveList = new ();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_DiskDrive"
                                                  : "SELECT Caption, Description, DeviceID, FirmwareRevision, Index, Manufacturer, Model, Name, Partitions, SerialNumber, Size FROM Win32_DiskDrive";
            using ManagementObjectSearcher Win32_DiskDrive = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);
            foreach (ManagementObject DiskDrive in Win32_DiskDrive.Get())
            {
                Hikari.Common.Hardware.Components.DriveInfo drive = new ()
                {
                    Caption = GetPropertyString(DiskDrive["Caption"]),
                    Description = GetPropertyString(DiskDrive["Description"]),
                    FirmwareRevision = GetPropertyString(DiskDrive["FirmwareRevision"]),
                    Index = GetPropertyValue<uint>(DiskDrive["Index"]),
                    Manufacturer = GetPropertyString(DiskDrive["Manufacturer"]),
                    Model = GetPropertyString(DiskDrive["Model"]),
                    Name = GetPropertyString(DiskDrive["Name"]),
                    Partitions = GetPropertyValue<uint>(DiskDrive["Partitions"]),
                    SerialNumber = GetPropertyString(DiskDrive["SerialNumber"]),
                    Size = GetPropertyValue<ulong>(DiskDrive["Size"])
                };

                string queryString1 = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + DiskDrive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                using ManagementObjectSearcher Win32_DiskPartition = new ManagementObjectSearcher(_managementScope, queryString1, _enumerationOptions);
                foreach (ManagementObject DiskPartition in Win32_DiskPartition.Get())
                {
                    PartitionInfo partition = new PartitionInfo
                    {
                        Bootable = GetPropertyValue<bool>(DiskPartition["Bootable"]),
                        BootPartition = GetPropertyValue<bool>(DiskPartition["BootPartition"]),
                        Caption = GetPropertyString(DiskPartition["Caption"]),
                        Description = GetPropertyString(DiskPartition["Description"]),
                        DiskIndex = GetPropertyValue<uint>(DiskPartition["DiskIndex"]),
                        Index = GetPropertyValue<uint>(DiskPartition["Index"]),
                        Name = GetPropertyString(DiskPartition["Name"]),
                        PrimaryPartition = GetPropertyValue<bool>(DiskPartition["PrimaryPartition"]),
                        Size = GetPropertyValue<ulong>(DiskPartition["Size"]),
                        StartingOffset = GetPropertyValue<ulong>(DiskPartition["StartingOffset"])
                    };

                    string queryString2 = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + DiskPartition["DeviceID"] + "'} WHERE AssocClass = Win32_LogicalDiskToPartition";
                    using ManagementObjectSearcher Win32_LogicalDisk = new ManagementObjectSearcher(_managementScope, queryString2, _enumerationOptions);
                    foreach (ManagementObject LogicalDisk in Win32_LogicalDisk.Get())
                    {
                        VolumeInfo volume = new VolumeInfo
                        {
                            Caption = GetPropertyString(LogicalDisk["Caption"]),
                            Compressed = GetPropertyValue<bool>(LogicalDisk["Compressed"]),
                            Description = GetPropertyString(LogicalDisk["Description"]),
                            FileSystem = GetPropertyString(LogicalDisk["FileSystem"]),
                            FreeSpace = GetPropertyValue<ulong>(LogicalDisk["FreeSpace"]),
                            Name = GetPropertyString(LogicalDisk["Name"]),
                            Size = GetPropertyValue<ulong>(LogicalDisk["Size"]),
                            VolumeName = GetPropertyString(LogicalDisk["VolumeName"]),
                            VolumeSerialNumber = GetPropertyString(LogicalDisk["VolumeSerialNumber"])
                        };

                        partition.VolumeList.Add(volume);
                    }

                    drive.PartitionList.Add(partition);
                }

                driveList.Add(drive);
            }

            return driveList;
        }

        public List<KeyboardInfo> GetKeyboardList()
        {
            List<KeyboardInfo> keyboardList = new List<KeyboardInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_Keyboard"
                                                  : "SELECT Caption, Description, Name, NumberOfFunctionKeys FROM Win32_Keyboard";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                KeyboardInfo keyboard = new KeyboardInfo
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    Name = GetPropertyString(mo["Name"]),
                    NumberOfFunctionKeys = GetPropertyValue<ushort>(mo["NumberOfFunctionKeys"])
                };

                keyboardList.Add(keyboard);
            }

            return keyboardList;
        }

        public List<MemoryInfo> GetMemoryList()
        {
            List<MemoryInfo> memoryList = new List<MemoryInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_PhysicalMemory"
                                                  : _osVersion?.Major >= 10 ? "SELECT BankLabel, Capacity, FormFactor, Manufacturer, MaxVoltage, MinVoltage, PartNumber, SerialNumber, Speed FROM Win32_PhysicalMemory"
                                                                            : "SELECT BankLabel, Capacity, FormFactor, Manufacturer, PartNumber, SerialNumber, Speed FROM Win32_PhysicalMemory";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                MemoryInfo memory = new MemoryInfo
                {
                    BankLabel = GetPropertyString(mo["BankLabel"]),
                    Capacity = GetPropertyValue<ulong>(mo["Capacity"]),
                    FormFactor = (FormFactor)GetPropertyValue<ushort>(mo["FormFactor"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    PartNumber = GetPropertyString(mo["PartNumber"]),
                    SerialNumber = GetPropertyString(mo["SerialNumber"]),
                    Speed = GetPropertyValue<uint>(mo["Speed"])
                };

                if (_osVersion?.Major >= 10)
                {
                    memory.MaxVoltage = GetPropertyValue<uint>(mo["MaxVoltage"]);
                    memory.MinVoltage = GetPropertyValue<uint>(mo["MinVoltage"]);
                }

                memoryList.Add(memory);
            }

            return memoryList;
        }

        public List<MonitorInfo> GetMonitorList()
        {
            List<MonitorInfo> monitorList = new List<MonitorInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_DesktopMonitor WHERE PNPDeviceID IS NOT NULL"
                                                  : "SELECT Caption, Description, MonitorManufacturer, MonitorType, Name, PixelsPerXLogicalInch, PixelsPerYLogicalInch FROM Win32_DesktopMonitor WHERE PNPDeviceID IS NOT NULL";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                MonitorInfo monitor = new MonitorInfo
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    MonitorManufacturer = GetPropertyString(mo["MonitorManufacturer"]),
                    MonitorType = GetPropertyString(mo["MonitorType"]),
                    Name = GetPropertyString(mo["Name"]),
                    PixelsPerXLogicalInch = GetPropertyValue<uint>(mo["PixelsPerXLogicalInch"]),
                    PixelsPerYLogicalInch = GetPropertyValue<uint>(mo["PixelsPerYLogicalInch"])
                };

                monitorList.Add(monitor);
            }

            return monitorList;
        }

        public List<MotherboardInfo> GetMotherboardList()
        {
            List<MotherboardInfo> motherboardList = new List<MotherboardInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_BaseBoard"
                                                  : "SELECT Manufacturer, Product, SerialNumber FROM Win32_BaseBoard";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                MotherboardInfo motherboard = new MotherboardInfo
                {
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    Product = GetPropertyString(mo["Product"]),
                    SerialNumber = GetPropertyString(mo["SerialNumber"])
                };

                motherboardList.Add(motherboard);
            }

            return motherboardList;
        }

        public List<MouseInfo> GetMouseList()
        {
            List<MouseInfo> mouseList = new List<MouseInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_PointingDevice"
                                                  : "SELECT Caption, Description, Manufacturer, Name, NumberOfButtons FROM Win32_PointingDevice";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                MouseInfo mouse = new MouseInfo
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    Name = GetPropertyString(mo["Name"]),
                    NumberOfButtons = GetPropertyValue<byte>(mo["NumberOfButtons"])
                };

                mouseList.Add(mouse);
            }

            return mouseList;
        }

        public override List<NetworkAdapterInfo> GetNetworkAdapterList(bool includeBytesPersec = true, bool includeNetworkAdapterConfiguration = true)
        {
            List<NetworkAdapterInfo> networkAdapterList = new List<NetworkAdapterInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True AND MACAddress IS NOT NULL"
                                                  : "SELECT AdapterType, Caption, Description, DeviceID, MACAddress, Manufacturer, Name, NetConnectionID, ProductName, Speed FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True AND MACAddress IS NOT NULL";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                NetworkAdapterInfo networkAdapter = new NetworkAdapterInfo
                {
                    AdapterType = GetPropertyString(mo["AdapterType"]),
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    MACAddress = GetPropertyString(mo["MACAddress"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    Name = GetPropertyString(mo["Name"]),
                    NetConnectionID = GetPropertyString(mo["NetConnectionID"]),
                    ProductName = GetPropertyString(mo["ProductName"]),
                    Speed = GetPropertyValue<ulong>(mo["Speed"])
                };

                if (includeBytesPersec)
                {
                    string query = UseAsteriskInWMI ? $"SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkAdapter WHERE Name = '{networkAdapter.Name.Replace("(", "[").Replace(")", "]")}'"
                                                    : $"SELECT BytesSentPersec, BytesReceivedPersec FROM Win32_PerfFormattedData_Tcpip_NetworkAdapter WHERE Name = '{networkAdapter.Name.Replace("(", "[").Replace(")", "]")}'";
                    using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(_managementScope, query, _enumerationOptions);
                    foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                    {
                        networkAdapter.BytesSentPersec = GetPropertyValue<ulong>(managementObject["BytesSentPersec"]);
                        networkAdapter.BytesReceivedPersec = GetPropertyValue<ulong>(managementObject["BytesReceivedPersec"]);
                    }
                }

                if (includeNetworkAdapterConfiguration)
                {
                    IPAddress address;
                    foreach (ManagementObject configuration in mo.GetRelated("Win32_NetworkAdapterConfiguration"))
                    {
                        foreach (string str in GetPropertyArray<string>(configuration["DefaultIPGateway"]))
                            if (IPAddress.TryParse(str, out address))
                                networkAdapter.DefaultIPGatewayList.Add(address);

                        if (IPAddress.TryParse(GetPropertyString(configuration["DHCPServer"]), out address))
                            networkAdapter.DHCPServer = address;

                        foreach (string str in GetPropertyArray<string>(configuration["DNSServerSearchOrder"]))
                            if (IPAddress.TryParse(str, out address))
                                networkAdapter.DNSServerSearchOrderList.Add(address);

                        foreach (string str in GetPropertyArray<string>(configuration["IPAddress"]))
                            if (IPAddress.TryParse(str, out address))
                                networkAdapter.IPAddressList.Add(address);

                        foreach (string str in GetPropertyArray<string>(configuration["IPSubnet"]))
                            if (IPAddress.TryParse(str, out address))
                                networkAdapter.IPSubnetList.Add(address);
                    }
                }

                networkAdapterList.Add(networkAdapter);
            }

            return networkAdapterList;
        }

        public List<PrinterInfo> GetPrinterList()
        {
            List<PrinterInfo> printerList = new List<PrinterInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_Printer"
                                                  : "SELECT Caption, Default, Description, HorizontalResolution, Local, Name, Network, Shared, VerticalResolution FROM Win32_Printer";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                PrinterInfo printer = new PrinterInfo
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Default = GetPropertyValue<bool>(mo["Default"]),
                    Description = GetPropertyString(mo["Description"]),
                    HorizontalResolution = GetPropertyValue<uint>(mo["HorizontalResolution"]),
                    Local = GetPropertyValue<bool>(mo["Local"]),
                    Name = GetPropertyString(mo["Name"]),
                    Network = GetPropertyValue<bool>(mo["Network"]),
                    Shared = GetPropertyValue<bool>(mo["Shared"]),
                    VerticalResolution = GetPropertyValue<uint>(mo["VerticalResolution"])
                };

                printerList.Add(printer);
            }

            return printerList;
        }

        public List<SoundDeviceInfo> GetSoundDeviceList()
        {
            List<SoundDeviceInfo> soundDeviceList = new List<SoundDeviceInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_SoundDevice WHERE NOT Manufacturer='Microsoft'"
                                                  : "SELECT Caption, Description, Manufacturer, Name, ProductName FROM Win32_SoundDevice WHERE NOT Manufacturer='Microsoft'";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                SoundDeviceInfo soundDevice = new SoundDeviceInfo
                {
                    Caption = GetPropertyString(mo["Caption"]),
                    Description = GetPropertyString(mo["Description"]),
                    Manufacturer = GetPropertyString(mo["Manufacturer"]),
                    Name = GetPropertyString(mo["Name"]),
                    ProductName = GetPropertyString(mo["ProductName"])
                };

                soundDeviceList.Add(soundDevice);
            }

            return soundDeviceList;
        }

        public List<VideoControllerInfo> GetVideoControllerList()
        {
            List<VideoControllerInfo> videoControllerList = new List<VideoControllerInfo>();

            string queryString = UseAsteriskInWMI ? "SELECT * FROM Win32_VideoController"
                                                  : "SELECT AdapterCompatibility, AdapterRAM, Caption, CurrentBitsPerPixel, CurrentHorizontalResolution, CurrentNumberOfColors, CurrentRefreshRate, CurrentVerticalResolution, Description, DriverDate, DriverVersion, MaxRefreshRate, MinRefreshRate, Name, VideoModeDescription, VideoProcessor FROM Win32_VideoController";
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(_managementScope, queryString, _enumerationOptions);

            foreach (ManagementObject mo in mos.Get())
            {
                VideoControllerInfo videoController = new VideoControllerInfo
                {
                    Manufacturer = GetPropertyString(mo["AdapterCompatibility"]),
                    AdapterRAM = GetPropertyValue<uint>(mo["AdapterRAM"]),
                    Caption = GetPropertyString(mo["Caption"]),
                    CurrentBitsPerPixel = GetPropertyValue<uint>(mo["CurrentBitsPerPixel"]),
                    CurrentHorizontalResolution = GetPropertyValue<uint>(mo["CurrentHorizontalResolution"]),
                    CurrentNumberOfColors = GetPropertyValue<ulong>(mo["CurrentNumberOfColors"]),
                    CurrentRefreshRate = GetPropertyValue<uint>(mo["CurrentRefreshRate"]),
                    CurrentVerticalResolution = GetPropertyValue<uint>(mo["CurrentVerticalResolution"]),
                    Description = GetPropertyString(mo["Description"]),
                    DriverDate = GetPropertyString(mo["DriverDate"]),
                    DriverVersion = GetPropertyString(mo["DriverVersion"]),
                    MaxRefreshRate = GetPropertyValue<uint>(mo["MaxRefreshRate"]),
                    MinRefreshRate = GetPropertyValue<uint>(mo["MinRefreshRate"]),
                    Name = GetPropertyString(mo["Name"]),
                    VideoModeDescription = GetPropertyString(mo["VideoModeDescription"]),
                    VideoProcessor = GetPropertyString(mo["VideoProcessor"])
                };

                videoControllerList.Add(videoController);
            }

            return videoControllerList;
        }
    }
}