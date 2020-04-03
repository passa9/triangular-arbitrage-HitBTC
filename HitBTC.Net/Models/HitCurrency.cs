using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitCurrency
    {
        /// <summary>
        /// Currency identifier. In the future, the description will simply use the currency
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Currency full name
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; private set; }

        /// <summary>
        /// Is currency belongs to blockchain (false for ICO and fiat, like EUR)
        /// </summary>
        [JsonProperty("crypto")]
        public bool Crypto { get; private set; }
        
        /// <summary>
        /// Is allowed for deposit (false for ICO)
        /// </summary>
        [JsonProperty("payinEnabled")]
        public bool PayingEnabled { get; private set; }

        /// <summary>
        /// Is required to provide additional information other than the address for deposit
        /// </summary>
        [JsonProperty("payinPaymentId")]
        public bool PayingPaymentId { get; private set; }

        /// <summary>
        /// Blocks confirmations count for deposit
        /// </summary>
        [JsonProperty("payinConfirmations")]
        public int PayingConfirmations { get; private set; }

        /// <summary>
        /// Is allowed for withdraw (false for ICO)
        /// </summary>
        [JsonProperty("payoutEnabled")]
        public bool PayoutEnabled { get; private set; }

        /// <summary>
        /// Is allowed to provide additional information for withdraw
        /// </summary>
        [JsonProperty("payoutIsPaymentId")]
        public bool PayoutIsPaymentId { get; private set; }

        /// <summary>
        /// Is allowed to transfer between trading and account (may be disabled on maintain)
        /// </summary>
        [JsonProperty("transferEnabled")]
        public bool TransferEnabled { get; private set; }
        
        /// <summary>
        /// True if currency delisted (stopped deposit and trading)
        /// </summary>
        [JsonProperty("delisted")]
        public bool Delisted { get; private set; }

        /// <summary>
        /// Default withdraw fee
        /// </summary>
        [JsonProperty("payoutFee")]
        public decimal PayoutFee { get; private set; }

        public override string ToString() => $"{this.Id} | {this.FullName}{(this.Delisted ? " | Delisted" : string.Empty)}";
    }
}