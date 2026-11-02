using Microsoft.Win32;
using MPS.Common;
using MPS.Common.Command;
using MPS.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualBench_PC_MTP;

namespace VirtualBenchPro_v4.ViewModel
{
    class ProjectToolsView : ObservableObject
    {
        public ProjectToolsView(ProjectConfig pobj)
        {
            this.ProConfig = pobj;
        }

        private ProjectConfig _projectConfig;
        public ProjectConfig ProConfig
        {
            get { return _projectConfig; }
            set
            {
                _projectConfig = value;

                RaisePropertyChanged("ProConfig");
            }
        }
    }
}
