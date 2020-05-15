﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DinicsAlgorithm.Auxiliary
{
    public class Node
    {
        private int _level;
        private bool _closed;
        public int Level { get => _level; set => _level = value; }
        public bool Closed { get => _closed; set => _closed = value; }

        public Node()
        {
            _level = 999;
            Closed = false;
        }
    }
}