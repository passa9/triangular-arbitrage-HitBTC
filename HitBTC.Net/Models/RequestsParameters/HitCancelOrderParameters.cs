using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitCancelOrderParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.CancelOrder;

        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
    }
}