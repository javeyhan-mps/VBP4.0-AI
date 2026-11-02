using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VirtualBenchPro_v4.ViewModel
{
    public class WinAboutView
    {
        public string AppVersion
        {
            get
            {
                return string.Format("Version:{0}", Assembly.GetExecutingAssembly().GetName().Version);
            }
        }

        public string AppCopyright
        {
            get
            {
                return string.Format("Copyright {0}", ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright);
            }
        }
    }
}
