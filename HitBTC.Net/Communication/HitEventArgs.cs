using HitBTC.Net.Models;
using System;

namespace HitBTC.Net.Communication
{
    public class HitEventArgs : EventArgs
    {
        public HitConnectionState ConnectionState { get; private set; }

        public Exception SocketError { get; private set; }

        public HitNotificationMethod NotificationMethod { get; private set; }

        public HitTicker Ticker { get; private set; }

        public HitOrderBookData OrderBook { get; private set; }

        public HitTradesData Trades { get; private set; }

        public HitCandleData Candles { get; private set; }

        public HitOrder[] ActiveOrders { get; private set; }

        public HitReport Report { get; private set; }

        internal HitEventArgs(HitConnectionState connectionState) => this.ConnectionState = connectionState;

        internal HitEventArgs(HitConnectionState connectionState, Exception socketError)
            : this(connectionState) => this.SocketError = socketError;

        internal HitEventArgs(HitNotification hitNotification)
        {
            this.NotificationMethod = hitNotification.Method;

            switch(hitNotification)
            {
                case HitNotification<HitTicker> ticker:
                    this.Ticker = ticker.Params;
                    break;
                case HitNotification<HitOrderBookData> orderBook:
                    this.OrderBook = orderBook.Params;
                    break;
                case HitNotification<HitTradesData> trades:
                    this.Trades = trades.Params;
                    break;
                case HitNotification<HitCandleData> candles:
                    this.Candles = candles.Params;
                    break;
                case HitNotification<HitOrder[]> orders:
                    this.ActiveOrders = orders.Params;
                    break;
                case HitNotification<HitReport> report:
                    this.Report = report.Params;
                    break;
            }
        }
    }
}