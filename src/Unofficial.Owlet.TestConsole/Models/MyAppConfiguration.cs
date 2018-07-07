using System.Collections.Generic;

namespace Unofficial.Owlet.TestConsole.Models
{
    public class MyAppConfiguration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> KnownDeviceSerialNumbers { get; set; }
    }
}
