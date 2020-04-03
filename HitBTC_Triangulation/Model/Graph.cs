using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitBTC.Net.Models;

namespace HitBTC_Triangulation.Model
{
    public abstract class Graph
    {
        protected List<Model.TickerResponse> _lstTickers;
        private decimal _feeBuy;
        private decimal _feeSell;
        protected List<Vertex> _lstAdj;
        protected List<Edge> _lstEdge;
        public string StartCoin { get; private set; }
        public int MaxRange { get; private set; }
        private List<PairToOvercome> _lstPairToOvercome { get; set; }

        protected Graph(string startCoin, decimal feeBuy, decimal feeSell)
        {
            StartCoin = startCoin;
            _feeBuy = feeBuy;
            _feeSell = feeSell;
            _lstAdj = new List<Vertex>();
            _lstEdge = new List<Edge>();
            _lstPairToOvercome = null;
        }

        public void RemoveOptionalNode()
        {
            var lstNodeUseless = _lstAdj.Where(x => x.LstEdge.Count <= 1).ToList();

            foreach (var node in lstNodeUseless)
            {
                _lstTickers.Remove(_lstTickers.FirstOrDefault(x => x.A.Equals(node.Coin)));

                _lstAdj.Remove(node);

                foreach (var node2 in _lstAdj)
                {
                    var edge = node2.LstEdge.FirstOrDefault(x => x.Vertex.Coin.Equals(node.Coin));
                    if (edge != null)
                        node2.LstEdge.Remove(edge);
                }
            }
        }
        /// <summary>
        /// Genera il grafo
        /// </summary>
        /// <param name="lstTickerResponse"></param>
        protected void BuildGraph(List<Model.TickerResponse> lstTickerResponse)
        {
            MaxRange = 0;
            _lstAdj = new List<Vertex>();
            _lstPairToOvercome = null;
            foreach (TickerResponse pairParsed in lstTickerResponse)
            {
                AddVertex(pairParsed.A);
                AddVertex(pairParsed.B);

                AddEdge(pairParsed.Symbol, pairParsed.A, pairParsed.B, "SELL", pairParsed.QuantityIncrement);
                AddEdge(pairParsed.Symbol, pairParsed.B, pairParsed.A, "BUY", pairParsed.QuantityIncrement);
            }
        }

