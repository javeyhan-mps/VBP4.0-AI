using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class Register
    {

        public string Name { get; set; }
        public int Address { get; set; }
        public string DataType { get; set; }
        public string Right { get; set; }
        public List<BitControl> BitControls { get; set; }
        public Dictionary<string, int> Pages { get; set; }

        public Register(string name, int address, string dataType, string right, List<BitControl> bitControls, Dictionary<string, int> pages)
        {
            Name = name;
            Address = address;
            DataType = dataType;
            Right = right;
            BitControls = bitControls;
            Pages = pages;
        }
    }
}
