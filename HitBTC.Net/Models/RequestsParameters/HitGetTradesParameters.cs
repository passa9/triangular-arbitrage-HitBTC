using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetTradesParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetTrades;

        [JsonProperty("sort")]
        public HitSort? Sort { get; set; }  
             
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        
        [JsonProperty("offset")]
        public long? Offset { get; set; }

        [JsonProperty("by")]
        public string By { get; set; }
    }
}