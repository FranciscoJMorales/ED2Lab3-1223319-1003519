using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compressors
{
    public class HuffmanCompressor : ICompressor
    {
        private readonly string Path;

        public HuffmanCompressor(string path)
        {
            Path = path;
        }

        public void Compress(string text, string currentName, string newName)
        {
            var codes = GenerateCode(text);
        }

        private Dictionary<char, Character> GenerateCode(string text)
        {
            var list = new Dictionary<char, Character>();
            for (int i = 0; i < text.Length; i++)
            {
                if (list.ContainsKey(text[i]))
                    list[text[i]].Count++;
                else
                    list.Add(text[i], new Character(text[i]));
            }
            var codes = new List<Node>();
            foreach (var item in list)
                codes.Add(new Node(item.Value));
            codes.Sort();
            while (codes.Count > 1)
            {
                codes[0].Father = new Node(new Character(codes[0].Value.Count + codes[1].Value.Count)) { Left = codes[1], Right = codes[0] };
                codes[1].Father = codes[0].Father;
                codes.Add(codes[0].Father);
                codes.RemoveAt(0);
                codes.RemoveAt(1);
                codes.Sort();
            }
            AssignCode(codes[0], "");
            return list;
        }

        private void AssignCode(Node pos, string code)
        {
            pos.Value.Code = code;
            if (pos.Left != null)
                AssignCode(pos.Left, code + "0");
            if (pos.Right != null)
                AssignCode(pos.Right, code + "1");
        }

        public void Decompress(string text, string currentName)
        {
            throw new NotImplementedException();
        }

        public List<Compression> GetCompressions()
        {
            var file = new FileStream(Path + "//Compressions", FileMode.OpenOrCreate);
            using StreamReader reader = new StreamReader(file, Encoding.ASCII);
            List<Compression> compressions = new List<Compression>();
            Compression aux = new Compression();
            while (!reader.EndOfStream)
                compressions.Add((Compression)aux.CreateFromFixedText(reader.ReadLine()));
            if (compressions.Count > 0)
                return compressions;
            else
                return null;
        }
    }
}
