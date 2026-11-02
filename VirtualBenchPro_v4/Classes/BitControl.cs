using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class BitControl
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int BitLength { get; set; }
        public string Unit { get; set; }
        public double Lsb { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public Dictionary<string, int> PossibleValues { get; set; }

        //Type PossibleValuesType
        public BitControl(string id, string type, string name, int position, int bitLength, Dictionary<string, int> possibleValues)
        {
            ID = id;
            Type = type;
            Name = name;
            Position = position;
            BitLength = bitLength;
            PossibleValues = possibleValues;
        }

        //Type MinMaxType
        public BitControl(string id, string type, string name, int position, int bitLength, string unit, double lsb, double minValue, double maxValue)
        {
            ID = id;
            Type = type;
            Name = name;
            Position = position;
            BitLength = bitLength;
            Unit = unit;
            Lsb = lsb;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
