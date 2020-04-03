using HitBTC.Net.Communication;
using HitBTC.Net.Models;
using HitBTC.Net.Models.RequestsParameters;
using HitBTC.Net.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace HitBTC.Net
{
    public class HitSocketApi
    {
        #region Properties
        private readonly HitConfig hitConfig;

        private readonly WebSocket socket;
        private Exception lastSocketError;

        private readonly Dictionary<string, IHitResponseWaiter> waitersCache;

        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Current connection state
        /// </summary>
        public HitConnectionState ConnectionState
        {
            get => this.connectionState;
            set
            {
                if (this.connectionState == value)
                    return;

                this.connectionState = value;

                this.OnConnectionStateChanged(this.connectionState);
            }
        }
        private HitConnectionState connectionState;

        /// <summary>
        /// Occurs when connection state changed
        /// </summary>
        public event HitEventHandler ConnectionStateChanged;
        
        /// <summary>
        /// Occurs when new notification (e.g. ticker or book) has arrived
        /// </summary>
        public event HitEventHandler Notification;
        #endregion Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hitConfig">Optional parameter Needs only if you want to change default configs</param>
        public HitSocketApi(HitConfig hitConfig = null)
        {
            this.hitConfig = hitConfig ?? new HitConfig();
            this.waitersCache = new Dictionary<string, IHitResponseWaiter>();

            this.socket = new WebSocket(this.hitConfig.SocketEndpoint, sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);

            this.socket.Opened += this.Socket_Opened;
            this.socket.MessageReceived += this.Socket_MessageReceived;
            this.socket.Error += this.Socket_Error;
            this.socket.Closed += this.Socket_Closed;

            this.ConnectionState = HitConnectionState.PrepareToConnect;

            this.serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Opens socket connection
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            this.ConnectionState = HitConnectionState.Connecting;

            await Task.Factory.StartNew(() => 
            {
                this.socket.Open();

                while (this.ConnectionState == HitConnectionState.Connecting)
                    Task.Delay(100);
            });
        }

        /// <summary>
        /// Closes socket connection
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectAsync()
        {
            this.ConnectionState = HitConnectionState.Disconnecting;

            await Task.Factory.StartNew(() =>
            {
                this.socket.Close();

                while (this.ConnectionState == HitConnectionState.Disconnecting)
                    Task.Delay(100);
            });
        }

        #region Market data
        /// <summary>
        /// Returns the specific currency, token, ICO etc.
        /// </summary>
        /// <param name="currency">Currency name (e.g. "BTC")</param>
        /// <returns></returns>
        public async Task<HitResponse<HitCurrency>> GetCurrencyAsync(string currency, CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetCurrencyParameters() { Currency = currency };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitCurrency>(request, cancellationToken);
        }

        /// <summary>
        /// Returns the actual list of available currencies, tokens, ICO etc.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HitResponse<HitCurrency[]>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetCurrenciesParameters() { Currency = string.Empty };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitCurrency[]>(request, cancellationToken);
        }

        /// <summary>
        /// Returns the specific currency pair traded on HitBTC exchange. 
        /// The first listed currency of a symbol is called the base currency, and the second currency is called the quote currency. 
        /// The currency pair indicates how much of the quote currency is needed to purchase one unit of the base currency.
        /// </summary>
        /// <param name="symbol">Symbol name (e.g. "BTCUSD")</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitSymbol>> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetSymbolParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitSymbol>(request, cancellationToken);
        }

        /// <summary>
        /// Returns the actual list of currency symbols (currency pairs) traded on HitBTC exchange. 
        /// The first listed currency of a symbol is called the base currency, and the second currency is called the quote currency. 
        /// The currency pair indicates how much of the quote currency is needed to purchase one unit of the base currency.
        /// </summary>
        /// <returns></returns>
        public async Task<HitResponse<HitSymbol[]>> GetSymbolsAsync(CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetSymbolsParameters();

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitSymbol[]>(request, cancellationToken);
        }

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
        public async Task<HitResponse<HitTrade[]>> GetTradesByTimestampAsync(string symbol, HitSort? sort = null, DateTime? from = null, DateTime? to = null,
            int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetTradesByTimestampParameters()
            {
                Symbol = symbol,
                Sort = sort,
                By = HitBy.Timestamp.Format(),
                FromTimestamp = from,
                ToTimestamp = to,
                Limit = limit,
                Offset = offset
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitTrade[]>(request, cancellationToken);
        }

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
        public async Task<HitResponse<HitTrade[]>> GetTradesByIdAsync(string symbol, HitSort? sort = null, long? from = null, long? to = null,
            int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
        {
            var parameters = new HitGetTradesByIdParameters()
            {
                Symbol = symbol,
                Sort = sort,
                By = HitBy.Id.Format(),
                FromId = from,
                ToId = to,
                Limit = limit,
                Offset = offset
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitTrade[]>(request, cancellationToken);
        }
        #endregion Market data
        
        #region Subscriptions 
        public async Task<HitResponse<bool>> SubscribeTickerAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitSubscribeTickerParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> UnsubscribeTickerAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitUnsubscribeTickerParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> SubscribeOrderbookAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitSubscribeOrderbookParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> UnsubscribeOrderbookAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitUnsubscribeOrderbookParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> SubscribeTradesAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitSubscribeTradesParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> UnsubscribeTradesAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var parameters = new HitUnsubscribeTradesParameters() { Symbol = symbol };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> SubscribeCandlesAsync(string symbol, HitPeriod period, CancellationToken cancellationToken = default)
        {
            var parameters = new HitSubscribeCandlesParameters()
            {
                Symbol = symbol,
                Period = period
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        public async Task<HitResponse<bool>> UnsubscribeCandlesAsync(string symbol, HitPeriod period, CancellationToken cancellationToken = default)
        {
            var parameters = new HitUnsubscribeCandlesParameters()
            {
                Symbol = symbol,
                Period = period
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }
        #endregion Subscriptions

        #region Authentication
        /// <summary>
        /// Sends login request with API public key and API secret key which might be specified in <see cref="HitConfig"/>
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<bool>> LoginAsync(CancellationToken cancellationToken = default)
        {
            var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var parameters = new HitLoginParameters
            {
                Algo = HitLoginAlgo.SHA256,
                PublicKey = this.hitConfig.ApiKey,
                Nonce = nonce,
                Signature = ComputeHash(nonce, this.hitConfig.Secret)
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        /// <summary>
        /// Subscribes to any order updates such as open, modify, cancel, etc.
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<bool>> SubscribeReportsAsync(CancellationToken cancellationToken = default)
        {
            var parameters = new HitSubscribeReportsParameters();

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

        /// <summary>
        /// Unsubscribes from any order updates such as open, modify, cancel, etc.
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<bool>> UnsubscribeReportsAsync(CancellationToken cancellationToken = default)
        {
            var parameters = new HitUnsubscribeReportsParameters();

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<bool>(request, cancellationToken);
        }

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
        public async Task<HitResponse<HitReport>> PlaceNewOrderAsync(string symbol, HitSide side, decimal quantity, decimal price = -1, decimal stopPrice = -1,
            HitTimeInForce timeInForce = HitTimeInForce.Day, DateTime expireTime = default, string clientOrderId = null, bool strictValidate = true, CancellationToken cancellationToken = default)
        {
            HitOrderType orderType = HitOrderType.Market;

            if (price != -1 && stopPrice == -1)
                orderType = HitOrderType.Limit;
            else if (price == -1 && stopPrice != -1)
                orderType = HitOrderType.StopMarket;
            else if (price != -1 && stopPrice != -1)
                orderType = HitOrderType.StopLimit;

            if (clientOrderId == null)
                clientOrderId = GenerateId();

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

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitReport>(request, cancellationToken);
        }

        /// <summary>
        /// Cancel order with specified clientOrderId
        /// </summary>
        /// <param name="clientOrderId">Order id</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        public async Task<HitResponse<HitReport>> CancelOrderAsync(string clientOrderId, CancellationToken cancellationToken = default)
        {
            var parameters = new HitCancelOrderParameters
            {
                ClientOrderId = clientOrderId
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitReport>(request, cancellationToken);
        }

        /// <summary>
        /// Replace existing order
        /// </summary>
        /// <param name="clientOrderId">Required parameter. Replaced order</param>
        /// <param name="requestClientId">clientOrderId for new order Required parameter. Uniqueness must be guaranteed within a single trading day, including all active orders</param>
        /// <param name="quantity">New order quantity</param>
        /// <param name="price">New order price</param>
        /// <param name="strictValidate">Price and quantity will be checked that they increment within tick size and quantity step. See symbol tickSize and quantityIncrement</param>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitReport>> ReplaceOrderAsync(string clientOrderId, decimal quantity, decimal price, string requestClientId = null, bool strictValidate = true, CancellationToken cancellationToken = default)
        {
            if (requestClientId == null)
                requestClientId = GenerateId();

            var parameters = new HitReplaceOrderParameters
            {
                ClientOrderId = clientOrderId,
                RequestClientId = requestClientId,
                Quantity = quantity,
                Price = price,
                StrictValidate = strictValidate
            };

            var request = new HitRequest(parameters);

            return await this.MakeRequestAsync<HitReport>(request, cancellationToken);
        }

        /// <summary>
        /// Returns array of active orders
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitReport[]>> GetActiveOrdersAsync(CancellationToken cancellationToken = default)
        {
            var request = new HitRequest(new HitGetActiveOrdersParameters());

            return await this.MakeRequestAsync<HitReport[]>(request, cancellationToken);
        }

        /// <summary>
        /// Returns array of trading balances
        /// </summary>
        /// <param name="cancellationToken">Token for cancel operation</param>
        /// <returns></returns>
        public async Task<HitResponse<HitBalance[]>> GetTradingBalanceAsync(CancellationToken cancellationToken = default)
        {
            var request = new HitRequest(new HitGetTradingBalanceParameters());

            return await this.MakeRequestAsync<HitBalance[]>(request, cancellationToken);
        }
        #endregion Authentication


        private void Socket_Opened(object sender, EventArgs e) => this.ConnectionState = HitConnectionState.Connected;

        private void Socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                var jObject = JObject.Parse(e.Message);

                var method = jObject["method"]?.ToObject<HitNotificationMethod>();
                var id = jObject["id"]?.ToObject<string>();

                // Notification
                if (method != null)
                {
                    var notification = HitNotification.TryParse(method.Value, jObject);

                    if (notification != null) 
                        this.OnNotification(notification);
                }
                
                // Response
                if (id != null && this.waitersCache.TryGetValue(id, out var hitResponseWaiter))
                    hitResponseWaiter.TryParseResponse(jObject);
            }
            catch (Exception ex)
            {

            }
        }

        private void Socket_Error(object sender, ErrorEventArgs e)
        {
            this.lastSocketError = e.Exception;

            this.ConnectionState = HitConnectionState.Failed;
        }

        private void Socket_Closed(object sender, EventArgs e) => this.ConnectionState = HitConnectionState.Disconnected;


        #region Misc
        private async Task<HitResponse<T>> MakeRequestAsync<T>(HitRequest hitRequest, CancellationToken cancellationToken)
        {
            var waiter = this.CreateWaiter<T>(hitRequest.Id);

            var json = JsonConvert.SerializeObject(hitRequest, this.serializerSettings);

            this.socket.Send(json);

            await waiter.WaitAsync(cancellationToken);

            this.waitersCache.Remove(hitRequest.Id);

            return waiter.GetResponse<T>();
        }

        private void OnConnectionStateChanged(HitConnectionState connectionState) 
            => this.ConnectionStateChanged?.Invoke(this, new HitEventArgs(connectionState, connectionState == HitConnectionState.Failed ? this.lastSocketError : null));

        private void OnNotification(HitNotification hitNotification) => this.Notification?.Invoke(this, new HitEventArgs(hitNotification));

        private IHitResponseWaiter CreateWaiter<T>(string id)
        {
            var waiter = new HitResponseWaiter<T>(this.hitConfig.Timeout);

            this.waitersCache.Add(id, waiter);

            return waiter;
        }

        private static string ComputeHash(string text, string sign)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var signBytes = Encoding.UTF8.GetBytes(sign);

            byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(signBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static string GenerateId() => Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .TrimEnd('=')
            .Replace("+", "")
            .Replace(@"\", "")
            .Replace(@"/", "");
        #endregion Misc
    }

    public delegate void HitEventHandler(HitSocketApi hitSocketApi, HitEventArgs e);
}