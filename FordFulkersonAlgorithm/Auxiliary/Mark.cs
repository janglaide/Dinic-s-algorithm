using System;
using System.Collections.Generic;
using System.Text;

namespace FordFulkersonAlgorithm.Auxiliary
{
    public class Mark
    {
        public int a { get; }
        public int vertexFrom { get; }
        public Mark(int a, int vertexFrom)
        {
            this.a = a;
            this.vertexFrom = vertexFrom;
        }
        public override string ToString() => $"[{a}, {vertexFrom}]";
    }
}
