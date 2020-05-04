using FordFulkersonAlgorithm.Auxiliary;
using System;
using System.Collections.Generic;
using System.Text;

namespace FordFulkersonAlgorithm
{
    public class Solver
    {
        private int _vertexAmount;
        private Mark[] _marks;
        public Flow FordFulkerson(int[,] cMatrix, int[,] eMatrix, int from, int to)
        {
            _vertexAmount = cMatrix.GetLength(0);
            int i, flowCost;
            int[,] flowVertexes = new int[_vertexAmount, _vertexAmount];
            List<int> deletedVertex = new List<int>();
            _marks = new Mark[_vertexAmount];

            flowCost = 0;

            int[,] fMatrix = CopyMatrix(cMatrix);

            i = 0;
            _marks[i] = new Mark(int.MaxValue, -1);
            do
            {
                List<int> S = new List<int>();
                for (int j = 0; j < _vertexAmount; j++)
                {
                    if (/*eMatrix[i, j] > 0 &&*/ !deletedVertex.Contains(j) && cMatrix[i, j] > 0 && _marks[j] == null)
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
                        if (cMatrix[i, S[j]] > max)
                        {
                            max = cMatrix[i, S[j]];
                            nextVertex = S[j];
                        }
                    }
                    _marks[nextVertex] = new Mark(max, i);
                    i = nextVertex;
                    if (i == to)
                    {
                        int min = int.MaxValue;
                        for (int j = 0; j < _vertexAmount; j++)
                        {
                            if (_marks[j] != null)
                            {
                                if (_marks[j].a < min)
                                {
                                    min = _marks[j].a;
                                }
                            }
                        }
                        flowCost += min;

                        var stack = new Stack<int>(); 
                        var k = to;
                        while(k != -1)
                        {
                            stack.Push(k);
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

                        for (int j = 0; j < _vertexAmount; j++)
                        {
                            if (_marks[j] != null && j != from)
                            {
                                cMatrix[_marks[j].vertexFrom, j] -= min;
                                cMatrix[j, _marks[j].vertexFrom] += min;
                            }
                        }

                        Console.Write("| f = " + min.ToString());
                        Console.WriteLine();

                        for (int j = 1; j < _vertexAmount; j++)
                        {
                            _marks[j] = null;
                        }
                        i = 0;
                    }
                }
                else
                {
                    if (i != from)
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


            for (int j = 0; j < _vertexAmount; j++)
            {
                for (int k = 0; k < _vertexAmount; k++)
                {
                    if (eMatrix[j, k] > 0)
                    {
                        int a, b;
                        a = fMatrix[j, k] - cMatrix[j, k];
                        b = fMatrix[k, j] - cMatrix[k, j];
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
            return new Flow(flowCost, flowVertexes);
        }
        public int[,] CopyMatrix(int[,] matrix)
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
