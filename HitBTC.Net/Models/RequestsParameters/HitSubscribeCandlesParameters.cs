using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitSubscribeCandlesParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.SubscribeCandles;

        [JsonProperty("period")]
        public HitPeriod Period { get; set; }
    }
}