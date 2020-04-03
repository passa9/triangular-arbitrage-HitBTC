using System;
using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitNewOrderParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.NewOrder;
        
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("side")]
        public HitSide Side { get; set; }
        
        [JsonProperty("type")]
        public HitOrderType OrderType { get; set; }
        
        [JsonProperty("timeInForce")]
        public HitTimeInForce TimeInForce { get; set; }
        
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }
        
        [JsonProperty("price")]
        public decimal Price { get; set; }
        
        [JsonProperty("stopPrice")]
        public decimal StopPrice { get; set; }
        
        [JsonProperty("expireTime")]
        public DateTime ExpireTime { get; set; }
        
        [JsonProperty("strictValidate")]
        public bool StrictValidate { get; set; }
    }
}