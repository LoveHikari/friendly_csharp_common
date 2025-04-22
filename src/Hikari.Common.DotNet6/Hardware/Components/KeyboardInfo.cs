using System;

namespace Hikari.Common.Hardware.Components;
/// <summary>
/// WMI class: Win32_Keyboard
/// </summary>
public class KeyboardInfo
{
    /// <summary>
    /// Short description of the object a one-line string.
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Description of the object.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Label by which the object is known
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Number of function keys on the keyboard.
    /// </summary>
    public UInt16 NumberOfFunctionKeys { get; set; }
}
