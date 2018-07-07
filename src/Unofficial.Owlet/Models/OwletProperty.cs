using System;
using Newtonsoft.Json;

namespace Unofficial.Owlet.Models
{
    public class OwletProperty
    {
        public OwletProperty()
        {
        }
        [JsonProperty("data_updated_at")]
        public string DataUpdatedAtString { get; set; }
        public DateTimeOffset? DataUpdatedAt
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DataUpdatedAtString) || DataUpdatedAtString == "null")
                {
                    return null;
                }
                var parsedDateTimeOffset = default(DateTimeOffset);
                DateTimeOffset.TryParse(DataUpdatedAtString, out parsedDateTimeOffset);
                return parsedDateTimeOffset.ToLocalTime();
            }
        }
        [JsonProperty("key")]
        public int Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("base_type")]
        public string BaseType { get; set; }
        [JsonProperty("read_only")]
        public bool ReadOnly { get; set; }
        [JsonProperty("device_key")]
        public long DeviceKey { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }

        public T GetValue<T>() where T : IConvertible
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
