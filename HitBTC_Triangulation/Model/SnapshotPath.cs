using System.Collections.Generic;
using System.Linq;

namespace HitBTC_Triangulation.Model
{
    class SnapshotPath
    {
        public decimal StartVolume { get
            {
                return LstStep.First().Amount;
            }
        }

        public decimal FinalVolume { get
            {
                return LstStep.Last().Amount;
            }
        }
        public decimal Percentage
        {
            get
            {
                return (FinalVolume / StartVolume) * 100;
            }
        }
        public bool Tradable(decimal minTotalAmount)
        {

            for (int i = 0; i < LstStep.Count -1; i++)
            {
                decimal total = LstStep[i + 1].Action == "BUY" ? LstStep[i].Amount : LstStep[i + 1].Amount;
                if (total < minTotalAmount)
                    return false;
            }

            return true;
        }


        public List<SnapshotStep> LstStep { get; set; } = new List<SnapshotStep>();

        public override string ToString()
        {
            string strPath = this.LstStep.First().Coin;

            for (int i = 1; i < this.LstStep.Count; i++)
            {
                strPath += ((this.LstStep[i].Action == "BUY" ? "-->" : "<--") + this.LstStep[i].Coin);
            }

            string report =  strPath.PadRight(40) + ("Perc: " + this.Percentage.ToString("0.#####") + "%").PadRight(31) + ("Start Volume: " +  StartVolume.ToString("0.########")).PadRight(26) + "End Volume: " + FinalVolume.ToString("0.########");        
             
            return report;
        }
    }

    public class SnapshotStep
    {
        public decimal Price { get; set; }
        public decimal QuantityIncrement { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountWithOutFee { get; set; }
        public string Coin { get; set; }
        public string Action { get; set; }
    }
}
