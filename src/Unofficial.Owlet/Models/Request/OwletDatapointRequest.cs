using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Request
{
    public class OwletDatapointRequest
    {
        public OwletDatapointRequest()
        {
        }
        public OwletDatapointRequest(object value)
        {
            Datapoint.Value = value;
        }
        [JsonProperty("datapoint")]
        public OwletDatapoint Datapoint { get; set; } = new OwletDatapoint();
    }

    public class OwletDatapoint
    {
        public OwletDatapoint()
        {
        }
        public OwletDatapoint(object value)
        {
            Value = value;
        }
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
