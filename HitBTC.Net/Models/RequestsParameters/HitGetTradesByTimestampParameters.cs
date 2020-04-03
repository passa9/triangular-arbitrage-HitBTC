using System;
using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetTradesByTimestampParameters : HitGetTradesParameters
    {
        [JsonProperty("from")]
        public DateTime? FromTimestamp { get; set; }
        
        [JsonProperty("till")]
        public DateTime? ToTimestamp { get; set; }
    }
}