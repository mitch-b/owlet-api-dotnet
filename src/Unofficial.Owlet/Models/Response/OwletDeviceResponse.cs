using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Response
{
    public class OwletDeviceResponse
    {
        [JsonProperty("device")]
        public OwletDevice Device { get; set; }
    }
}
