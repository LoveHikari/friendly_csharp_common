using Hikari.Common.Hardware.Components;
using Hikari.Common.Hardware.Retrieval;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Hikari.Common.Hardware;

/// <summary>
/// 硬件信息帮助类，使用命令行的方式查询计算机硬件信息，自动识别系统。在 Linux 上使用 /dev、/proc、/sys，在 macOS 上使用 sysctl、system_profiler。
/// https://github.com/Jinjinov/Hardware.Info
/// </summary>
public class HardwareInfo : IHardwareInfo
{
    private readonly IHardwareInfo _hardwareInfo;
    /// <summary>
    /// 构造函数
    /// </summary>
    public HardwareInfo()
    {
        // 判断当前系统
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _hardwareInfo = new LinuxHardwareInfoRetrieval();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            _hardwareInfo = new OSXHardwareInfoRetrieval();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _hardwareInfo = new WindowsHardwareInfoRetrieval();
        }
        else
        {
            _hardwareInfo = new WindowsHardwareInfoRetrieval();
        }
    }


    public MemoryStatusInfo GetMemoryStatus()
    {
        return _hardwareInfo.GetMemoryStatus();
    }

    public List<BatteryInfo> GetBatteryList()
    {
        return _hardwareInfo.GetBatteryList();
    }

    public List<BiosInfo> GetBiosList()
    {
        return _hardwareInfo.GetBiosList();
    }

    public List<CpuInfo> GetCpuList(bool includePercentProcessorTime = true)
    {
        return _hardwareInfo.GetCpuList(includePercentProcessorTime);
    }

    public List<Hikari.Common.Hardware.Components.DriveInfo> GetDriveList()
    {
        return _hardwareInfo.GetDriveList();
    }

    public List<KeyboardInfo> GetKeyboardList()
    {
        return _hardwareInfo.GetKeyboardList();
    }

    public List<MemoryInfo> GetMemoryList()
    {
        return _hardwareInfo.GetMemoryList();
    }

    public List<MonitorInfo> GetMonitorList()
    {
        return _hardwareInfo.GetMonitorList();
    }

    public List<MotherboardInfo> GetMotherboardList()
    {
        return _hardwareInfo.GetMotherboardList();
    }

    public List<MouseInfo> GetMouseList()
    {
        return _hardwareInfo.GetMouseList();
    }

    public List<NetworkAdapterInfo> GetNetworkAdapterList(bool includeBytesPersec = true, bool includeNetworkAdapterConfiguration = true)
    {
        return _hardwareInfo.GetNetworkAdapterList(includeBytesPersec, includeNetworkAdapterConfiguration);
    }

    public List<PrinterInfo> GetPrinterList()
    {
        return _hardwareInfo.GetPrinterList();
    }

    public List<SoundDeviceInfo> GetSoundDeviceList()
    {
        return _hardwareInfo.GetSoundDeviceList();
    }

    public List<VideoControllerInfo> GetVideoControllerList()
    {
        return _hardwareInfo.GetVideoControllerList();
    }
    public IEnumerable<IPAddress> GetLocalIPv4Addresses()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                               .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                               .Where(addressInformation => addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                               .Select(addressInformation => addressInformation.Address);
        }
        else
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }

    public IEnumerable<IPAddress> GetLocalIPv4Addresses(NetworkInterfaceType networkInterfaceType)
    {
        return NetworkInterface.GetAllNetworkInterfaces()
                               .Where(networkInterface => networkInterface.NetworkInterfaceType == networkInterfaceType)
                               .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                               .Where(addressInformation => addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                               .Select(addressInformation => addressInformation.Address);
    }

    public IEnumerable<IPAddress> GetLocalIPv4Addresses(OperationalStatus operationalStatus)
    {
        return NetworkInterface.GetAllNetworkInterfaces()
                               .Where(networkInterface => networkInterface.OperationalStatus == operationalStatus)
                               .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                               .Where(addressInformation => addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                               .Select(addressInformation => addressInformation.Address);
    }

    public IEnumerable<IPAddress> GetLocalIPv4Addresses(NetworkInterfaceType networkInterfaceType, OperationalStatus operationalStatus)
    {
        return NetworkInterface.GetAllNetworkInterfaces()
                               .Where(networkInterface => networkInterface.NetworkInterfaceType == networkInterfaceType && networkInterface.OperationalStatus == operationalStatus)
                               .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                               .Where(addressInformation => addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                               .Select(addressInformation => addressInformation.Address);
    }



    // https://github.com/lindexi/UWP/blob/master/uwp/src/MacAddress/MacAddress/MacAddress.cs
    // https://blog.csdn.net/diedouju3498/article/details/101894078


    /// <summary>
    /// 获取当前使用的IP
    /// </summary>
    /// <returns></returns>
    public static string GetLocalUsedIPv4()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up); // 所有网卡信息
        var iPv4Address = interfaces.SelectMany(n => n.GetIPProperties().UnicastAddresses).Where(n => n.Address.AddressFamily == AddressFamily.InterNetwork).ToList();

        var address = iPv4Address.FirstOrDefault(i => i.IsDnsEligible == true && i.SuffixOrigin == SuffixOrigin.OriginDhcp);  // 如果地址不符合 DNS 条件，则它是保留的内部 IP, 它不是互联网提供商主机。如果 PrefixOrigin 是由 DHCP 服务器提供的，这可能是最佳地址选择
        return address?.Address.ToString() ?? "127.0.0.1";
    }

    /// <summary>
    /// 获取当前使用的IP
    /// </summary>
    /// <returns></returns>
    public static string GetLocalUsedIPv6()
    {
        var iPv4 = GetLocalUsedIPv4();
        var ipv6 = "";
        var interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up); // 所有网卡信息
        foreach (NetworkInterface inter in interfaces)
        {
            var properties = inter.GetIPProperties();
            foreach (UnicastIPAddressInformation information in properties.UnicastAddresses)
            {
                if (information.Address.ToString() == iPv4)
                {
                    ipv6 = properties.UnicastAddresses.FirstOrDefault(p => p.Address.AddressFamily == AddressFamily.InterNetworkV6).Address.ToString();
                    break;
                }
            }
        }

        return ipv6;
    }

    /// <summary>
    /// 获取本地Mac地址
    /// </summary>
    /// <returns></returns>
    public static string GetLocalMacAddress()
    {
        var iPv4 = GetLocalUsedIPv4();
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        var interfaces = nics.Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up); // 所有网卡信息
                                                                                                                                                  //adapters = adapters.Where(a => a.GetIPProperties().IsDynamicDnsEnabled);  // DNS 网卡

        //var macAddress = adapters.Select(adapter => adapter.GetPhysicalAddress())
        //    .Select(address => string.Join("-", address.GetAddressBytes().Select(b => $"{b:X2}"))).ToList();
        var macAddress = "";
        foreach (NetworkInterface inter in interfaces)
        {
            var properties = inter.GetIPProperties();
            foreach (UnicastIPAddressInformation information in properties.UnicastAddresses)
            {
                if (information.Address.ToString() == iPv4)
                {
                    macAddress = string.Join("-", inter.GetPhysicalAddress().GetAddressBytes().Select(b => $"{b:X2}"));
                    break;
                }
            }
        }

        return macAddress;
    }
}
