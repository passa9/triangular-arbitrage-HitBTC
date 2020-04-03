using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetSymbolParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetSymbol;

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        public override string ToString() => this.Symbol;
    }
}