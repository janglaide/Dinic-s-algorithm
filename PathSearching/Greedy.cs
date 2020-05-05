using System;
using System.Collections.Generic;
using System.Text;

namespace PathSearching
{
    public class Greedy //used in Ford-Fulkerson algorithm
    {
        public (int cost, List<int> way) GreedyAlgorithm(int[,] cMatrix, int from, int to)
        {
            int vertexAmount = cMatrix.GetLength(0), i = from;
            int[] d = new int[vertexAmount];
            List<int>[] p = new List<int>[vertexAmount];
            for (int j = 0; j < vertexAmount; j++)
            {
                p[j] = new List<int>();
            }
            List<int> u = new List<int>();
            d[i] = 0;
            for (int j = 0; j < vertexAmount; j++)
            {
                if (j != i)
                {
                    d[j] = int.MinValue;
                }
            }
            while (u.Count < vertexAmount)
            {
                u.Add(i);
                int nextVertex = -1;
                List<int> S = new List<int>();
                for (int j = 0; j < vertexAmount; j++)
                {
                    if (cMatrix[i, j] > 0)
                    {
                        S.Add(j);
                    }
                }
                bool thereIsWay = false;
                if (S.Count > 0)
                {
                    foreach (int v in S)
                    {
                        if (d[v] < d[i] + cMatrix[i, v])
                        {
                            thereIsWay = true;
                            d[v] = d[i] + cMatrix[i, v];
                            p[v].Clear();
                            foreach (int vertex in p[i])
                            {
                                p[v].Add(vertex);
                            }
                            p[v].Add(i);
                            if (u.Contains(v))
                            {
                                u.Remove(v);
                            }
                        }
                    }
                    if (thereIsWay)
                    {
                        int max = int.MinValue;
                        foreach (int k in S)
                        {
                            if (d[k] > max && !u.Contains(k))
                            {
                                max = d[k];
                                nextVertex = k;
                            }
                        }
                    }
                }
                if (!thereIsWay)
                {
                    int max = int.MinValue;
                    for (int k = 0; k < vertexAmount; k++)
                    {
                        if (d[k] > max && !u.Contains(k))
                        {
                            max = d[k];
                            nextVertex = k;
                        }
                    }
                }
                i = nextVertex;
            }
            return (d[to], p[to]);
        }
    }
}
