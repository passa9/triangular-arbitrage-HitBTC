using Newtonsoft.Json;

namespace HitBTC.Net.Models
{
    public class HitError
    {
        #region Consts

        public const int CODE_REQUEST_TIMEOUT = -100;
        public const int CODE_REQUEST_ABORTED = -101;
        public const int CODE_RESPONSE_PARSE_ERROR = -200;
        public const int CODE_RESPONSE_INVALID_TYPE = -201;

        public const int CODE_ACTION_FORBIDDEN_FOR_ACCOUNT = 403;
        public const int CODE_TOO_MANY_REQUESTS = 429;
        public const int CODE_INTERNAL_SERVER_ERROR = 500;
        public const int CODE_SERVICE_UNAVAILABLE = 503;
        public const int CODE_GATEWAY_TIMEOUT = 504;
        public const int CODE_AUTHORIZATION_REQUIRED = 1001;
        public const int CODE_AUTHORIZATION_FAILED = 1002;
        public const int CODE_ACTION_IS_FORBIDDEN_FOR_API_KEY = 1003;
        public const int CODE_UNSUPPORTED_AUTHORIZATION_METHOD = 1004;
        public const int CODE_SYMBOL_NOT_FOUND = 2001;
        public const int CODE_CURRENCY_NOT_FOUND = 2002;
        public const int CODE_QUANTITY_NOT_A_VALID_NUMBER = 2010;
        public const int CODE_QUANTITY_TOO_LOW = 2011;
        public const int CODE_BAD_QUANTITY = 2012;
        public const int CODE_PRICE_NOT_A_VALID_VALUE = 2020;
        public const int CODE_PRICE_TOO_LOW = 2021;
        public const int CODE_BAD_PRICE = 2022;
        public const int CODE_INSUFFICIENT_FUNDS = 20001;
        public const int CODE_ORDER_NOT_FOUND = 20002;
        public const int CODE_LIMIT_EXCEEDED = 20003;
        public const int CODE_TRANSACTION_NOT_FOUND = 20004;
        public const int CODE_PAYOUT_NOT_FOUND = 20005;
        public const int CODE_PAYOUT_ALREADY_COMMITED = 20006;
        public const int CODE_PAYOUT_ALREADY_ROLLED_BACK = 20007;
        public const int CODE_DUPLICATE_CLIENT_ORDER_ID = 20008;
        public const int CODE_PRICE_AND_QUANTITY_NOT_CHANGED = 20009;
        public const int CODE_EXCHANGE_TEMPORARY_CLOSED = 20010;
        public const int CODE_VALIDATION_ERROR = 10001;

        #endregion Consts

        /// <summary>
        /// Error code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Error description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        internal HitError()
        { }

        internal HitError(int code, string message, string description = "")
        {
            this.Code = code;
            this.Message = message;
            this.Description = description;
        }

        public override string ToString() => $"Code: {this.Code} | Msg: {this.Message} | Descr: {this.Description}";
    }
}