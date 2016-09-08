namespace ActiveDisplay.Platform
{
    using System;

    enum MONITOR : Int32
    {
        DEFAULTTONULL = 0,
        DEFAULTTOPRIMARY = 1,
        DEFAULTTONEAREST = 2
    }

    enum MONITORINFOF : UInt32
    {
        PRIMARY = 1
    }

    enum WM : Int32
    {
        WINDOWPOSCHANGING = 0x0046
    }

    [Flags]
    enum SWP : UInt32
    {
        NOSIZE       = 0x00000001,
        NOMOVE       = 0x00000002,
        NOZORDER     = 0x00000004,
        FRAMECHANGED = 0x00000020,

        /// <summary>
        /// Undocumented flag - Based on the window current position, this 
        /// flag indicates if the window frame will be docked to one or 
        /// more edges of the monitor
        /// </summary>
        /// <remarks>
        /// Only available on Windows 10
        /// </remarks>
        DOCKFRAME    = 0x00100000
    }

    [Flags]
    public enum EDDF : Int32
    {
        ATTACHED_TO_DESKTOP = 0x00000001, // Adapter
        ACTIVE              = 0x00000001, // Monitor
        MULTI_DRIVER        = 0x00000002, // Adapter
        ATTACHED            = 0x00000002, // Monitor
        PRIMARY_DEVICE      = 0x00000004,
        MIRRORING_DRIVER    = 0x00000008,
        VGA_COMPATIBLE      = 0x00000010,
        REMOVABLE           = 0x00000020,
        UNSAFE_MODES_ON     = 0x00080000, // Undocumented - Monitor unsafe modes is enabled (Vista+)
        TS_COMPATIBLE       = 0x00200000, // Undocumented - Terminal service compatible adapter
        UNDEF_0x01000000    = 0x01000000, // Undocumented - (Windows 10+ ?)
        DISCONNECT          = 0x02000000,
        REMOTE              = 0x04000000,
        MODESPRUNED         = 0x08000000
    }
}
