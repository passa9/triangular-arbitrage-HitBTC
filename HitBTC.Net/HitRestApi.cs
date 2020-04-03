using HitBTC.Net.Communication;
using HitBTC.Net.Models;
using HitBTC.Net.Models.RequestsParameters;
using HitBTC.Net.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HitBTC.Net
{
    public class HitRestApi
    {
        private readonly HitConfig hitConfig;

        private readonly HttpClient httpClient;

        public HitRestApi(HitConfig hitConfig = null)
        {
            this.hitConfig = hitConfig ?? new HitConfig();

            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri(this.hitConfig.RestApiEndpoint)
            };
        }

        #region Public Rest API
        /// <summary>
        /// Returns the actual list of available currencies, tokens, ICO etc.
        /// </summary>
        /// <returns></returns>
        public async Task<HitResponse<HitCurrency[]>> GetCurrenciesAsync(CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitCurrency[]>(HttpMethod.Get, cancellationToken, "public/currency");

        /// <summary>
        /// Returns the specific currency, token, ICO etc.
        /// </summary>
        /// <param name="currency">Currency name (e.g. "BTC")</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitCurrency>> GetCurrencyAsync(string currency, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitCurrency>(HttpMethod.Get, cancellationToken, "public/currency", currency);

        /// <summary>
        /// Returns the actual list of currency symbols (currency pairs) traded on HitBTC exchange. 
        /// The first listed currency of a symbol is called the base currency, and the second currency is called the quote currency. 
        /// The currency pair indicates how much of the quote currency is needed to purchase one unit of the base currency.
        /// </summary>
        /// <returns></returns>
        public async Task<HitResponse<HitSymbol[]>> GetSymbolsAsync(CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitSymbol[]>(HttpMethod.Get, cancellationToken, "public/symbol");

        /// <summary>
        /// Returns the specific currency pair traded on HitBTC exchange. 
        /// The first listed currency of a symbol is called the base currency, and the second currency is called the quote currency. 
        /// The currency pair indicates how much of the quote currency is needed to purchase one unit of the base currency.
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSD")</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitSymbol>> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitSymbol>(HttpMethod.Get, cancellationToken, "public/symbol", symbol);

        /// <summary>
        /// Returns ticker information
        /// </summary>
        /// <returns></returns>
        public async Task<HitResponse<HitTicker[]>> GetTickersAsync(CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitTicker[]>(HttpMethod.Get, cancellationToken, "public/ticker");

        /// <summary>
        /// Returns ticker information
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSDT")</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTicker>> GetTickerAsync(string symbol, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitTicker>(HttpMethod.Get, cancellationToken, "public/ticker", symbol);

        /// <summary>
        /// Returns trades list by symbol and timestamp borders
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSDT")</param>
        /// <param name="sort">Sort direction. Accepted values: ASC, DESC. Default DESC</param>
        /// <param name="from">From timestamp</param>
        /// <param name="till">Till timestamp</param>
        /// <param name="limit">Number of results per call. Accepted values: 0 - 1000. Default 100</param>
        /// <param name="offset">Offset in trade id from last trade in requested interval</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTrade[]>> GetTradesByTimestampAsync(string symbol, HitSort? sort = null, DateTime? from = null, DateTime? till = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitTrade[]>(
                HttpMethod.Get,
                cancellationToken,
                "public/trades",
                $"{symbol}?" +
                $"{sort.TryCreateParameter("sort")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}");

        /// <summary>
        /// Returns trades list by symbol and ids borders
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSDT")</param>
        /// <param name="sort">Sort direction. Accepted values: ASC, DESC. Default DESC</param>
        /// <param name="from">From id</param>
        /// <param name="till">Till id</param>
        /// <param name="limit">Number of results per call. Accepted values: 0 - 1000. Default 100</param>
        /// <param name="offset">Offset in trade id from last trade in requested interval</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTrade[]>> GetTradesByIdsAsync(string symbol, HitSort? sort = null, long? from = null, long? till = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitTrade[]>(
                HttpMethod.Get,
                cancellationToken,
                "public/trades",
                $"{symbol}?" +
                $"{HitBy.Id.TryCreateParameter("by")}" +
                $"{sort.TryCreateParameter("sort")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}");

        /// <summary>
        /// An order book is an electronic list of buy and sell orders for a specific symbol, organized by price level.
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSDT")</param>
        /// <param name="limit">Limit of orderbook levels, default 100. Set 0 to view full orderbook levels</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitOrderBook>(
                HttpMethod.Get,
                cancellationToken,
                "public/orderbook",
                $"{symbol}?" +
                $"{limit.TryCreateParameter("limit")}");

        /// <summary>
        /// An candles used for OHLC a specific symbol. Result contain candles only with non zero volume.
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSDT")</param>
        /// <param name="period">Candle period. From Minute1 to Month1</param>
        /// <param name="limit">Number of results per call. Accepted values: 0 - 1000. Default 100</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitCandle[]>> GetCandlesAsync(string symbol, HitPeriod? period = null, int? limit = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAsync<HitCandle[]>(
                HttpMethod.Get,
                cancellationToken,
                "public/candles",
                $"{symbol}?" +
                $"{period.TryCreateParameter("period")}" +
                $"{limit.TryCreateParameter("limit")}");
        #endregion Public Rest API

        #region Authentication Rest API
        /// <summary>
        /// Returns array of trading balances
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitBalance[]>> GetTradingBalancesAsync(CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitBalance[]>(HttpMethod.Get, cancellationToken, "trading/balance");

        /// <summary>
        /// Returns array of active orders
        /// </summary>
        /// <param name="symbol">Optional parameter to filter active orders by symbol</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder[]>> GetActiveOrdersAsync(string symbol = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitOrder[]>(
                HttpMethod.Get, 
                cancellationToken, 
                "order",
                $"{symbol?.TryCreateParameter("symbol")?.TrimEnd('&') ?? string.Empty}",
                separator: '?');

        /// <summary>
        /// Returns active order by specified id
        /// </summary>
        /// <param name="clientOrderId">Unique identifier for Order as assigned by trader. Uniqueness must be guaranteed within a single trading day, including all active orders.</param>
        /// <param name="wait">Optional parameter. Time in milliseconds. Max 60000. Default none. Use long polling request, if order is filled, canceled or expired return order info instantly, or after specified wait time returns actual order info</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder>> GetActiveOrderByClientIdAsync(string clientOrderId, int? wait = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitOrder>(
                HttpMethod.Get,
                cancellationToken,
                "order",
                $"{clientOrderId}?" +
                $"{wait.TryCreateParameter("wait")}");

        /// <summary>
        /// Place new order
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="side">sell buy</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Order price. Required for limit types.</param>
        /// <param name="stopPrice">Required for stop types.</param>
        /// <param name="timeInForce">Optional. Default - GDC. One of: GTC, IOC, FOK, Day, GTD</param>
        /// <param name="expireTime">Required for GTD timeInForce.</param>
        /// <param name="clientOrderId">Optional parameter, if skipped - will be generated by server. Uniqueness must be guaranteed within a single trading day, including all active orders.</param>
        /// <param name="strictValidate">Price and quantity will be checked that they increment within tick size and quantity step. See symbol tickSize and quantityIncrement</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder>> CreateNewOrderAsync(string symbol, HitSide side, decimal quantity, decimal price = -1, decimal stopPrice = -1,
            HitTimeInForce timeInForce = HitTimeInForce.Day, DateTime expireTime = default, string clientOrderId = null, bool strictValidate = true, CancellationToken cancellationToken = default)
        {
            HitOrderType orderType = HitOrderType.Market;

            if (price != -1 && stopPrice == -1)
                orderType = HitOrderType.Limit;
            else if (price == -1 && stopPrice != -1)
                orderType = HitOrderType.StopMarket;
            else if (price != -1 && stopPrice != -1)
                orderType = HitOrderType.StopLimit;

            var parameters = new HitNewOrderParameters
            {
                ClientOrderId = clientOrderId,
                Symbol = symbol,
                Side = side,
                OrderType = orderType,
                TimeInForce = timeInForce,
                Quantity = quantity,
                Price = price,
                StopPrice = stopPrice,
                ExpireTime = expireTime,
                StrictValidate = strictValidate
            };

            return await this.MakeRequestAuthorizedAsync<HitOrder>(HttpMethod.Post, cancellationToken, "order", postParameters: parameters);
        }

        /// <summary>
        /// Cancel all active orders, or all active orders for specified symbol.
        /// </summary>
        /// <param name="symbol">Optional parameter to filter active orders by symbol</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder[]>> CancelAllOrdersAsync(string symbol = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitOrder[]>(
                HttpMethod.Delete, 
                cancellationToken, 
                "order",
                $"{symbol?.TryCreateParameter("symbol")?.TrimEnd('&') ?? string.Empty}",
                separator: '?');

        /// <summary>
        /// Cancel order with specified clientOrderId
        /// </summary>
        /// <param name="clientOrderId">Order id</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder>> CancelOrderAsync(string clientOrderId, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitOrder>(
                HttpMethod.Delete, 
                cancellationToken,
                "order",
                $"{clientOrderId}");

        /// <summary>
        /// Get personal trading commission rate.
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTradingCommissionRates>> GetTradingCommissionAsync(string symbol, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitTradingCommissionRates>(
                HttpMethod.Get, 
                cancellationToken,
                "trading/fee",
                $"{symbol}");

        /// <summary>
        /// Returns orders history for account. Please note, that trading history may be updated with delay up to 30 seconds, with mean delay around 1 second.
        /// </summary>
        /// <param name="symbol">Optional parameter to filter orders by symbol</param>
        /// <param name="clientOrderId">If set, other parameters will be ignored. Without limit and pagination</param>
        /// <param name="from">From date</param>
        /// <param name="till">Till date</param>
        /// <param name="limit">Max orders count. Default 100</param>
        /// <param name="offset">Offset</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitOrder[]>> GetOrdersHistoryAsync(string symbol = null, string clientOrderId = null, DateTime? from = null, DateTime? till = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default) => 
            await this.MakeRequestAuthorizedAsync<HitOrder[]>(
                HttpMethod.Get, 
                cancellationToken,
                "history/order",
                $"{symbol.TryCreateParameter("symbol")}" +
                $"{clientOrderId.TryCreateParameter("clientOrderId")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}",
                separator: '?'
                );

        /// <summary>
        /// Returns trades history for account by timestamp borders
        /// </summary>
        /// <param name="symbol">Optional parameter to filter active orders by symbol</param>
        /// <param name="sort">DESC or ASC. Default value DESC</param>
        /// <param name="from">From date</param>
        /// <param name="till">Till date</param>
        /// <param name="limit">Max trades count. Default 100</param>
        /// <param name="offset">Offset</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitUserTrade[]>> GetUserTradesHistoryByTimestampAsync(string symbol = null, HitSort? sort = null, DateTime? from = null, DateTime? till = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default) => 
            await this.MakeRequestAuthorizedAsync<HitUserTrade[]>(
                HttpMethod.Get, 
                cancellationToken,
                "history/trades",
                $"{HitBy.Timestamp.TryCreateParameter("by")}" +
                $"{symbol.TryCreateParameter("symbol")}" +
                $"{sort.TryCreateParameter("clientOrderId")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}",
                separator: '?'
            );

        /// <summary>
        /// Returns trades history for account by trade id borders
        /// </summary>
        /// <param name="symbol">Optional parameter to filter active orders by symbol</param>
        /// <param name="sort">DESC or ASC. Default value DESC</param>
        /// <param name="from">From id</param>
        /// <param name="till">Till id</param>
        /// <param name="limit">Max trades count. Default 100</param>
        /// <param name="offset">Offset</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitUserTrade[]>> GetUserTradesHistoryByIdAsync(string symbol = null, HitSort? sort = null, long? from = null, long? till = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default) => 
            await this.MakeRequestAuthorizedAsync<HitUserTrade[]>(
                HttpMethod.Get, 
                cancellationToken,
                "history/trades",
                $"{HitBy.Id.TryCreateParameter("by")}" +
                $"{symbol.TryCreateParameter("symbol")}" +
                $"{sort.TryCreateParameter("clientOrderId")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}",
                separator: '?'
            );

        /// <summary>
        /// Returns trades that were occured by order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitUserTrade[]>> GetUserTradesByOrderAsync(long orderId, CancellationToken cancellationToken = default) => 
            await this.MakeRequestAuthorizedAsync<HitUserTrade[]>(HttpMethod.Get, cancellationToken, $"history/order/{orderId}/trades");

        /// <summary>
        /// Returns array of account balances
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitBalance[]>> GetAccountBalancesAsync(CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitBalance[]>(HttpMethod.Get, cancellationToken, "account/balance");

        /// <summary>
        /// Returns current address
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitDepositAddress>> GetCurrentDepositAddressAsync(string currency, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitDepositAddress>(HttpMethod.Get, cancellationToken, $"account/crypto/address/{currency}");

        /// <summary>
        /// Creates new deposit address
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitDepositAddress>> CreateDepositAddressAsync(string currency, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitDepositAddress>(HttpMethod.Post, cancellationToken, $"account/crypto/address/{currency}");

        /// <summary>
        /// Returns transactions history by timestamp borders
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="sort">DESC or ASC. Default value DESC</param>
        /// <param name="from">From date</param>
        /// <param name="till">Till date</param>
        /// <param name="limit">Max transactions count. Default 100</param>
        /// <param name="offset">Offset</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTransaction[]>> GetTransactionsHistoryByTimestampAsync(string currency = null, HitSort? sort = null, DateTime? from = null, DateTime? till = null,
            int? limit = null, int? offset = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitTransaction[]>(
                HttpMethod.Get, 
                cancellationToken,
                "account/transactions",
                $"{HitBy.Timestamp.TryCreateParameter("by")}" +
                $"{currency.TryCreateParameter("currency")}" +
                $"{sort.TryCreateParameter("sort")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}",
                separator: '?');

        /// <summary>
        /// Returns transactions history by id borders
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="sort">DESC or ASC. Default value DESC</param>
        /// <param name="from">From id</param>
        /// <param name="till">Till id</param>
        /// <param name="limit">Max transactions count. Default 100</param>
        /// <param name="offset">Offset</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTransaction[]>> GetTransactionsHistoryByIdAsync(string currency = null, HitSort? sort = null, long? from = null, long? till = null,
            int? limit = null, int? offset = null, CancellationToken cancellationToken = default) =>
            await this.MakeRequestAuthorizedAsync<HitTransaction[]>(
                HttpMethod.Get, 
                cancellationToken,
                "account/transactions",
                $"{HitBy.Id.TryCreateParameter("by")}" +
                $"{currency.TryCreateParameter("currency")}" +
                $"{sort.TryCreateParameter("sort")}" +
                $"{from.TryCreateParameter("from")}" +
                $"{till.TryCreateParameter("till")}" +
                $"{limit.TryCreateParameter("limit")}" +
                $"{offset.TryCreateParameter("offset")}",
                separator: '?');

        /// <summary>
        /// Returns transaction by id
        /// </summary>
        /// <param name="id">Transaction id</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitTransaction>> GetTransactionByIdAsync(string id, CancellationToken cancellationToken = default) => 
            await this.MakeRequestAuthorizedAsync<HitTransaction>(HttpMethod.Get, cancellationToken, $"account/transactions/{id}");
        #endregion Authentication Rest API

        #region Misc
        private async Task<HitResponse<T>> MakeRequestAsync<T>(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            var result = new HitResponse<T>();

            //if (this.forbidAllRequests)
            //{
            //    result.ErrorCode = BinanceErrors.REQUEST_FORBIDDEN_MANUALLY;
            //    result.ErrorMessage = $"All requests are forbidden manual. Approximate time to release - {CalculateRemainingForbiddenTime()}";

            //    return result;
            //}

            var response = await this.httpClient.SendAsync(httpRequestMessage, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return result;

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                result.Result = JsonConvert.DeserializeObject<T>(json);
            else
                result = JsonConvert.DeserializeObject<HitResponse<T>>(json);

            return result;
        }

        private async Task<HitResponse<T>> MakeRequestAuthorizedAsync<T>(HttpMethod httpMethod, CancellationToken cancellationToken, string endpoint, string parameters = "", HitRequestParameters postParameters = null, char separator = '/')
        {
            var httpRequestMessage = this.CreateHttpRequest(httpMethod, endpoint, parameters, postParameters: postParameters, separator: separator);

            var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(this.hitConfig.ApiKey + ":" + this.hitConfig.Secret));
            httpRequestMessage.Headers.Add("Authorization", "Basic " + encoded);

            return await this.MakeRequestAsync<T>(httpRequestMessage, cancellationToken);
        }

        private async Task<HitResponse<T>> MakeRequestAsync<T>(HttpMethod httpMethod, CancellationToken cancellationToken, string endpoint, string parameters = "")
        {
            var httpRequestMessage = this.CreateHttpRequest(httpMethod, endpoint, parameters);

            return await this.MakeRequestAsync<T>(httpRequestMessage, cancellationToken);
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod httpMethod, string endpoint, string parameters = "", HitRequestParameters postParameters = null, char separator = '/')
        {
            var requestUri = string.IsNullOrEmpty(parameters) ? endpoint : $"{endpoint}{separator}{parameters}";

            var requestMessage = new HttpRequestMessage(httpMethod, requestUri);

            if (postParameters != null)
            {
                string json = JsonConvert.SerializeObject(postParameters);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }  
            
            return requestMessage;
        }
        #endregion Misc
    }
}