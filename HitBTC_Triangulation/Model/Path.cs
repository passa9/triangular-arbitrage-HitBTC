using System;
using System.Collections.Generic;
using System.Linq;

namespace HitBTC_Triangulation.Model
{
    public class Path : ICloneable
    {
        Vertex _start;
        Vertex _end;
        public decimal Profit
        {
            get
            {
                try
                {
                    return TotalVolume() - MaxStartVolume;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public decimal _amountToOvercome
        {
            get
            {
                return LstStep.First().Amount;
            }
        }

        public Decimal Amount
        {
            get
            {
                return LstStep.Last().Amount;
            }
        }

        public decimal MaxStartVolume
        {
            get
            {
                decimal currentVolume = LstStep.Last().Volume;
                try
                {


                    for (int i = LstStep.Count - 1; i > 0; i--)
                    {
                        currentVolume = LstStep[i].Volume < currentVolume ? LstStep[i].Volume : currentVolume;
                        currentVolume = LstStep[i].Action == "BUY" ? (currentVolume * LstStep[i].Price + ((currentVolume * LstStep[i].Price) * LstStep[i].Fee)) : (currentVolume / LstStep[i].Price + ((currentVolume / LstStep[i].Price) * LstStep[i].Fee));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                return currentVolume;

            }
        }

        public decimal TotalVolume(decimal? startVolume = null)
        {
            decimal volume = startVolume == null ? MaxStartVolume : startVolume.Value;
            LstStep.First().SetStartAmount(volume);
            return LstStep.Last().Amount;

        }

        public decimal Percentage
        {
            get
            {
                return (this.Amount * 100) / (_amountToOvercome == 0 ? -1 : _amountToOvercome);
            }
        }
        public List<Step> LstStep { get; set; }

        public Path(ref Vertex start, ref Vertex end)
        {
            _start = start;
            _end = end;
            LstStep = new List<Step>();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string ToSimpleString()
        {

            string strPath = this.LstStep.First().Coin;

            for (int i = 1; i < this.LstStep.Count; i++)
            {
                strPath += ((this.LstStep[i].Action == "BUY" ? "-->" : "<--") + this.LstStep[i].Coin);
            }

            return strPath;
        }

        public override string ToString()
        {
            string strPath = this.LstStep.First().Coin;

            for (int i = 1; i < this.LstStep.Count; i++)
            {
                strPath += ((this.LstStep[i].Action == "BUY" ? "-->" : "<--") + this.LstStep[i].Coin);
            }

            string report = DateTime.Now.ToString("[dd/MM/yyyy HH:mm:ss]  ") + strPath.PadRight(40) + (this.Percentage.ToString("0.#####") + "%").PadRight(25) + ("Max volume: " + this.MaxStartVolume.ToString("0.#######") + LstStep.First().Coin).PadRight(30) + ("Total: " + TotalVolume().ToString("0.#######") + LstStep.First().Coin).PadRight(30) + "Profit: " + (TotalVolume() - MaxStartVolume).ToString("0.##########") + LstStep.First().Coin;

            return report;
        }

        public string ToStringCSV()
        {
            string strPath = this.LstStep.First().Coin;

            for (int i = 1; i < this.LstStep.Count; i++)
            {
                strPath += "\t" + this.LstStep[i].Coin;
            }

            int numOfTab = 7 - LstStep.Count;

            for (int i = 0; i < numOfTab; i++)
                strPath += "\t";

            string report = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss\t") + strPath + (this.Percentage.ToString("0.#####") + "\t") + (this.MaxStartVolume.ToString("0.#######")) + "\t" + (TotalVolume().ToString("0.#######") + "\t" + (TotalVolume() - MaxStartVolume).ToString("0.#######"));

            return report;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Path path = (Model.Path)obj;

            if (LstStep.Count != path.LstStep.Count)
                return false;

            for (int i = 0; i < LstStep.Count; i++)
            {
                if (LstStep[i].Coin != path.LstStep[i].Coin)
                    return false;
            }

            return true;
        }
    }
}
