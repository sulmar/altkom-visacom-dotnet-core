namespace production.models
{
    public class Measure : Base
    {
        public string DeviceId { get; set; }
        public float Temperature { get; set; }
    }
}