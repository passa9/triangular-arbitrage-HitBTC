using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HitBTC.Net;
using HitBTC.Net.Communication;
using HitBTC.Net.Models;

namespace HitBTC_Triangulation.Model
{
    /// <summary>
    /// Comparer for comparing two keys, handling equality as beeing greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DuplicateKeyComparer<TKey>
                    :
                 IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }

        #endregion
    }

    class HitBTCClient : Graph
    {

        private List<HitBTC.Net.Models.HitSymbol> _lstMarket;
        private HitRestApi _hitbtcRestClient;
        private HitSocketApi _hitbctSocketApi;
        private decimal _currentAmount;
        private decimal _updatedAmount;
        private decimal _waitingTime;
        public decimal CurrentAmount
        {
            get
            {
                return _currentAmount;
            }
        }
        public decimal UpdatedAmount
        {
            get
            {
                _currentAmount = _updatedAmount;
                return 0.003m;
                var result = _hitbtcRestClient.GetTradingBalancesAsync().Result.Result
                    .First(x => x.Currency == StartCoin).Available;
                _updatedAmount = result;
                return _updatedAmount;
            }
        }
        private int _countPairInizialized = 0;
        public decimal MinTotalTrade { get; set; } = 0.0001m;
        public decimal MinStartAmount { get; set; } = 0.0001m;
        public bool CanStart
        {
            get
            {
                return _countPairInizialized == _lstTickers.Count;
            }
        }

        public HitBTCClient(string startCoin, decimal waitingTime  = 0, string key = "74e9b6746389500ef873e87ce2478beb", string sign = "06eff83f9162e69f41fc87555c80527e", decimal feeSell = 0.00054m, decimal feeBuy = 0.00054m) : base(startCoin: startCoin, feeBuy: feeBuy, feeSell: feeSell)
        {
            _hitbtcRestClient = new HitRestApi(new HitConfig
            {
                ApiKey = key,
                Secret = sign
            });
            _hitbctSocketApi = new HitSocketApi(new HitConfig
            {
                ApiKey = key,
                Secret = sign,
            });

            _currentAmount = UpdatedAmount;
            _lstMarket = _hitbtcRestClient.GetSymbolsAsync().Result.Result.ToList();
            _waitingTime = waitingTime;

        }

        public void FillGraph()
        {
            try
            {
                _lstTickers = GetTicker();
                BuildGraph(_lstTickers);
                RemoveOptionalNode();

            }
            catch (Exception e)
            {
                using (StreamWriter writetext = File.AppendText("C:\\log\\log.txt"))
                {
                    writetext.Write("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + e.ToString() + "\r\n");
                    writetext.Close();
                }

                Console.WriteLine("Eccezione: " + e.ToString());
            }
        }

        public void InitWebSocket()
        {
            int count = 0;
            _hitbctSocketApi.ConnectAsync().ContinueWith(x =>
            {
                var resultT = _hitbctSocketApi.LoginAsync().Result;
                _lstTickers.ForEach(ticker =>
                {
                    System.Threading.Thread.Sleep(300);
                    _hitbctSocketApi.SubscribeOrderbookAsync(ticker.Symbol).ContinueWith(result =>
                    {
                        Console.WriteLine(++count + ") " + ticker.Symbol);
                        if (!result.Result.Result)
                            throw new Exception();
                    });
                });
            });
            _hitbctSocketApi.Notification += (api, args) =>
            {
                if (args.OrderBook != null)
                {
                    var orderbook = args.OrderBook;

                    UpdateOrderBook(orderbook.Symbol, orderbook.Bids.ToList(), orderbook.Asks.ToList(), orderbook.Sequence);

                    if (args.NotificationMethod == HitNotificationMethod.SnapshotOrderBook)
                    {
                        _countPairInizialized++;
                    }
                }
            };

            void PrintOrderbook(HitOrderBookData hitOrderBookData)
            {
                using (StreamWriter writetext = File.AppendText("C:\\log\\log.txt"))
                {
                    string text = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "]  " + hitOrderBookData.Sequence + "\r\n";

                    text += "BIDS \t\t\t\tASK\r\n";
                    int length = hitOrderBookData.Bids.Length > hitOrderBookData.Asks.Length
                        ? hitOrderBookData.Bids.Length
                        : hitOrderBookData.Asks.Length;

                    for (int j = 0; j < length; j++)
                    {
                        if (hitOrderBookData.Bids.Length - 1 < j)
                        {
                            text += string.Empty.PadRight(10) + " " + String.Empty.PadRight(10) + "\t\t\t\t";
                        }
                        else
                        {
                            text += hitOrderBookData.Bids[j].Price.ToString().PadRight(10) + " " + hitOrderBookData.Bids[j].Size.ToString().PadRight(10) + "\t\t\t\t";
                        }

                        if (hitOrderBookData.Asks.Length - 1 < j)
                        {
                            text += string.Empty.PadRight(10) + " " + String.Empty.PadRight(10) + "\r\n";
                        }
                        else
                        {
                            text += hitOrderBookData.Asks[j].Price.ToString().PadRight(10) + " " + hitOrderBookData.Asks[j].Size.ToString().PadRight(10) + "\r\n";
                        }
                    }

                    writetext.Write(text + "\r\n\r\n\r\n");
                    writetext.Close();
                }

            }

        }

        private bool Tradable(SnapshotPath path)
        {

            string baseCoin;
            string marketCoin;
            decimal minTradableVolume;
            decimal amount;
            for (int i = 0; i < path.LstStep.Count - 1; i++)
            {
                if (path.LstStep[i + 1].Action == "BUY")
                {
                    amount = path.LstStep[i + 1].Amount;
                    baseCoin = path.LstStep[i].Coin;
                    marketCoin = path.LstStep[i + 1].Coin;

                }
                else
                {
                    amount = path.LstStep[i].Amount;
                    baseCoin = path.LstStep[i + 1].Coin;
                    marketCoin = path.LstStep[i].Coin;
                }

                minTradableVolume = _lstMarket.First(x => x.QuoteCurrency.Equals(baseCoin) && x.BaseCurrency.Equals(marketCoin)).QuantityIncrement;
                if (amount < minTradableVolume)
                    return false;
            }

            return true;
        }

        public bool Tradable(Path path)
        {

            string baseCoin;
            string marketCoin;
            decimal minTradableVolume;
            decimal amount;
            for (int i = 0; i < path.LstStep.Count - 1; i++)
            {
                if (path.LstStep[i + 1].Action == "BUY")
                {
                    amount = path.LstStep[i + 1].Amount;
                    baseCoin = path.LstStep[i].Coin;
                    marketCoin = path.LstStep[i + 1].Coin;

                }
                else
                {
                    amount = path.LstStep[i].Amount;
                    baseCoin = path.LstStep[i + 1].Coin;
                    marketCoin = path.LstStep[i].Coin;
                }

                minTradableVolume = _lstMarket.First(x => x.QuoteCurrency.Equals(baseCoin) && x.BaseCurrency.Equals(marketCoin)).QuantityIncrement;
                if (amount < minTradableVolume)
                    return false;
            }

            return true;
        }

        public void WalkPath(Path path)
        {
            SnapshotPath snapshot = GetSnapshotPath(path);

            if (!(snapshot.Percentage > 100))
                return;
            if (!Tradable(snapshot))
            {
                return;
            }
            if (!snapshot.Tradable(MinTotalTrade))
                return;

            string tradeHistory = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\t";
            List<long> lstTimeElapsed = new List<long>();
            for (int i = 0; i < 5; i++)
            {
                if (snapshot.LstStep.Count > i)
                {
                    tradeHistory += snapshot.LstStep[i].Coin + "\t";
                }
                else
                    tradeHistory += "\t";
            }

            tradeHistory += snapshot.Percentage.ToString("0.#####") + "\t" + snapshot.StartVolume.ToString("0.########") + "\t" + snapshot.FinalVolume.ToString("0.########") + "\t";

            Stopwatch watch = new Stopwatch();
            Stopwatch watchTrade = new Stopwatch();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Path Selected: " + snapshot.ToString());
            Console.ResetColor();
            Console.WriteLine("Starting PATH...");
            for (int i = 0; i < snapshot.LstStep.Count - 1; i++)
            {
                watch.Restart();
                string output = (i + 1) + ")  ";
                output += (snapshot.LstStep[i].Coin + (snapshot.LstStep[i + 1].Action == "BUY" ? "-->" : "<--") + snapshot.LstStep[i + 1].Coin).PadRight(20);
                output += ("Amount: " + (snapshot.LstStep[i + 1].Action == "BUY" ? (snapshot.LstStep[i + 1].AmountWithOutFee.ToString("0.######") + snapshot.LstStep[i + 1].Coin) : (snapshot.LstStep[i].Amount.ToString("0.######")) + snapshot.LstStep[i].Coin)).PadRight(25);
                output += ("Price: " + snapshot.LstStep[i + 1].Price.ToString("0.######") + (snapshot.LstStep[i + 1].Action == "BUY" ? snapshot.LstStep[i].Coin : snapshot.LstStep[i + 1].Coin)).PadRight(25);
                output += "Total: " + (snapshot.LstStep[i + 1].Action == "BUY" ? (snapshot.LstStep[i].Amount.ToString("0.######") + snapshot.LstStep[i].Coin) : (snapshot.LstStep[i + 1].Amount.ToString("0.######") + snapshot.LstStep[i + 1].Coin));

                Console.WriteLine(output);
                HitOrder trade = null;
                watchTrade.Restart();
                trade = DoTrade(snapshot.LstStep[i], snapshot.LstStep[i + 1]);
                watchTrade.Stop();
                lstTimeElapsed.Add(watchTrade.ElapsedMilliseconds);

                HitOrder openOrder = null;
                bool isFilled = false;
                try
                {
                    do
                    {
                        HitResponse<HitOrder> resultOpenOrder;

                        if (trade.Status == HitOrderStatus.Filled)
                        {
                            isFilled = true;
                            Console.WriteLine("ALL bought/selled");
                            continue;
                        }

                        else
                        {

                            resultOpenOrder = _hitbtcRestClient.GetActiveOrderByClientIdAsync(trade.ClientOrderId).Result;
                            openOrder = resultOpenOrder.Result;

                            if (resultOpenOrder.Result == null || openOrder.Status == HitOrderStatus.Filled)
                            {
                                isFilled = true;
                                Console.WriteLine("ALL bought/selled");
                                continue;
                            }

                            Console.WriteLine(" Exchange: " + openOrder.Symbol + " OrderType: " + openOrder.OrderType + " isOpen: " + openOrder.Status);

                            System.Threading.Thread.Sleep(200);
                        }

                        if (watch.ElapsedMilliseconds > _waitingTime)
                        {
                            //  Reset();
                            tradeHistory += "NO" + "\t";
                            tradeHistory += UpdatedAmount + "\t";

                            foreach (var time in lstTimeElapsed)
                            {
                                tradeHistory += time + "\t";
                            }
                            tradeHistory += "\r\n";

                            using (StreamWriter writetext = File.AppendText("C:\\log\\tradehistory.txt"))
                            {
                                writetext.Write(tradeHistory);
                                writetext.Close();
                            }
                            return;
                        }
                    } while (!isFilled);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Eccezione while: " + e.ToString());

                    // Reset();
                    tradeHistory += "NO" + "\t";
                    tradeHistory += UpdatedAmount + "\t";

                    foreach (var time in lstTimeElapsed)
                    {
                        tradeHistory += time + "\t";
                    }
                    tradeHistory += "\r\n";

                    using (StreamWriter writetext = File.AppendText("C:\\log\\tradehistory.txt"))
                    {
                        writetext.Write(tradeHistory);
                        writetext.Close();
                    }
                    return;
                }

            }
            //  Reset();
            tradeHistory += "SI" + "\t";
            tradeHistory += UpdatedAmount + "\t";

            foreach (var time in lstTimeElapsed)
            {
                tradeHistory += time + "\t";
            }
            tradeHistory += "\r\n";

            using (StreamWriter writetext = File.AppendText("C:\\log\\tradehistory.txt"))
            {
                writetext.Write(tradeHistory);
                writetext.Close();
            }
        }
        private SnapshotPath GetSnapshotPath(Path path)
        {
            path.TotalVolume(path.MaxStartVolume < CurrentAmount ? path.MaxStartVolume : CurrentAmount);
            SnapshotPath snapshot = new SnapshotPath();
            foreach (Model.Step step in path.LstStep)
            {
                snapshot.LstStep.Add(new SnapshotStep()
                {
                    Action = step.Action,
                    Amount = step.Amount,
                    AmountWithOutFee = step.AmountWithOutFee,
                    Coin = step.Coin,
                    Price = step.Price,
                    QuantityIncrement = step.QuantityIncrement
                });
            }
            return snapshot;
        }

        private List<Model.TickerResponse> GetTicker()
        {
            var tickers = _hitbtcRestClient.GetTickersAsync().Result.Result.ToList();
            var lstData = new List<Model.TickerResponse>();
            var symbols = _hitbtcRestClient.GetSymbolsAsync().Result.Result.ToList();

            foreach (var symbol in symbols)
            {
                var ticker = tickers.SingleOrDefault(x => x.Symbol == symbol.Id);

                if (ticker == null)
                    continue;

                var pairParsed = new Model.TickerResponse()
                {
                    Symbol = symbol.Id,
                    QuantityIncrement = symbol.QuantityIncrement,
                    A = symbol.BaseCurrency,
                    B = symbol.QuoteCurrency,// BTC
                    BaseVolume = ticker.Volume ?? 0,
                    High24hr = ticker.High ?? 0,
                    HighestBid = ticker.Bid ?? 0,
                    Last = ticker.Last ?? 0,
                    Low24hr = ticker.Low ?? 0,
                    LowestAsk = ticker.Ask ?? 0,
                    PercentChange = 0,
                    QuoteVolume = ticker.Volume ?? 0,
                };
                lstData.Add(pairParsed);
            }
            return lstData;
        }


        // TRADING
        private HitOrder DoTrade(SnapshotStep currentStep, SnapshotStep nextStep)
        {
            var watch = new Stopwatch();
            string currencyPair = nextStep.Action == "SELL" ? (currentStep.Coin + nextStep.Coin) : (nextStep.Coin + currentStep.Coin);

            decimal amount = nextStep.Action == "BUY" ? nextStep.Amount : currentStep.Amount;
            amount = amount - (amount % nextStep.QuantityIncrement);

            Console.WriteLine("Amount: " + decimal.Round(amount, 8) + " Price: " + nextStep.Price + " Market: " + currencyPair + " Action: " + nextStep.Action);
            watch.Start();
            HitResponse<HitReport> placeOrder = _hitbctSocketApi.PlaceNewOrderAsync(currencyPair, nextStep.Action == "BUY" ? HitSide.Buy : HitSide.Sell, amount, nextStep.Price).Result;
            watch.Stop();
            var order = placeOrder.Result;
            Console.WriteLine("Time Elapsed: " + watch.ElapsedMilliseconds);

            return order;
        }
    }
}
