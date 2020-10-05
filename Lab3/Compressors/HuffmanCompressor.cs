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

        public string ShowCompress(string text)
        {
            var codes = new Dictionary<char, Character>();
            for (int i = 0; i < text.Length; i++)
            {
                if (codes.ContainsKey(text[i]))
                    codes[text[i]].Count++;
                else
                    codes.Add(text[i], new Character(text[i]));
            }
            GenerateCode(codes);
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
            using StreamWriter writer = new StreamWriter(path, false);
            writer.Write(huffman);
            writer.Close();
            var comp = new Compression { OriginalName = currentName, CompressedFileName = newName + ".huff", CompressedFilePath = path };
            comp.CompressionRatio = Math.Round(Convert.ToDouble(huffman.Length) / Convert.ToDouble(text), 4);
            comp.CompressionFactor = Math.Round(Convert.ToDouble(text) / Convert.ToDouble(huffman.Length), 4);
            comp.ReductionPercentage = Math.Round(100 * (Convert.ToDouble(text) - Convert.ToDouble(huffman)) / Convert.ToDouble(text), 4);
            var file = new FileStream(path + "//Compressions", FileMode.OpenOrCreate);
            using StreamWriter writer1 = new StreamWriter(file, Encoding.ASCII);
            writer1.Write(comp.ToFixedString());
            file.Close();
            writer1.Close();
            return path;
        }

        private void GenerateCode(Dictionary<char, Character> list)
        {
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
            AssignCode(codes[0], "");
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

        private string ConvertToBinary(char value)
        {
            int binary = Convert.ToInt32(value);
            string aux = "";
            while (binary > 0)
            {
                aux = binary % 2 + aux;
                binary /= 2;
            }
            return aux;
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
            var letters = Convert.ToInt32(text[0]);
            var length = Convert.ToInt32(text[1]);
            text = text.Remove(0, 2);
            var codes = new Dictionary<char, Character>();
            for (int i = 0; i < letters; i++)
            {
                var aux = new Character(text[0]);
                aux.Count = 0;
                for (int j = 1; j <= length; j++)
                {
                    aux.Count *= 256;
                    aux.Count += Convert.ToInt32(text[j]);
                }
                codes.Add(aux.Letter, aux);
                text = text.Remove(0, length + 1);
            }
            GenerateCode(codes);
            var coding = new Dictionary<string, char>();
            length = 0;
            foreach (var item in codes)
            {
                coding.Add(item.Value.Code, item.Key);
                length += item.Value.Count;
            }
            string binary = "";
            for (int i = 0; i < text.Length; i++)
                binary += ConvertToBinary(text[i]);
            string final = "";
            for (int i = 0; i < length; i++)
            {
                int j = 0;
                while (!coding.ContainsKey(binary.Substring(0, j)))
                    j++;
                final += coding[binary.Substring(0, j)];
                binary = binary.Remove(0, j);
            }
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
