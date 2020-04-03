using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace HitBTC.Net.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitSide
    {
        [EnumMember(Value = "buy")]
        Buy,
        [EnumMember(Value = "sell")]
        Sell
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitSort
    {
        [EnumMember(Value = "DESC")]
        Desc,
        [EnumMember(Value = "ASC")]
        Asc
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitPeriod
    {
        [EnumMember(Value = "M1")]
        Minute1,
        [EnumMember(Value = "M3")]
        Minute3,
        [EnumMember(Value = "M5")]
        Minute5,
        [EnumMember(Value = "M15")]
        Minute15,
        [EnumMember(Value = "M30")]
        Minute30,
        [EnumMember(Value = "H1")]
        Hour1,
        [EnumMember(Value = "H4")]
        Hour4,
        [EnumMember(Value = "D1")]
        Day1,
        [EnumMember(Value = "D7")]
        Day7,
        [EnumMember(Value = "1M")]
        Month1
    }

    [JsonConverter(typeof(StringEnumConverter))]
    internal enum HitRequestMethod
    {
        [EnumMember(Value = "getCurrency")]
        GetCurrency,
        [EnumMember(Value = "getCurrencies")]
        GetCurrencies,
        [EnumMember(Value = "getSymbol")]
        GetSymbol,
        [EnumMember(Value = "getSymbols")]
        GetSymbols,
        [EnumMember(Value = "subscribeTicker")]
        SubscribeTicker,
        [EnumMember(Value = "unsubscribeTicker")]
        UnsubscribeTicker,
        [EnumMember(Value = "subscribeOrderbook")]
        SubscribeOrderbook,
        [EnumMember(Value = "unsubscribeOrderbook")]
        UnsubscribeOrderbook,
        [EnumMember(Value = "subscribeTrades")]
        SubscribeTrades,
        [EnumMember(Value = "unsubscribeTrades")]
        UnsubscribeTrades,
        [EnumMember(Value = "getTrades")]
        GetTrades,
        [EnumMember(Value = "subscribeCandles")]
        SubscribeCandles,
        [EnumMember(Value = "unsubscribeCandles")]
        UnsubscribeCandles,
        [EnumMember(Value = "login")]
        Login,
        [EnumMember(Value = "subscribeReports")]
        SubscribeReports,
        [EnumMember(Value = "unsubscribeReports")]
        UnsubscribeReports,
        [EnumMember(Value = "newOrder")]
        NewOrder,
        [EnumMember(Value = "cancelOrder")]
        CancelOrder,
        [EnumMember(Value = "cancelReplaceOrder")]
        CancelReplaceOrder,
        [EnumMember(Value = "getOrders")]
        GetOrders,
        [EnumMember(Value = "getTradingBalance")]
        GetTradingBalance
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitNotificationMethod
    {
        [EnumMember(Value = "ticker")]
        Ticker,
        [EnumMember(Value = "snapshotOrderbook")]
        SnapshotOrderBook,
        [EnumMember(Value = "updateOrderbook")]
        UpdateOrderBook,
        [EnumMember(Value = "snapshotTrades")]
        SnapshotTrades,
        [EnumMember(Value = "updateTrades")]
        UpdateTrades,
        [EnumMember(Value = "snapshotCandles")]
        SnapshotCandles,
        [EnumMember(Value = "updateCandles")]
        UpdateCandles,
        [EnumMember(Value = "activeOrders")]
        ActiveOrders,
        [EnumMember(Value = "report")]
        Report,
    }

    public enum HitConnectionState
    {
        PrepareToConnect,
        Connecting,
        Connected,
        Failed,
        Disconnecting,
        Disconnected
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitOrderStatus
    {
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "suspended")]
        Suspended,
        [EnumMember(Value = "partiallyFilled")]
        PartiallyFilled,
        [EnumMember(Value = "filled")]
        Filled,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "expired")]
        Expired
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitOrderType
    {
        [EnumMember(Value = "limit")]
        Limit,
        [EnumMember(Value = "market")]
        Market,
        [EnumMember(Value = "stopLimit")]
        StopLimit,
        [EnumMember(Value = "stopMarket")]
        StopMarket
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitTimeInForce
    {
        [EnumMember(Value = "GTC")]
        GTC,
        [EnumMember(Value = "IOC")]
        IOC,
        [EnumMember(Value = "FOK")]
        FOK,
        [EnumMember(Value = "Day")]
        Day,
        [EnumMember(Value = "GTD")]
        GTD
    }

    internal enum HitBy
    {
        [EnumMember(Value = "timestamp")]
        Timestamp,
        [EnumMember(Value = "id")]
        Id
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitTransactionStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "success")]
        Success
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitTransactionType
    {
        [EnumMember(Value = "payout")]
        Payout,
        [EnumMember(Value = "payin")]
        Payin,
        [EnumMember(Value = "deposit")]
        Deposit,
        [EnumMember(Value = "withdraw")]
        Withdraw,
        [EnumMember(Value = "bankToExchange")]
        BankToExchange,
        [EnumMember(Value = "exchangeToBank")]
        ExchangeToBank
    }

    [JsonConverter(typeof(StringEnumConverter))]
    internal enum HitLoginAlgo
    {
        [EnumMember(Value = "BASIC")]
        Basic,
        [EnumMember(Value = "HS256")]
        SHA256
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HitReportType
    {
        [EnumMember(Value = "status")]
        Status,
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "canceled")]
        Canceled,
        [EnumMember(Value = "expired")]
        Expired,
        [EnumMember(Value = "suspended")]
        Suspended,
        [EnumMember(Value = "trade")]
        Trade,
        [EnumMember(Value = "replaced")]
        Replaced
    }
}