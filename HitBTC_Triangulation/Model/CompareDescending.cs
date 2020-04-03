using System;
using System.Collections.Generic;

namespace HitBTC_Triangulation.Model
{
    class CompareDescending: IComparer<decimal>
    {
        public int Compare(decimal x, decimal y)
        {
         
            if (x > y)
                return -1;
            else if (x == y)
                return 0;
            else if (x < y)
                return 1;
            else
                throw new Exception();
        }
    }
}
