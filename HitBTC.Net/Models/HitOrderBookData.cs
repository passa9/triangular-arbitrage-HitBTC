using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitOrderBookData : HitOrderBook
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        /// <summary>
        /// Sequence is monotone increasing for each update, each symbol has its own sequence
        /// </summary>
        [JsonProperty("sequence")]
        public long Sequence { get; private set; }
    }
}