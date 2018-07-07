using Newtonsoft.Json;

namespace Unofficial.Owlet.Models
{
    public class OwletDevice
    {
        public OwletDevice()
        {
        }

        [JsonProperty("dsn")]
        public string DeviceSerialNumber { get; set; }
        [JsonProperty("key")]
        public int Key { get; set; }
        [JsonProperty("connection_status")]
        public string ConnectionStatus { get; set; }
    }
}
