using System;
using System.Collections.Generic;
using System.Text;

namespace FordFulkersonAlgorithm.Auxiliary
{
    public class Flow // Ford-Fulkerson
    {
        public int Cost { get; }
        public int[,] Vertexes { get; }
        public Flow(int cost, int[,] vertexes)
        {
            this.Cost = cost;
            this.Vertexes = vertexes;
        }
    }
}
