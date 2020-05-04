using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DinicsAlgorithm
{
    public class Matrix
    {
        private Edge[,] _edges;
        private int _N;
        public int N => _N;
        public Edge this[int i, int j]
        {
            get
            {
                return _edges[i, j];
            }
            set
            {
                _edges[i, j] = value;
            }
        }
        public Matrix(string filename) 
        {
            StreamReader streamReader = new StreamReader(filename, Encoding.UTF8);
            _N = Convert.ToInt32(streamReader.ReadLine());
            var buffer = streamReader.ReadToEnd();
            _edges = new Edge[_N, _N];
            Fill();
            var i = 0;
            foreach(var row in buffer.Split('\n'))
            {
                var j = 0;
                foreach(var col in row.Trim().Split(' '))
                {
                    if(Convert.ToInt32(col.Trim()) != 999)
                    {
                        _edges[i, j].Flow = Convert.ToInt32(col.Trim());
                        if(j != _N - 1)
                        {
                            _edges[j, i].Flow = Convert.ToInt32(col.Trim());
                            _edges[j, i].CurrentUsage = Convert.ToInt32(col.Trim());
                        }
                    }
                    j++;
                }
                i++;
            }
            streamReader.Close();
            Console.WriteLine(filename + " was read");
        }
        public void Fill()
        {
            for (var i = 0; i < _N; i++)
                for (var j = 0; j < _N; j++)
                    _edges[i, j] = new Edge(999);
        }
        public void ConsoleOutput()
        {
            for (var i = 0; i < _N; i++)
            {
                for (var j = 0; j < _N; j++)
                {
                    if(this[i, j].Flow == 999)
                        Console.Write("(---) ");
                    else
                        Console.Write("(" + this[i, j].CurrentUsage.ToString() + "/" + this[i, j].Flow.ToString() + ") ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
