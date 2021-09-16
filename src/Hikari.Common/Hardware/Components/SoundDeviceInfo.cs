namespace Hikari.Common.Hardware.Components
{
    /// <summary>
    /// WMI class: Win32_SoundDevice
    /// </summary>
    public class SoundDeviceInfo
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
        /// Manufacturer of the sound device.
        /// </summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// Label by which the object is known.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product name of the sound device.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
    }
}