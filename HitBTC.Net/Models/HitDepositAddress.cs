using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitDepositAddress
    {
        /// <summary>
        /// Address for deposit
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; private set; }

        /// <summary>
        /// Optional additional parameter. Required for deposit if persist
        /// </summary>
        [JsonProperty("paymentId")]
        public string PaymentId { get; private set; }
    }
}