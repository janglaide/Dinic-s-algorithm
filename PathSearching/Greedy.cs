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
            while (i != to)
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
                    for (int j = 0; j < S.Count; j++)
                    {
                        if (d[S[j]] < d[i] + cMatrix[i, S[j]])
                        {
                            thereIsWay = true;
                            d[S[j]] = d[i] + cMatrix[i, S[j]];
                            p[S[j]].Clear();
                            foreach (int vertex in p[i])
                            {
                                p[S[j]].Add(vertex);
                            }
                            p[S[j]].Add(i);
                            if (u.Contains(S[j]))
                            {
                                u.Remove(S[j]);
                            }
                        }
                    }
                    if (thereIsWay)
                    {
                        int max = int.MinValue;
                        for (int j = 0; j < S.Count; j++)
                        {
                            if (d[S[j]] > max && !u.Contains(S[j]))
                            {
                                max = d[S[j]];
                                nextVertex = S[j];
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
                if (nextVertex == -1)
                {
                    d[to] = 0;
                    p[to] = null;
                    break;
                }
                i = nextVertex;
            }
            p[to].Add(to);
            return (d[to], p[to]);
        }
    }
}
