using Hikari.Common.Hardware.Components;
using System.Collections.Generic;

namespace Hikari.Common.Hardware.Retrieval;

internal interface IHardwareInfo
{
    MemoryStatusInfo GetMemoryStatus();

    List<BatteryInfo> GetBatteryList();
    List<BiosInfo> GetBiosList();
    List<CpuInfo> GetCpuList(bool includePercentProcessorTime = true);
    List<Hikari.Common.Hardware.Components.DriveInfo> GetDriveList();
    List<KeyboardInfo> GetKeyboardList();
    List<MemoryInfo> GetMemoryList();
    List<MonitorInfo> GetMonitorList();
    List<MotherboardInfo> GetMotherboardList();
    List<MouseInfo> GetMouseList();
    List<NetworkAdapterInfo> GetNetworkAdapterList(bool includeBytesPersec = true, bool includeNetworkAdapterConfiguration = true);
    List<PrinterInfo> GetPrinterList();
    List<SoundDeviceInfo> GetSoundDeviceList();
    List<VideoControllerInfo> GetVideoControllerList();
}
