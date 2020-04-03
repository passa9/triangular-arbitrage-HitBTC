using HitBTC.Net.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HitBTC.Net.Communication
{
    public class HitNotification
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; private set; }

        [JsonProperty("method")]
        public HitNotificationMethod Method { get; private set; }

        internal static HitNotification TryParse(HitNotificationMethod method, JObject jObject)
        {
            switch(method)
            {
                case HitNotificationMethod.Ticker:
                    return jObject.ToObject<HitNotification<HitTicker>>();
                case HitNotificationMethod.SnapshotOrderBook:
                case HitNotificationMethod.UpdateOrderBook:
                    return jObject.ToObject<HitNotification<HitOrderBookData>>();
                case HitNotificationMethod.SnapshotTrades:
                case HitNotificationMethod.UpdateTrades:
                    return jObject.ToObject<HitNotification<HitTradesData>>();
                case HitNotificationMethod.SnapshotCandles:
                case HitNotificationMethod.UpdateCandles:
                    return jObject.ToObject<HitNotification<HitCandleData>>();
                case HitNotificationMethod.ActiveOrders:
                    return jObject.ToObject<HitNotification<HitOrder[]>>();
                case HitNotificationMethod.Report:
                    return jObject.ToObject<HitNotification<HitReport>>();
                default:
                    return null;
            }
        }

        public override string ToString() => $"{this.JsonRpc} | {this.Method}";
    }

    public class HitNotification<T> : HitNotification
    {
        [JsonProperty("params")]
        public T Params { get; private set; }

        public override string ToString() => $"{base.ToString()} | {this.Params}";
    }
}
