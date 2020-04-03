using Newtonsoft.Json;
using System;

namespace HitBTC.Net.Models
{
    public class HitCandle
    {
        /// <summary>
        /// Candle timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("open")]
        public decimal Open { get; private set; }

        /// <summary>
        /// Close price
        /// </summary>
        [JsonProperty("close")]
        public decimal Close { get; private set; }

        /// <summary>
        /// Min price
        /// </summary>
        [JsonProperty("min")]
        public decimal Min { get; private set; }

        /// <summary>
        /// Max price
        /// </summary>
        [JsonProperty("max")]
        public decimal Max { get; private set; }

        /// <summary>
        /// Volume in base currency
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; private set; }

        /// <summary>
        /// Volume in quote currency
        /// </summary>
        [JsonProperty("volumeQuote")]
        public decimal VolumeQuote { get; private set; }

        public override string ToString() => $"{this.Timestamp} | O: {this.Open} | H: {this.Max} | L: {this.Min} | C: {this.Close} | BaseV: {this.Volume} | QuoteV: {this.VolumeQuote}";
    }
}