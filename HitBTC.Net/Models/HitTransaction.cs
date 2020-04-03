using System;
using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitTransaction
    {
        /// <summary>
        /// Unique identifier for Transaction as assigned by exchange
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Is the internal index value that represents when the entry was updated
        /// </summary>
        [JsonProperty("index")]
        public long Index { get; private set; }

        /// <summary>
        /// Currency
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; private set; }
        
        /// <summary>
        /// Transaction amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; private set; }
        
        /// <summary>
        /// Transaction fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; private set; }
        
        /// <summary>
        /// Transaction address
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; private set; }
        
        /// <summary>
        /// Transaction payment id
        /// </summary>
        [JsonProperty("paymentId")]
        public string PaymentId { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { get; private set; }

        /// <summary>
        /// Transaction status. pending, failed, success
        /// </summary>
        [JsonProperty("status")]
        public HitTransactionStatus Status { get; private set; }

        /// <summary>
        /// Transaction type. One of: payout - crypto withdraw transaction, payin - crypto deposit transaction, deposit, withdraw, bankToExchange, exchangeToBank
        /// </summary>
        [JsonProperty("type")]
        public HitTransactionType Type { get; private set; }
        
        /// <summary>
        /// Date and time when transaction was created
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; private set; }
        
        /// <summary>
        /// Transaction last update date and time
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; private set; }
    }
}