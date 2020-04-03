using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitCandleData
    {
        /// <summary>
        /// Array of candles
        /// </summary>
        [JsonProperty("data")]
        public HitCandle[] Data { get; private set; }
        
        /// <summary>
        /// Candles symbol name
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; private set; }
        
        /// <summary>
        /// Candles period
        /// </summary>
        [JsonProperty("period")]
        public HitPeriod Period { get; private set; }
    }
}