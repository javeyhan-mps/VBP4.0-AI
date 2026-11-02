using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4.Classes
{
    public class JsonObject
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public int ProductsFound { get; set; }
        public ObservableCollection<string> ProdcutList { get; set; }
    }
}
