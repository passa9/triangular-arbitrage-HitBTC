using System;
using System.Collections.Generic;

namespace HitBTC_Triangulation.Model
{
    public class Edge
    {
        public string ID { get; set; }
        public string Symbol { get; set; }
        public long Nounce { get; set; } = 0;
        public Vertex Vertex { get; set; }
        public string Action { get; set; }
        public decimal QuantityIncrement { get; set; }

        /// <summary>
        /// KEY: PRICE
        /// VALUE: AMOUNT
        /// </summary>
        public SortedList<decimal, Quantity> LstOrderBook { get; set; }
        public decimal Fee { get; set; }
    }

    public class Quantity
    {
        public Quantity(decimal size)
        {
            this.Size = size;
            this.Timestamp = DateTime.Now;
        }

        public decimal Size { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return Size.ToString() + "|" + Timestamp.ToString("HH:mm:ss");
        }
    }
}
