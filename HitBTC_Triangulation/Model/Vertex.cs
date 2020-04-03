using System.Collections.Generic;

namespace HitBTC_Triangulation.Model
{
   public class Vertex
    {
        public string Coin { get; set; }
        public List<Edge> LstEdge { get; set; } = new List<Edge>();
    }
}
