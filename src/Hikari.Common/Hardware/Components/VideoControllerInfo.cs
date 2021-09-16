using System;

namespace Hikari.Common.Hardware.Components
{
    /// <summary>
    /// WMI class: Win32_VideoController
    /// </summary>
    public class VideoControllerInfo
    {
        /// <summary>
        /// Memory size of the video adapter.
        /// </summary>
        public UInt32 AdapterRAM { get; set; }

        /// <summary>
        /// Short description of the object.
        /// </summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary>
        /// Number of bits used to display each pixel.
        /// </summary>
        public UInt32 CurrentBitsPerPixel { get; set; }

        /// <summary>
        /// Current number of horizontal pixels.
        /// </summary>
        public UInt32 CurrentHorizontalResolution { get; set; }

        /// <summary>
        /// Number of colors supported at the current resolution.
        /// </summary>
        public UInt64 CurrentNumberOfColors { get; set; }

        /// <summary>
        /// Frequency at which the video controller refreshes the image for the monitor. 
        /// A value of 0 (zero) indicates the default rate is being used, while 0xFFFFFFFF indicates the optimal rate is being used.
        /// </summary>
        public UInt32 CurrentRefreshRate { get; set; }

        /// <summary>
        /// Current number of vertical pixels.
        /// </summary>
        public UInt32 CurrentVerticalResolution { get; set; }

        /// <summary>
        /// Description of the object.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Last modification date and time of the currently installed video driver.
        /// </summary>
        public string DriverDate { get; set; } = string.Empty;

        /// <summary>
        /// Version number of the video driver.
        /// </summary>
        public string DriverVersion { get; set; } = string.Empty;

        /// <summary>
        /// Manufacturer of the video controller.
        /// </summary>
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// Maximum refresh rate of the video controller in hertz.
        /// </summary>
        public UInt32 MaxRefreshRate { get; set; }

        /// <summary>
        /// Minimum refresh rate of the video controller in hertz.
        /// </summary>
        public UInt32 MinRefreshRate { get; set; }

        /// <summary>
        /// Label by which the object is known.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Current resolution, color, and scan mode settings of the video controller.
        /// </summary>
        public string VideoModeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Free-form string describing the video processor.
        /// </summary>
        public string VideoProcessor { get; set; } = string.Empty;
    }
}