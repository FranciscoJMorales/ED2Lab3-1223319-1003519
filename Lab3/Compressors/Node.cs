using System;
using System.Collections.Generic;
using System.Text;

namespace Compressors
{
    class Node : IComparable
    {
        public Node Father { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Character Value { get; set; }

        public Node(Character val)
        {
            Value = val;
        }

        public int CompareTo(object obj)
        {
            return Value.Count.CompareTo(((Node)obj).Value.Count);
        }
    }
}
