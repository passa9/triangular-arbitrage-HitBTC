using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetCurrencyParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetCurrency;

        [JsonProperty("currency")]
        public string Currency { get; set; }

        public override string ToString() => this.Currency;
    }
}