using System;
using System.Collections.Generic;
using System.Text;

namespace DinicsAlgorithm
{
    public class Dinics
    {
        private int _N;
        private int _F;
        private Matrix _matrix;
        public Matrix FlowMatrix { get => _matrix; }
        private List<Node> _nodes;
        public Dinics(string filename)
        {
            _matrix = new Matrix(filename);
            _N = _matrix.N;
            _nodes = FillNodes();
            _F = 0;
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
        public void Run(int start, int final)
        {
            while (true)
            {
                var flag = BFS(start - 1);

                if (!flag)
                    break;
                while (!_nodes[start - 1].Closed)
                {
                    var tmpPath = new List<int>();
                    var found = false;
                    DFS(ref tmpPath, 0, final - 1, start - 1, ref found);
                    if (tmpPath.Contains(final - 1))
                        PathOutput(tmpPath, start);
                }
                RerollNodes();
            }
            Console.WriteLine("\nResult: Max F = " + _F.ToString());
            _matrix.ConsoleOutput();
        }
        public void PathOutput(List<int> path, int start)
        {
            var current = start;
            Console.Write(current.ToString());
            foreach (var x in path)
                Console.Write(" --> " + (x + 1).ToString());
            Console.WriteLine();
        }
        public void DFS(ref List<int> path, int current, int final, int start, ref bool found)
        {
            if(current == final)
            {
                var f = GetMinF(path, start);
                _F += f;
                MatrixResolve(f, path, start);
                found = true;
                return;
            }
            var next = GetNodesLevels(current);
            if(next.Count != 0)
            {
                foreach(var x in next)
                {
                    path.Add(x);
                    DFS(ref path, x, final, start, ref found);
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
        public List<int> GetNodesLevels(int current)
        {
            var list = new List<int>();
            for (var i = 0; i < _N; i++)
            {
                if ((!_nodes[i].Closed) && _matrix[current, i].Flow != 999 && _matrix[current, i].Difference() > 0 &&
                    _nodes[i].Level > _nodes[current].Level)
                    list.Add(i);
            }
            return list;
        }
        public void MatrixResolve(int f, List<int> path, int start)
        {
            var from = start;
            foreach (var x in path)
            {
                _matrix[from, x].CurrentUsage += f;
                _matrix[x, from].CurrentUsage -= f;
                from = x;
            }
        }
        public int GetMinF(List<int> path, int start)
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
        public bool BFS(int start)
        {
            var current = start;
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
        public void FinalNodeResolver()
        {
            var max = 0;
            var list = new List<int>();
            for (var i = 0; i < _N; i++)
            {
                if (_matrix[i, _N - 1].Flow != 999)
                    list.Add(i);
            }
            foreach(var x in list)
            {
                if (_nodes[x].Level != 999 && _nodes[x].Level > max)
                    max = _nodes[x].Level;
            }
            _nodes[_N - 1].Level = max + 1;
        }
        public void SetLevel(int lvl, List<int> next)
        {
            foreach(var x in next)
            {
                if(_nodes[x].Level == 999)
                    _nodes[x].Level = lvl + 1;
            }
        }
        public int OpenedCount()
        {
            var counter = 0;
            foreach(var x in _nodes)
            {
                if (x.Level != 999)
                    counter++;
            }
            return counter;
        }
        public void QueueAdd(List<int> list, ref Queue<int> queue)
        {
            foreach (var x in list)
                queue.Enqueue(x);
        }
        public List<int> GetNodes(int current)
        {
            var list = new List<int>();
            for(var i = 0; i <_N; i++)
            {
                if (_matrix[current, i].Flow != 999 && _matrix[current, i].Difference() > 0 && _nodes[i].Level == 999)
                    list.Add(i);
            }
            return list;
        }
    }
}
