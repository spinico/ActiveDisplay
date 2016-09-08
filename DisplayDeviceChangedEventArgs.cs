namespace ActiveDisplay
{
    using System;

    public class DisplayDeviceChangedEventArgs : EventArgs
    {
        public DisplayDevice DisplayDevice { get; }

        public DisplayDeviceChangedEventArgs(DisplayDevice displayDevice)
        {
            DisplayDevice = displayDevice;
        }
    }
}
