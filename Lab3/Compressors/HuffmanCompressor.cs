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
        private int originalLenght;

        public HuffmanCompressor(string path)
        {
            Path = path;
        }

        public string ShowCompress(string text)
        {
            var codes = GenerateCode(text);
            string huffman = "";
            for (int i = 0; i < text.Length; i++)
                huffman += codes[text[i]].Code;
            while (huffman.Length % 8 != 0)
                huffman += "0";
            string final = "";
            while (huffman.Length > 0)
            {
                final += ConvertToChar(huffman.Substring(0, 8));
                huffman = huffman.Remove(0, 8);
            }
            string metadata = Convert.ToString(Convert.ToChar(codes.Count));
            int max = 0;
            foreach (var item in codes)
            {
                if (item.Value.Count > max)
                    max = item.Value.Count;
            }
            int space = 0;
            while (max > 0)
            {
                space++;
                max /= 256;
            }
            metadata += Convert.ToChar(space);
            foreach (var item in codes)
            {
                metadata += item.Value.Letter;
                string count = "";
                for (int i = 0; i < space; i++)
                {
                    count = Convert.ToChar(item.Value.Count % 256) + count;
                    item.Value.Count /= 256;
                }
                metadata += count;
            }
            return metadata + final;
        }

        public string Compress(string text, string currentName, string newName)
        {
            string huffman = ShowCompress(text);
            string path = Path + newName + ".huff";
            var file = new FileStream(path, FileMode.OpenOrCreate);
            using StreamWriter writer = new StreamWriter(file, Encoding.ASCII);
            writer.Write(huffman);
            file.Close();
            writer.Close();
            var comp = new Compression { OriginalName = currentName, CompressedName = newName + ".huff", CompressedRoute = path };
            comp.CompressionRatio = Math.Round(Convert.ToDouble(huffman.Length) / Convert.ToDouble(text), 4);
            comp.CompressionFactor = Math.Round(Convert.ToDouble(text) / Convert.ToDouble(huffman.Length), 4);
            comp.ReductionPercentage = Math.Round(100 * (Convert.ToDouble(text) - Convert.ToDouble(huffman)) / Convert.ToDouble(text), 4);
            file = new FileStream(path + "//Compressions", FileMode.OpenOrCreate);
            using StreamWriter writer1 = new StreamWriter(file, Encoding.ASCII);
            writer1.Write(comp.ToFixedString());
            file.Close();
            writer1.Close();
            return path;
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
                codes.RemoveAt(0);
                codes.Sort();
            }
            originalLenght = codes[0].Value.Count;
            AssignCode(codes[0], "");
            return list;
        }

        private char ConvertToChar(string binary)
        {
            int value = 0;
            while (binary.Length > 0)
            {
                value *= 2;
                value += int.Parse(binary.Substring(0, 1));
                binary = binary.Remove(0, 1);
            }
            return Convert.ToChar(value);
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
