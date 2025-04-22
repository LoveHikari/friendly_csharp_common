using Hikari.Common.Hardware.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Hikari.Common.Hardware.Retrieval
{
    internal class HardwareInfoRetrievalBase
    {
        internal Process StartProcess(string cmd, string args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(cmd, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            return Process.Start(processStartInfo);
        }

        internal string ReadProcessOutput(string cmd, string args)
        {
            try
            {
                using Process process = StartProcess(cmd, args);
                using System.IO.StreamReader streamReader = process.StandardOutput;
                process.WaitForExit();

                return streamReader.ReadToEnd().Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        internal string TryReadFileText(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(path).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        internal string[] TryReadFileLines(string path)
        {
            try
            {
                return System.IO.File.ReadAllLines(path);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        public virtual List<Hikari.Common.Hardware.Components.DriveInfo> GetDriveList()
        {
            List<Hikari.Common.Hardware.Components.DriveInfo> driveList = new ();

            Hikari.Common.Hardware.Components.DriveInfo drive = new ();

            PartitionInfo partition = new ();

            foreach (System.IO.DriveInfo driveInfo in System.IO.DriveInfo.GetDrives())
            {
                VolumeInfo volume = new ()
                {
                    FileSystem = driveInfo.DriveFormat,
                    Description = driveInfo.DriveType.ToString(),
                    Name = driveInfo.Name,
                    Caption = driveInfo.RootDirectory.FullName,
                    FreeSpace = (ulong)driveInfo.TotalFreeSpace,
                    Size = (ulong)driveInfo.TotalSize,
                    VolumeName = driveInfo.VolumeLabel
                };

                partition.VolumeList.Add(volume);
            }

            drive.PartitionList.Add(partition);

            driveList.Add(drive);

            return driveList;
        }

        public virtual List<NetworkAdapterInfo> GetNetworkAdapterList(bool includeBytesPersec = true, bool includeNetworkAdapterConfiguration = true)
        {
            List<NetworkAdapterInfo> networkAdapterList = new ();

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                NetworkAdapterInfo networkAdapter = new ()
                {
                    MACAddress = networkInterface.GetPhysicalAddress().ToString().Trim(),
                    Description = networkInterface.Description.Trim(),
                    Name = networkInterface.Name.Trim()
                };

                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    networkAdapter.Speed = (ulong)networkInterface.Speed;
                }

                if (includeNetworkAdapterConfiguration)
                {
                    foreach (UnicastIPAddressInformation addressInformation in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            networkAdapter.IPAddressList.Add(addressInformation.Address);
                        }
                    }
                }

                networkAdapterList.Add(networkAdapter);
            }

            return networkAdapterList;
        }
    }
}