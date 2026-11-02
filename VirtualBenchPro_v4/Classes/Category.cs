using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4
{
    public class Category
    {
        public string Name { get; set; }
        public string InfoCategory { get; set; }
        public List<SubCategory> SubCategories { get; set; }

        public Category(string name, string infoCategory, List<SubCategory> subCategories)
        {
            Name = name;
            InfoCategory = infoCategory;
            SubCategories = subCategories;
        }
    }
}
