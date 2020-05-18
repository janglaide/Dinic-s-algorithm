using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public abstract class Algorithm
    {
        protected int _N;
        protected int _A;
        protected int _B;
        public Algorithm(int A, int B, int N)
        {
            _N = N;
            _A = A;
            _B = B;
        }
        private int[,] CopyMatrix(int[,] matrix)
        {
            var m = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    m[i, j] = matrix[i, j];
            }
            return m;
        }
    }
}
