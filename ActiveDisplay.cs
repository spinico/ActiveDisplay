namespace ActiveDisplay
{
    using Platform;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public class ActiveDisplay : IDisposable
    {
        private Window _window = null;
        private IntPtr _hWnd = IntPtr.Zero;
        private string _device = null;

        public bool InterfaceName { get; set; }

        public DisplayDevice Device
        {
            get
            {
                if (_hWnd != IntPtr.Zero)
                {
                    MonitorArea monitorArea = Helpers.GetMonitorArea(_hWnd);

                    return GetDisplayDevice(monitorArea.Device);
                }

                return null;
            }
        }

        public Window Window
        {
            get { return _window; }

            set
            {
                if (_window != null)
                {
                    Detach();
                }

                _hWnd = Attach(value);

                _window = value;
            }
        }

        private void Detach()
        {
            if (_hWnd != IntPtr.Zero)
            {
                HwndSource.FromHwnd(_hWnd).RemoveHook(WindowProc);
                _hWnd = IntPtr.Zero;

                _window = null;
            }
        }

        private IntPtr Attach(Window window)
        {
            IntPtr hWnd = IntPtr.Zero;

            if (window != null)
            {
                hWnd = new WindowInteropHelper(window).Handle;
                HwndSource.FromHwnd(hWnd).AddHook(WindowProc);
            }
            else
            {
                throw new ArgumentNullException("The window instance should not be null.");
            }

            return hWnd;
        }

        public event EventHandler DisplayDeviceChanged = delegate { };

        public ActiveDisplay()
        {
            InterfaceName = false;
        }

        public virtual IntPtr WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((WM)msg)
            {
                case WM.WINDOWPOSCHANGING:
                    {
                        WINDOWPOS windowPos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                        if (Helpers.WindowChanged(windowPos))
                        {
                            // Get the monitor area that intersect the most with the specified rectangle
                            MonitorArea monitorArea = Helpers.GetMonitorArea(hWnd, new RECT
                            {
                                left = windowPos.x,
                                top = windowPos.y,
                                right = windowPos.x + windowPos.cx,
                                bottom = windowPos.y + windowPos.cy
                            });

                            UpdateDisplayDevice(monitorArea.Device);
                        }

                        break;
                    }
            }

            return IntPtr.Zero;
        }

        private void UpdateDisplayDevice(string device)
        {
            if (_device != device)
            {
                _device = device;

                var displayDevice = GetDisplayDevice(device);

                DisplayDeviceChanged(this, new DisplayDeviceChangedEventArgs(displayDevice));
            }
        }

        private DisplayDevice GetDisplayDevice(string device)
        {
            var dd = SafeNativeMethods.GetDisplayDevice(device, InterfaceName);

            return new DisplayDevice(
                dd.DeviceID,
                dd.DeviceKey,
                device,
                dd.DeviceName,
                dd.DeviceString,
                dd.StateFlags);
        }

        #region IDisposable

        private bool _disposed = false;

        ~ActiveDisplay()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free other managed objects that 
                // implement IDisposable  
                if (_hWnd != IntPtr.Zero)
                {
                    HwndSource.FromHwnd(_hWnd).RemoveHook(WindowProc);
                    _hWnd = IntPtr.Zero;
                }

                _window = null;
            }

            // Release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
