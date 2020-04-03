using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitUserTrade : HitTrade
    {
        /// <summary>
        /// Trading symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        /// <summary>
        /// Unique identifier for Order as assigned by trader.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; private set; }

        /// <summary>
        /// Unique identifier for Order as assigned by exchange
        /// </summary>
        [JsonProperty("orderId")]
        public long OrderId { get; private set; }

        /// <summary>
        /// Trade commission. Could be negative – reward. Fee currency see in symbol config
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; private set; }
    }
}