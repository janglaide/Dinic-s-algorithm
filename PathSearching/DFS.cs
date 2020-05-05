using System;
using System.Collections.Generic;
using System.Text;

namespace PathSearching
{
    public class DFS //used in Dinics algorithm
    {
        private List<bool> _closed = new List<bool>();
        private int _N;
        private int[,] _matrix;
        private int _from;
        private int _to;
        public DFS(int[,] matrix, int n, int from, int to)
        {
            _N = n;
            _matrix = CopyMatrix(matrix);
            for (var i = 0; i < _N; i++)
                _closed.Add(false);
            _from = from;
            _to = to;
        }
        public (int, List<int>) Run()
        {
            var result = new List<int>();
            result.Add(_from);
            var found = false;
            var cost = 0;
            DFSAlgorithm(ref result, _from, ref found, ref cost, -1);
            return (cost, result);
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
        private void DFSAlgorithm(ref List<int> path, int current, ref bool found, ref int cost, int previous)
        {
            if (current == _to)
            {
                found = true;
                return;
            }
            var next = GetNextNodes(current);
            if (next.Count != 0)
            {
                foreach (var x in next)
                {
                    path.Add(x);
                    cost += _matrix[current, x];
                    DFSAlgorithm(ref path, x, ref found, ref cost, current);
                    if (found)
                        break;
                }
                if (!found)
                {
                    path.Remove(current);
                    cost -= _matrix[previous, current];
                }

            }
            else
            {
                path.Remove(current);
                cost -= _matrix[previous, current];
                _closed[current] = true;
            }

        }
        private List<int> GetNextNodes(int current)
        {
            var list = new List<int>();
            for (var i = 0; i < _N; i++)
            {
                if (_matrix[current, i] != 0 && !_closed[i])
                    list.Add(i);
            }
            return list;
        }
    }
}
