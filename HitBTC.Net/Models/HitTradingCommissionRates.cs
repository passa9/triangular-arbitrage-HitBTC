using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitTradingCommissionRates
    {
        /// <summary>
        /// Take liquidity rate
        /// </summary>
        [JsonProperty("takeLiquidityRate")]
        public decimal TakeLiquidityRate { get; private set; }
        
        /// <summary>
        /// Provide liquidity rate
        /// </summary>
        [JsonProperty("provideLiquidityRate")]
        public decimal ProvideLiquidityRate { get; private set; }
    }
}