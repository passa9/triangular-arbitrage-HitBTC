using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitUnsubscribeCandlesParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.UnsubscribeCandles;

        [JsonProperty("period")]
        public HitPeriod Period { get; set; }
    }
}