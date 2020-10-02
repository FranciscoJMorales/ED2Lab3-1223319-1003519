using System;
using System.Collections.Generic;
using System.Text;

namespace Compressors
{
    public interface ICompressor
    {
        public abstract void Compress(string text, string currentName, string newName);
        public abstract void Decompress(string text, string currentName);
        public abstract List<Compression> GetCompressions();
    }
}
