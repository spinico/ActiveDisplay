namespace ActiveDisplay.Platform
{
    using System;

    internal static class Helpers
    {
        internal static Func<SWP, bool> WindowChanged = (flags) =>
        {
            return (flags & SWP.NOMOVE) != SWP.NOMOVE ||
                   (flags & SWP.NOSIZE) != SWP.NOSIZE;
        };

        internal static MonitorArea GetMonitorArea(IntPtr hWnd)
        {
            return SafeNativeMethods.GetMonitorAreaFromWindow(hWnd);
        }

        internal static MonitorArea GetMonitorArea(IntPtr hWnd, RECT? rc)
        {
            return rc != null ? SafeNativeMethods.GetMonitorAreaFromRectangle((RECT)rc) :
                                SafeNativeMethods.GetMonitorAreaFromWindow(hWnd);
        }
    }
}
