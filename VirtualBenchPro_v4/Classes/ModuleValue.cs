using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class ModuleValue
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        public ModuleValue(double v, string u)
        {
            Value = v;
            Unit = u;
        }
    }

    public class RegInfo
    {
        public string page;
        public byte regaddr;
        public uint regval;
    }
}
