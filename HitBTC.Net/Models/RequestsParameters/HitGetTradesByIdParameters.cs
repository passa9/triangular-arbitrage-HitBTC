using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetTradesByIdParameters : HitGetTradesParameters
    {
        [JsonProperty("from")]
        public long? FromId { get; set; }
        
        [JsonProperty("till")]
        public long? ToId { get; set; }   
    }
}