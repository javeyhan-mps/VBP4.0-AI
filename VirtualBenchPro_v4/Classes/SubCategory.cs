using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class SubCategory
    {
        public string Name { get; set; }
        public string InfoSubCategory { get; set; }
        public List<ParameterGroup> ParametersGroups { get; set; }

        public SubCategory(string name, string infoSubCategory, List<ParameterGroup> parametersGroups)
        {
            Name = name;
            InfoSubCategory = infoSubCategory;
            ParametersGroups = parametersGroups;
        }
    }
}
