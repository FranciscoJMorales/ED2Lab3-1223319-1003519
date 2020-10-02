using System;
using System.Collections.Generic;
using System.Text;

namespace Compressors
{
    public class Compression : IFixedSizeText
    {
        public string OriginalName { get; set; }
        public string CompressedName { get; set; }
        public string CompressedRoute { get; set; }
        public double CompressionRatio { get; set; }
        public double CompressionFactor { get; set; }
        public double ReductionPercentage { get; set; }

        public int TextLength => ToFixedString().Length;

        public IFixedSizeText CreateFromFixedText(string text)
        {
            if (text.Trim() != "")
            {
                Compression item = new Compression();
                item.OriginalName = text.Substring(0, 50).Trim();
                text = text.Remove(0, 51);
                if (item.OriginalName == "")
                    item.OriginalName = null;
                item.CompressedName = text.Substring(0, 50).Trim();
                text = text.Remove(0, 51);
                if (item.CompressedName == "")
                    item.CompressedName = null;
                item.CompressedRoute = text.Substring(0, 100).Trim();
                text = text.Remove(0, 101);
                if (item.CompressedRoute == "")
                    item.CompressedRoute = null;
                item.CompressionRatio = int.Parse(text.Substring(0, 9));
                text = text.Remove(0, 10);
                item.CompressionFactor = int.Parse(text.Substring(0, 9));
                text = text.Remove(0, 10);
                item.ReductionPercentage = int.Parse(text.Substring(0, 9));
                return item;
            }
            else
                return null;
        }

        public string ToFixedString()
        {
            string text = "";
            if (OriginalName != null)
                text += string.Format("{0, -50}", OriginalName) + ",";
            else
                text += new string(' ', 50) + ",";
            if (CompressedName != null)
                text += string.Format("{0, -50}", CompressedName) + ",";
            else
                text += new string(' ', 50) + ",";
            if (CompressedRoute != null)
                text += string.Format("{0, -100}", CompressedRoute) + ",";
            else
                text += new string(' ', 11) + ",";
            text += CompressionRatio.ToString("0000.0000") + ",";
            text += CompressionFactor.ToString("0000.0000") + ",";
            text += ReductionPercentage.ToString("0000.0000");
            return text;
        }
    }
}
