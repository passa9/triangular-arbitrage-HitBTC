using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitOrderBook
    {
        /// <summary>
        /// Array of asks
        /// </summary>
        [JsonProperty("ask")]
        public HitOrderBookLevel[] Asks { get; private set; }

        /// <summary>
        /// Array of bids
        /// </summary>
        [JsonProperty("bid")]
        public HitOrderBookLevel[] Bids { get; private set; }

        public override string ToString() => $"Asks count: {this.Asks.Length} | Bids count: {this.Bids}";
    }
}