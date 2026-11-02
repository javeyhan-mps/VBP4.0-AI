using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VirtualBenchPro_v4.Classes
{
	public class Help
	{
		public string Help_Name { get; set; }
		public Image Help_Image { get; set; }


		public Help(string name)
		{
			Help_Name = name;
		}

		public Help(string name, Image image)
		{
			Help_Name = name;
			Help_Image = image;
		}

	}
}
