namespace ActiveDisplay
{
    using Platform;

    public class DisplayDevice
    {
        public string Id { get; }
        public string Key { get; }
        public string Device { get; }
        public string Monitor { get; }
        public string Name { get; }
        public EDDF State { get; }

        public DisplayDevice(string id, string key, string device, string monitor, string name, EDDF state)
        {
            Id = id;
            Key = key;
            Device = device;
            Monitor = monitor;
            Name = name;
            State = state;
        }
    }
}
