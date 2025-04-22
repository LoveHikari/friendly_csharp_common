namespace Hikari.Common.Hardware.Components;
/// <summary>
/// WMI class: Win32_BaseBoard
/// </summary>
public class MotherboardInfo
{
    /// <summary>
    /// Name of the organization responsible for producing the physical element.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Baseboard part number defined by the manufacturer.
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Manufacturer-allocated number used to identify the physical element.
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;
}
