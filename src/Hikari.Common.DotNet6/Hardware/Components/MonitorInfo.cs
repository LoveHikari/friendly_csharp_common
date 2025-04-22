﻿using System;

namespace Hikari.Common.Hardware.Components;
/// <summary>
/// WMI class: Win32_DesktopMonitor
/// </summary>
public class MonitorInfo
{
    /// <summary>
    /// Short description of the object.
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Description of the object.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Name of the monitor manufacturer.
    /// </summary>
    public string MonitorManufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Type of monitor.
    /// </summary>
    public string MonitorType { get; set; } = string.Empty;

    /// <summary>
    /// Label by which the object is known.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Resolution along the x-axis (horizontal direction) of the monitor.
    /// </summary>
    public UInt32 PixelsPerXLogicalInch { get; set; }

    /// <summary>
    /// Resolution along the y-axis (vertical direction) of the monitor.
    /// </summary>
    public UInt32 PixelsPerYLogicalInch { get; set; }
}
