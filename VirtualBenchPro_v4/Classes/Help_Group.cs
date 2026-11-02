using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4.Classes
{
	public class Help_Group
	{
		public string Help_Group_Title { get; set; }
		public List<Help> Help_List { get; set; }

		public Help_Group(string subGroupTitle, List<Help> help_list)
		{
			Help_Group_Title = subGroupTitle;
			Help_List = help_list;
		}
	}
}
