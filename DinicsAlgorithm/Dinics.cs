using DinicsAlgorithm.Auxiliary;
using System;
using System.Collections.Generic;
using System.Text;


namespace DinicsAlgorithm
{
    public class Dinics
    {
        private int _N;
        private int _F;
        public int F { get => _F; }
        private int _A;
        private int _B;
        private Matrix _matrix;
        public Matrix FlowMatrix { get => _matrix; }
        private List<Node> _nodes;
        public Dinics(int A, int B, int N, int[,] matrix)
        {
            _matrix = new Matrix(matrix, N);
            _N = N;
            _nodes = FillNodes();
            _F = 0;
            _A = A;
            _B = B;
        }
        private List<Node> FillNodes()
        {
            var nodes = new List<Node>();
            for(var i = 0; i < _N; i++)
                nodes.Add(new Node());
            return nodes;
        }
        private void RerollNodes()
        {
            foreach(var x in _nodes)
            {
                x.Level = 999;
                x.Closed = false;
            }
        }

        public void Run()
        {
            while (true)
            {
                var flag = BFS();

                if (!flag)
                    break;
                while (!_nodes[_A - 1].Closed)
                {
                    var tmpPath = new List<int>();
                    var found = false;
                    var f = 0;
                    DFS(ref tmpPath, 0, ref found, ref f);
                    if (tmpPath.Contains(_B - 1))
                        PathOutput(tmpPath, _A, f);
                }
                RerollNodes();
            }
        }
        private void PathOutput(List<int> path, int start, int f)
        {
            var current = start;
            Console.Write(current.ToString());
            foreach (var x in path)
                Console.Write(" --> " + (x + 1).ToString());
            Console.Write("| f = " + f.ToString());
            Console.WriteLine();
        }
        private void DFS(ref List<int> path, int current, ref bool found, ref int f)
        {
            if(current == _B - 1)
            {
                f = GetMinF(path, _A - 1);
                _F += f;
                MatrixResolve(f, path, _A - 1);
                found = true;
                return;
            }
            var next = GetNodesLevels(current);
            if(next.Count != 0)
            {
                foreach(var x in next)
                {
                    path.Add(x);
                    DFS(ref path, x, ref found, ref f);
                    if (found)
                        break;
                    
                }
                if (!found)
                    path.Remove(current);
            }
            else
            {
                path.Remove(current);
                _nodes[current].Closed = true;
            }
            
        }
        private List<int> GetNodesLevels(int current)
        {
            var list = new List<int>();
            for (var i = 0; i < _N; i++)
            {
                if ((!_nodes[i].Closed) && _matrix[current, i].Flow != 0 && _matrix[current, i].Difference() > 0 &&
                    _nodes[i].Level > _nodes[current].Level)
                    list.Add(i);
            }
            return list;
        }
        private void MatrixResolve(int f, List<int> path, int start)
        {
            var from = start;
            foreach (var x in path)
            {
                _matrix[from, x].CurrentUsage += f;
                _matrix[x, from].CurrentUsage -= f;
                from = x;
            }
        }
        private int GetMinF(List<int> path, int start)
        {
            var min = 999;
            var from = start;
            foreach(var x in path)
            {
                if (_matrix[from, x].Difference() < min)
                    min = _matrix[from, x].Difference();
                from = x;
            }
            return min;
        }
        private bool BFS()
        {
            var current = _A - 1;
            var queue = new Queue<int>();
            queue.Enqueue(current);
            _nodes[current].Level = 0;
            while (queue.Count != 0 && OpenedCount() != _N)
            {
                current = queue.Dequeue();
                var next = GetNodes(current);
                QueueAdd(next, ref queue);
                SetLevel(_nodes[current].Level, next);
            }
            if (OpenedCount() == _N || _nodes[_N - 1].Level != 999)
            {
                FinalNodeResolver();
                return true;
            }
            else
                return false;
        }
        private void FinalNodeResolver()
        {
            var max = 0;
            var list = new List<int>();
            for (var i = 0; i < _N; i++)
            {
                if (_matrix[i, _N - 1].Flow != 0)
                    list.Add(i);
            }
            foreach(var x in list)
            {
                if (_nodes[x].Level != 999 && _nodes[x].Level > max)
                    max = _nodes[x].Level;
            }
            _nodes[_N - 1].Level = max + 1;
        }
        private void SetLevel(int lvl, List<int> next)
        {
            foreach(var x in next)
            {
                if(_nodes[x].Level == 999)
                    _nodes[x].Level = lvl + 1;
            }
        }
        private int OpenedCount()
        {
            var counter = 0;
            foreach(var x in _nodes)
            {
                if (x.Level != 999)
                    counter++;
            }
            return counter;
        }
        private void QueueAdd(List<int> list, ref Queue<int> queue)
        {
            foreach (var x in list)
                queue.Enqueue(x);
        }
        private List<int> GetNodes(int current)
        {
            var list = new List<int>();
            for(var i = 0; i <_N; i++)
            {
                if (_matrix[current, i].Flow != 0 && _matrix[current, i].Difference() > 0 && _nodes[i].Level == 999)
                    list.Add(i);
            }
            return list;
        }
    }
}