        private bool AddVertex(string coin)
        {
            if (!_lstAdj.Any(x => x.Coin.Equals(coin)))
            {
                _lstAdj.Add(new Vertex()
                {
                    Coin = coin,
                    LstEdge = new List<Edge>()
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aggiunge collegamento al grafo
        /// </summary>
        /// <param name="coinA">es AMP</param>
        /// <param name="coinB">es BTC</param>
        /// <param name="action"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        protected void AddEdge(string symbol, string coinA, string coinB, string action, decimal quantityIncrement)
        {
            Vertex A = _lstAdj.FirstOrDefault(x => x.Coin.Equals(coinA));
            Vertex B = _lstAdj.FirstOrDefault(x => x.Coin.Equals(coinB));

            if (A == null || B == null)
                throw new Exception("VERTICI NON TROVATI!");

            if (A.LstEdge.Any(x => x.Vertex.Coin.Equals(coinB) && x.Action.Equals(action)))
                throw new Exception("ARCO NON TROVATO");
            IComparer<decimal> comparer = null;
            if (action.Equals("BUY"))
            {
                comparer = new Model.ComparerAscending();
            }
            else if (action.Equals("SELL"))
            {
                comparer = new Model.CompareDescending();
            }
            Edge e = new Edge()
            {
                ID = coinA + "-" + coinB,
                Symbol = symbol,
                Action = action,
                LstOrderBook = new SortedList<decimal, Quantity>(comparer),
                Vertex = B,
                Fee = action.Equals("BUY") ? _feeBuy : _feeSell,
                QuantityIncrement = quantityIncrement
            };

            _lstEdge.Add(e);
            A.LstEdge.Add(e);

        }

        /// <summary>
        /// Trova tutti i percorsi tra tutte le coppie  
        /// </summary>
        /// <param name="indexCoin"></param>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public void FindAllPathByCoin(int maxRange, string indexCoin = null)
        {
            try
            {
                StartCoin = indexCoin ?? StartCoin;
                MaxRange = maxRange;
                List<PairToOvercome> lstPairToOvercome = new List<PairToOvercome>();

                Vertex source = _lstAdj.FirstOrDefault(x => x.Coin.Equals(StartCoin));

                if (source == null)
                {
                    Console.WriteLine("Nodo sorgente non trovato!");
                    return;
                }

                // MULTITHREAD METHOD
                List<Task> lstTask = new List<Task>();
                foreach (var edge in source.LstEdge)
                {
                    lstTask.Add(Task.Run(() =>
                    {
                        Edge e = edge;

                        var pToOverCome = new PairToOvercome(source.Coin, ref e);

                        Edge VertexEdge = _lstAdj.First(x => x.Coin == edge.Vertex.Coin).LstEdge
                            .First(y => y.Vertex.Coin == source.Coin);

                        pToOverCome.LstPath = FindAllPathByPair(source, edge.Vertex, maxRange);

                        lstPairToOvercome.Add(pToOverCome);
                    }));
                }
                while (!lstTask.TrueForAll(x => x.Status == TaskStatus.RanToCompletion)) { }

                // SINGLE THREAD METHOD
                //foreach (var edge in source.LstEdge)
                //{
                //    Edge e = edge;

                //    var pToOverCome = new PairToOvercome(source.Coin, ref e);

                //    Edge VertexEdge = _lstAdj.First(x => x.Coin == edge.Vertex.Coin).LstEdge
                //        .First(y => y.Vertex.Coin == source.Coin);
                //    //    decimal amountToOvercome = edge.Action == "BUY" ? 1 / VertexEdge.LstOrderBook.First().Key  : 1 * VertexEdge.LstOrderBook.First().Key;
                //    pToOverCome.LstPath = FindAllPathByPair(source, edge.Vertex, maxRange);

                //    lstPairToOvercome.Add(pToOverCome);

                //}

                _lstPairToOvercome = lstPairToOvercome;

            }
            catch (Exception e)
            {
                Console.WriteLine("Eccezione: " + e.ToString());
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">esBTC-AMP</param>
        /// <param name="buys">gli ordini di buy</param>
        /// <param name="sells">gli ordini di sell</param>
        public void UpdateOrderBook(string symbol, List<HitOrderBookLevel> buys, List<HitOrderBookLevel> sells, long nounce)
        {

            try
            {
                Edge eBuy = _lstEdge.Single(x => x.Symbol == symbol && x.Action == "BUY");
                Edge eSell = _lstEdge.Single(x => x.Symbol == symbol && x.Action == "SELL");

                if (buys == null || buys.Count == 0 || eBuy.Nounce > nounce) // || 
                {

                }
                else if (eSell.Nounce > nounce)
                {
                    Console.WriteLine("ERROR eSell: symbol: " + symbol);
                }
                else
                {
                    Task.Run(() =>
                    {
                        eSell.Nounce = nounce;
                        foreach (var order in buys)
                        {
                            if (order.Size == 0)
                                eSell.LstOrderBook.Remove(order.Price);

                            else if (!eSell.LstOrderBook.ContainsKey(order.Price))
                                eSell.LstOrderBook.Add(order.Price, new Quantity(order.Size));
                            else // Prezzo gia presento, modifico l'ammonto
                                eSell.LstOrderBook[order.Price] = new Quantity(order.Size);
                        }
                    });

                }

                if (sells == null || sells.Count == 0)
                {
                }
                else if (eBuy.Nounce > nounce)
                {
                    Console.WriteLine("ERROR eBuy: symbol: " + symbol);
                }
                else
                {
                    Task.Run(() =>
                    {
                        eBuy.Nounce = nounce;
                        foreach (var order in sells)
                        {
                            if (order.Size == 0)
                                eBuy.LstOrderBook.Remove(order.Price);

                            else if (!eBuy.LstOrderBook.ContainsKey(order.Price))
                                eBuy.LstOrderBook.Add(order.Price, new Quantity(order.Size));
                            else // Prezzo gia presento, modifico l'ammonto
                                eBuy.LstOrderBook[order.Price] = new Quantity(order.Size);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.ToString());
            }
        }

        /// <summary>
        /// Ritorna il path piu redditizio
        /// </summary
        /// >
        /// <returns></returns>
        public Path GetBestPath()
        {
            try
            {
                _lstPairToOvercome.ForEach(x => x.LstPath = x.LstPath.OrderByDescending(y => y.Profit).ToList());
                return _lstPairToOvercome.OrderByDescending(x => x.LstPath.First().Profit).ToList().First().LstPath.First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Calcola tutti i percorsi nel range "maxRange" partendo dal nodo "source" e finendo a nodo "end"
        /// </summary>
        /// <param name="source">Nodo di partenza</param>
        /// <param name="end">Nodo di fine</param>
        /// <param name="maxRange">Range di ricerca</param>
        /// <param name="amountToOvercome">Ammonto da battere</param>
        /// <returns></returns>
        private List<Path> FindAllPathByPair(Vertex source, Vertex end, int maxRange)
        {

            Edge finaleEdge = end.LstEdge.First(x => x.Vertex.Coin == source.Coin);
            List<Path> lstPath = new List<Path>() { new Path(ref source, ref end) { LstStep = new List<Step>() { new Step(coin: source.Coin) } } };
            List<Tuple<int, Vertex>> queue = new List<Tuple<int, Vertex>>();
            queue.Add(new Tuple<int, Vertex>(0, source));

            while (queue.Count > 0)
            {
                int depth = queue.First().Item1;
                Vertex currentVertex = queue.First().Item2;
                queue.RemoveAt(0);

                if (currentVertex.Coin == source.Coin && depth != 0)
                    continue;

                if (currentVertex.Coin.Equals(end.Coin))
                    continue;

                if (depth > maxRange)
                    continue;

                var lstPathThatEndWithCurrentNode = lstPath.Where(x => x.LstStep.Last().Coin.Equals(currentVertex.Coin) && x.LstStep.Count <= maxRange).ToList(); // prendo i percorsi che terminano con il nodo corrente
                if (lstPathThatEndWithCurrentNode.Count == 0)
                    continue;
                foreach (var edge in currentVertex.LstEdge)
                {
                    int sum = depth + 1;
                    if (sum <= maxRange)
                        queue.Add(new Tuple<int, Vertex>(sum, edge.Vertex));

                    foreach (var path in lstPathThatEndWithCurrentNode)
                    {
                        Path newPath = new Path(ref source, ref end) { LstStep = new List<Step>(path.LstStep) };
                        Edge e = edge;
                        Step lastStep = newPath.LstStep.Last();
                        newPath.LstStep.Add(new Step(ref e, ref lastStep));
                        Console.WriteLine(newPath.ToSimpleString());
                        lstPath.Add(newPath);
                    }
                }
                lstPath.RemoveAll(x => lstPathThatEndWithCurrentNode.Contains(x));
            }

            var lstPathFinished = lstPath.Where(x => x.LstStep.Last().Coin.Equals(end.Coin)).ToList();
            lstPathFinished.ForEach(x =>
            {
                var prestep = x.LstStep.Last();
                x.LstStep.Add(new Step(ref finaleEdge, ref prestep));
               
            });

            return lstPathFinished;
        }
    }
}
