using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Generics;

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

        public void Compress(string text, string currentName, string newName)
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
            final = metadata + final;
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
            var codes = new ColaPrioridad<Nodo<Character>>();
            foreach (var item in list)
                codes.Add(new Nodo<Character>(item.Value));
            while (codes.Count > 1)
            {
                var aux = new Nodo<Character>(new Character(codes.Get().Valor.Count)) { Derecha = codes.Get() };
                codes.Remove().Padre = aux;
                codes.Get().Padre = aux;
                aux.Izquierda = codes.Get();
                aux.Valor.Count += codes.Remove().Valor.Count;
                codes.Add(aux);
            }
            originalLenght = codes.Get().Valor.Count;
            AssignCode(codes.Get(), "");
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

        private void AssignCode(Nodo<Character> pos, string code)
        {
            pos.Valor.Code = code;
            if (pos.Izquierda != null)
                AssignCode(pos.Izquierda, code + "0");
            if (pos.Derecha != null)
                AssignCode(pos.Derecha, code + "1");
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
