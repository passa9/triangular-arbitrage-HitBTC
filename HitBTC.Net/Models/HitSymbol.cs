using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitSymbol
    {
        /// <summary>
        /// Symbol identifier. In the future, the description will simply use the symbol
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Base currency name
        /// </summary>
        [JsonProperty("baseCurrency")]
        public string BaseCurrency { get; private set; }

        /// <summary>
        /// Quoting currency name
        /// </summary>
        [JsonProperty("quoteCurrency")]
        public string QuoteCurrency { get; private set; }

        /// <summary>
        /// Quantity increment
        /// </summary>
        [JsonProperty("quantityIncrement")]
        public decimal QuantityIncrement { get; private set; }

        /// <summary>
        /// Tick size (minimum price change)
        /// </summary>
        [JsonProperty("tickSize")]
        public decimal TickSize { get; private set; }

        /// <summary>
        /// Take liquidity rate
        /// </summary>
        [JsonProperty("takeLiquidityRate")]
        public decimal TakeLiquidityRate { get; private set; }

        /// <summary>
        /// Provide liquiity rate
        /// </summary>
        [JsonProperty("provideLiquidityRate")]
        public decimal ProvideLiquidityRate { get; private set; }

        /// <summary>
        /// Fee currency name
        /// </summary>
        [JsonProperty("feeCurrency")]
        public string FeeCurrency { get; private set; }

        public override string ToString() => $"{this.Id} | {this.BaseCurrency} | {this.QuoteCurrency}";
    }
}