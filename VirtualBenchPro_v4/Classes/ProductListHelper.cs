using System.Collections.Generic;
using System.Linq;

namespace VirtualBenchPro_v4.Classes
{
    public static class ProductListHelper
    {
        public static List<string> GetUpdateProductListNameAndSort(IReadOnlyCollection<string> productList, List<string> localParts)
        {
            var updatedList = new List<string>();

            if (productList != null && localParts != null)
            {
                foreach (var product in productList)
                {
                    if (localParts.Contains(product.Replace('_', '-')))
                    {
                        updatedList.Add(product.Replace('_', '-'));
                    }
                }
                updatedList.Sort();
            }
            return updatedList;
        }
    }
}
