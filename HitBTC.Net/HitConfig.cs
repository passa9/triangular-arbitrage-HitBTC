using System;

namespace HitBTC.Net
{
    public class HitConfig
    {
        public string RestApiEndpoint { get; set; }

        public string SocketEndpoint { get; set; }

        public string ApiKey { get; set; }

        public string Secret { get; set; }

        public TimeSpan Timeout { get; set; }

        public HitConfig()
        {
            this.RestApiEndpoint = "https://api.hitbtc.com/api/2/";
            this.SocketEndpoint = "wss://api.hitbtc.com/api/2/ws";
            this.Timeout = TimeSpan.FromSeconds(5);
        }
    }
}