using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class Chip
    {
        public string ID { get; set; }
        public string Module_Name { get; set; }
        public int Address { get; set; }
        public List<Register> Registers { get; set; }

        public Chip(string id, string moduleName, int address, List<Register> registers)
        {
            ID = id;
            Module_Name = moduleName;
            Address = address;
            Registers = registers;
        }
    }
}
