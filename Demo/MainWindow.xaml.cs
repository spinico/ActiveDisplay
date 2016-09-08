namespace Demo
{
    using ActiveDisplay;
    using System;
    using System.ComponentModel;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private ActiveDisplay _activeDisplay;
        private bool _interfaceName = false;

        /// <summary>
        /// Set to true to get the GUID device interface name
        /// </summary>
        public bool InterfaceName
        {
            get { return _interfaceName; }

            set
            {
                _interfaceName = value;

                _activeDisplay.InterfaceName = _interfaceName;

                // Update display device data
                DisplayDevice = _activeDisplay.Device;
            }
        }

        #region Dependency Properties

        public DisplayDevice DisplayDevice
        {
            get { return (DisplayDevice)GetValue(DisplayDeviceProperty); }
            set { SetValue(DisplayDeviceProperty, value); }
        }
        public static readonly DependencyProperty DisplayDeviceProperty =
            DependencyProperty.Register("DisplayDevice", typeof(DisplayDevice), typeof(MainWindow),
                new PropertyMetadata(null));

        #endregion Dependency Properties

        public MainWindow()
        {
            InitializeComponent();

            _activeDisplay = new ActiveDisplay();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _activeDisplay.Window = this;
            _activeDisplay.DisplayDeviceChanged += DisplayDeviceChanged;

            // Initial display device data
            DisplayDevice = _activeDisplay.Device;
        }

        private void DisplayDeviceChanged(object sender, EventArgs e)
        {
            DisplayDevice = (e as DisplayDeviceChangedEventArgs).DisplayDevice;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_activeDisplay != null)
            {
                _activeDisplay.DisplayDeviceChanged -= DisplayDeviceChanged;
                _activeDisplay.Dispose();
                _activeDisplay = null;
            }
        }
    }
}
