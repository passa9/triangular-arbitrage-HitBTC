using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitBalance
    {
        /// <summary>
        /// Currency name
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; private set; }

        /// <summary>
        /// Amount available for trading or transfer to main account
        /// </summary>
        [JsonProperty("available")]
        public decimal Available { get; private set; }

        /// <summary>
        /// Amount reserved for active orders or incomplete transfers to main account
        /// </summary>
        [JsonProperty("reserved")]
        public decimal Reserved { get; private set; }

        public override string ToString() => $"{this.Currency} | Available: {this.Available} | Reserved: {this.Reserved}";
    }
}