using System;
using System.Collections.Generic;
using System.Text;

namespace Compressors
{
    class Character : IComparable
    {
        public char Letter { get; set; }
        public int Count { get; set; }
        public string Code { get; set; }

        public Character(char value)
        {
            Letter = value;
            Count = 1;
        }

        public Character(int weight)
        {
            Count = weight;
        }

        public int CompareTo(object obj)
        {
            return this.Count.CompareTo(((Character)obj).Count);
        }
    }
}
