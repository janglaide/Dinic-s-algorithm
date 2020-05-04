using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    public class IOConsole
    {
        private string _filename;
        private int _N;
        private int _A;
        private int _B;
        private int[,] _matrix;
        public int N { get => _N; }
        public int From { get => _A; }
        public int To { get => _B; }
        public int[,] CMatrix { get => _matrix; }
        public IOConsole(string filename)
        {
            _filename = filename;
        }
        public void ReadMatrix()
        {
            StreamReader streamReader = new StreamReader(_filename, Encoding.UTF8);
            _N = Convert.ToInt32(streamReader.ReadLine());
            var destinationLine = streamReader.ReadLine();
            var _from_to = new List<int>();
            foreach (var x in destinationLine.Trim().Split(' '))
                _from_to.Add(Convert.ToInt32(x));
            _A = _from_to[0];
            _B = _from_to[1];

            var buffer = streamReader.ReadToEnd();
            _matrix = new int[_N, _N];
            Fill();
            var i = 0;
            foreach (var row in buffer.Split('\n'))
            {
                var j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    _matrix[i, j] = Convert.ToInt32(col.Trim());
                    j++;
                }
                i++;
            }
            streamReader.Close();
        }
        public void WriteMatrix(int[,] A, int[,] B)
        {
            for (var i = 0; i < _N; i++)
            {
                for (var j = 0; j < _N; j++)
                {
                    if (A[i, j] == 0)
                        Console.Write("(---) ");
                    else
                        Console.Write("(" + B[i, j].ToString() + "/" + A[i, j].ToString() + ") ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void Fill()
        {
            for (var i = 0; i < _N; i++)
                for (var j = 0; j < _N; j++)
                    _matrix[i, j] = 0;
        }
        public int[,] GetNullMatrix()
        {
            var m = new int[_N, _N];
            for (int i = 0; i < _N; i++)
            {
                for (int j = 0; j < _N; j++)
                    m[i, j] = 0;
            }
            return m;
        }
    }
}
