using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FordFulkersonAlgorithm.Auxiliary
{
    public class IOStream
    {
        private int[,] _startMatrix;
        private int[,] _nullMatrix;
        public int[,] NullMatrix { get => _nullMatrix; }
        private int _N;
        public int[,] StartMatrix { get => _startMatrix; set => _startMatrix = value; }
        public (int[,], int[,]) FromFile(ref int from, ref int to)
        {
            string filename;
            int n;
            int[,] cMatrix;
            int[,] eMatrix;

            filename = "input.txt";//Console.ReadLine();
            Console.Write($"Enter filename: {filename}\n");
            if (File.Exists(filename))
            {
                try
                {
                    string[] lines, words;
                    lines = File.ReadAllLines(filename);

                    words = lines[0].Split(' ');
                    n = int.Parse(words[0]);

                    words = lines[1].Split(' ');
                    from = int.Parse(words[0]);
                    to = int.Parse(words[1]);

                    cMatrix = new int[n, n];
                    eMatrix = new int[n, n];
                    for (int i = 2; i <= n + 1; i++)
                    {
                        words = lines[i].Split(' ');
                        if (i <= n + 1)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                cMatrix[i - 2, j] = int.Parse(words[j]);
                                if (int.Parse(words[j]) != 0)
                                    eMatrix[i - 2, j] = 1;
                                else
                                    eMatrix[i - 2, j] = 0;
                            }
                        }
                    }
                    _startMatrix = CopyMatrix(cMatrix);
                    _N = n;
                    _nullMatrix = GetNullMatrix();
                    //Console.WriteLine($"Success");
                    return (cMatrix, eMatrix);
                }
                catch
                {
                    Console.WriteLine($"Invalid file content");
                }
            }
            else
            {
                Console.Write("There is no such file");
            }
            return (null, null);
        }
        private int[,] GetNullMatrix()
        {
            var m = new int[_N, _N];
            for (int i = 0; i < _N; i++)
            {
                for (int j = 0; j < _N; j++)
                    m[i, j] = 0;
            }
            return m;
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
        public void ToConsole(int[,] matrix)
        {
            if (matrix != null)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        Console.Write($"{matrix[i, j],-2}");
                    }
                    Console.WriteLine();
                }
            }
        }
        public void ConsoleOutput(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (_startMatrix[i, j] == 0)
                        Console.Write("(---) ");
                    else
                        Console.Write("(" + matrix[i, j].ToString() + "/" + _startMatrix[i, j].ToString() + ") ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
