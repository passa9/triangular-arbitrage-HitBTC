using System.Collections.Generic;

namespace HitBTC_Triangulation.Model
{
    public class Step
    {
        private decimal _startAmount = 1;
        private Edge _e;
        private Step _preStep;

        public void SetStartAmount(decimal value)
        {
            _startAmount = value;
        }
        public decimal Fee
        {
            get
            {
                return _e.Fee;
            }
        }
        public decimal QuantityIncrement
        {
            get
            {
                return _e.QuantityIncrement;
            }
        }
        public string Coin
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
        public decimal Price
        {
            get
            {
                return _e.LstOrderBook.Count == 0 ? -1 : _e.LstOrderBook.Keys[0];
            }
        }
        public decimal Amount
        {
            get
            {
                var amount = ((Action == "BUY" ? _precAmount / Price : _precAmount * Price) -
                        (Action == "BUY" ? _precAmount / Price : _precAmount * Price) * Fee);

                return amount;

            }
        }
        public decimal AmountWithOutFee
        {
            get
            {
                return (Action == "BUY" ? _precAmount / Price : _precAmount * Price);
            }
        }
        private decimal _precAmount
        {
            get
            {
                return _preStep == null ? _startAmount : _preStep.Amount;
            }
        }

        public decimal Volume
        {
            get
            {
                return _e.LstOrderBook.Count == 0 ? -1 : _e.LstOrderBook.Values[0].Size;
            }
        }

        public Step(ref Edge e, ref Step preStep)
        {
            _e = e;
            _preStep = preStep;
        }
        public Step(string coin)
        {
            _preStep = null;
            _startAmount = 1;
            _e = new Edge();
            _e.Action = null;
            _e.Fee = 0;
            _e.Vertex = new Vertex()
            {
                Coin = coin
            };

            _e.LstOrderBook = new SortedList<decimal, Quantity>();
            _e.LstOrderBook.Add(1, new Quantity(1));
        }
    }
}
