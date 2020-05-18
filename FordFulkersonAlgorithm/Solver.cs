using FordFulkersonAlgorithm.Auxiliary;
using System;
using System.Collections.Generic;

namespace FordFulkersonAlgorithm
{
    public class Solver
    {
        private Mark[] _marks;
        private int[,] _startMatrix;
        public int[,] StartMatrix { get => _startMatrix; }
        private int[,] _matrix;
        private int _N;
        private int _A;
        private int _B;
        private int _F;
        public Solver(int A, int B, int N, int[,] matrix)
        {
            _N = N;
            _A = A;
            _B = B;
            _F = 0;
            _matrix = CopyMatrix(matrix);
            _startMatrix = CopyMatrix(matrix);
        }
        public Flow FordFulkerson()
        {

            int i;
            //int flowCost;
            int[,] flowVertexes = new int[_N, _N];
            List<int> deletedVertex = new List<int>();
            _marks = new Mark[_N];

            //flowCost = 0;

            int[,] fMatrix = CopyMatrix(_matrix);

            i = _A;
            _marks[i] = new Mark(int.MaxValue, -1);
            do
            {
                List<int> S = new List<int>();
                for (int j = 0; j < _N; j++)
                {
                    if (!deletedVertex.Contains(j) && _matrix[i, j] > 0 && _marks[j] == null)
                    {
                        S.Add(j);
                    }
                }
                if (S.Count > 0)
                {
                    int max, nextVertex;
                    max = int.MinValue;
                    nextVertex = -1;
                    for (int j = 0; j < S.Count; j++)
                    {
                        if (_matrix[i, S[j]] > max)
                        {
                            max = _matrix[i, S[j]];
                            nextVertex = S[j];
                        }
                    }
                    _marks[nextVertex] = new Mark(max, i);
                    i = nextVertex;
                    if (i == _B)
                    {
                        int min = int.MaxValue;
                        for (int j = 0; j < _N; j++)
                        {
                            if (_marks[j] != null)
                            {
                                if (_marks[j].a < min)
                                {
                                    min = _marks[j].a;
                                }
                            }
                        }
                        _F += min;

                        var stack = new Stack<int>();
                        var k = _B;
                        while (k != -1)
                        {
                            stack.Push(k);
                            if (k == _A)
                                break;
                            k = _marks[k].vertexFrom;
                        }
                        var c = 0;
                        foreach (var x in stack)
                        {
                            if (c == 0)
                                Console.Write((x + 1).ToString());
                            else
                                Console.Write(" --> " + (x + 1).ToString());
                            c++;
                        }

                        for (int j = 0; j < _N; j++)
                        {
                            if (_marks[j] != null && j != _A)
                            {
                                _matrix[_marks[j].vertexFrom, j] -= min;
                                _matrix[j, _marks[j].vertexFrom] += min;
                            }
                        }

                        Console.Write("| f = " + min.ToString());
                        Console.WriteLine();

                        for (int j = 1; j < _N; j++)
                        {
                            _marks[j] = null;
                        }
                        deletedVertex = new List<int>();
                        i = _A;
                    }
                }
                else
                {
                    if (i != _A)
                    {
                        deletedVertex.Add(i);
                        i = _marks[i].vertexFrom;

                        _marks[deletedVertex[deletedVertex.Count - 1]] = null;
                    }
                    else
                    {
                        break;
                    }
                }
            } while (true);


            for (int j = 0; j < _N; j++)
            {
                for (int k = 0; k < _N; k++)
                {
                    if (fMatrix[j, k] != 0)
                    {
                        int a, b;
                        a = fMatrix[j, k] - _matrix[j, k];
                        b = fMatrix[k, j] - _matrix[k, j];
                        if (a > 0)
                        {
                            flowVertexes[j, k] = a;
                        }
                        else if (b > 0)
                        {
                            flowVertexes[j, k] = b;
                        }
                    }
                }
            }
            return new Flow(_F, flowVertexes);
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
