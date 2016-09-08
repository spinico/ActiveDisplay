namespace ActiveDisplay.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// Completely harmless for any code, even malicious code, to call.
    /// Can be used just like other managed code.
    /// For example, a function that gets the time of day is typically safe.
    ///
    /// Naming Convention for Unmanaged Code Methods 
    /// <devdoc>http://msdn.microsoft.com/en-us/library/btadwd4w(vs.80).aspx</devdoc>    
    internal static class SafeNativeMethods
    {
        #region GetMonitorInfo

        internal static MonitorArea GetMonitorAreaFromWindow(IntPtr hWnd)
        {
            IntPtr hMonitor = MonitorFromWindow(hWnd, MONITOR.DEFAULTTONEAREST);

            return GetMonitorAreaEx(hMonitor);
        }

        internal static MonitorArea GetMonitorAreaFromPoint(POINT pt)
        {
            IntPtr hMonitor = MonitorFromPoint(pt, MONITOR.DEFAULTTONEAREST);

            return GetMonitorAreaEx(hMonitor);
        }

        internal static MonitorArea GetMonitorAreaFromRectangle(RECT rc)
        {
            IntPtr hMonitor = MonitorFromRect(ref rc, MONITOR.DEFAULTTONEAREST);

            return GetMonitorAreaEx(hMonitor);
        }

        static MonitorArea GetMonitorAreaEx(IntPtr hMonitor)
        {
            bool success = false;

            var mi = new MONITORINFOEX()
            {
                cbSize = Marshal.SizeOf(typeof(MONITORINFOEX))
            };

            if (hMonitor != IntPtr.Zero)
            {
                success = GetMonitorInfo(hMonitor, ref mi);
            }

            return success ? new MonitorArea(mi.rcMonitor, mi.rcWork, mi.szDevice) : null;
        }

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd144901.aspx</devdoc>
        [DllImport("user32.dll", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetMonitorInfo([In] IntPtr hMonitor, [In, Out] ref MONITORINFOEX lpmi);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd145064.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromWindow([In] IntPtr hWnd, [In] MONITOR dwFlags);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd145063.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromRect([In] ref RECT lprc, [In] MONITOR dwFlags);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd145062.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint([In] POINT pt, [In] MONITOR dwFlags);

        #endregion GetMonitoInfo

        #region EnumDisplayDevices

        /// <summary>
        /// Get the list of display devices
        /// </summary>
        /// <param name="interfaceName">Set to true to get the GUID device interface name</param>
        /// <returns>
        /// A list of DISPLAY_DEVICE instances
        /// </returns>
        internal static List<DISPLAY_DEVICE> GetDisplayDevices(bool interfaceName = false)
        {
            const uint EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

            var devices = new List<DISPLAY_DEVICE>();

            var displayAdapter = new DISPLAY_DEVICE()
            {
                cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE))
            };

            uint adapterNum = 0;

            while (EnumDisplayDevices(null, adapterNum, ref displayAdapter,
                interfaceName ? EDD_GET_DEVICE_INTERFACE_NAME : 0))
            {
                devices.Add(displayAdapter);

                var displayMonitor = new DISPLAY_DEVICE()
                {
                    cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE))
                };

                uint monitorNum = 0;

                while (EnumDisplayDevices(displayAdapter.DeviceName, monitorNum, ref displayMonitor,
                    interfaceName ? EDD_GET_DEVICE_INTERFACE_NAME : 0))
                {
                    devices.Add(displayMonitor);

                    monitorNum++;
                }

                adapterNum++;
            }

            return devices;
        }

        /// <summary>
        /// Get the display device info from a given device name
        /// </summary>
        /// <param name="device"></param>
        /// <param name="interfaceName">Set to true to get the GUID device interface name</param>
        /// <returns>
        /// A filled DISPLAY_DEVICE instance on success, otherwise a DISPLAY_DEVICE 
        /// instance with empty fields (only the 'cb' field will be set)
        /// </returns>
        internal static DISPLAY_DEVICE GetDisplayDevice(string device, bool interfaceName = false)
        {
            const uint EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

            var display = new DISPLAY_DEVICE()
            {
                cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE))
            };

            // First, try to get the corresponding display monitor
            bool success = EnumDisplayDevices(device, 0, ref display, interfaceName ? EDD_GET_DEVICE_INTERFACE_NAME : 0);

            if (!success)
            {
                // On failure, try to get the corresponding display adapter instead
                success = EnumDisplayDevices(null, 0, ref display, interfaceName ? EDD_GET_DEVICE_INTERFACE_NAME : 0);
            }

            return display;
        }

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd162609.aspx</devdoc>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumDisplayDevices([In] string lpDevice, [In] uint iDevNum, [In, Out] ref DISPLAY_DEVICE lpDisplayDevice, [In] uint dwFlags);

        #endregion EnumDisplayDevices
    }
}
