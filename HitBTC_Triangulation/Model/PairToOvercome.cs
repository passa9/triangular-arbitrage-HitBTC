using System.Collections.Generic;
using System.Linq;

namespace HitBTC_Triangulation.Model
{
    public class PairToOvercome
    {
        private Edge _e;
        public string StartCoin { get; private set; }
        public string EndCoin
        {
            get
            {
                return _e.Vertex.Coin;
            }
        }
        public string Action
        {
            get
            {
                return _e.Action;
            }
        }
        public decimal Fee
        {
            get
            {
                return _e.Fee;
            }
        }
        public decimal Price
        {
            get
            {
                return _e.LstOrderBook.First().Key;
            }
        }

        public List<Path> LstPath { get; set; }

        public PairToOvercome(string startCoin, ref Edge e)
        {
            _e = e;
            StartCoin = startCoin;
        }

    }
}