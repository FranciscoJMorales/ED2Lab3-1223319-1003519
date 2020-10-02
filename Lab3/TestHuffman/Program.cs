using Compressors;
using System;

namespace TestHuffman
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese el texto para codificar");
            string text = Console.ReadLine();
            var huffman = new HuffmanCompressor("..//..//..");
            huffman.Compress(text, "a", "b");
            Console.ReadLine();
        }
    }
}
