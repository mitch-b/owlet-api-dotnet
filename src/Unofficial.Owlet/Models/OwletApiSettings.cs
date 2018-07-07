using System;
using System.Collections.Generic;
using System.Linq;
using Unofficial.Owlet.Interfaces;

namespace Unofficial.Owlet.Models
{
    public class OwletApiSettings : IOwletApiSettings
    {
        public OwletApiSettings()
        {
            this.FreshDataRetryPeriod = TimeSpan.FromSeconds(2);
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public TimeSpan? FreshDataRetryPeriod { get; set; }
        public int? AppActivePropertyId { get; set; } = null;
        public IEnumerable<string> KnownDeviceSerialNumbers { get; set; } = Enumerable.Empty<string>();
    }
}
