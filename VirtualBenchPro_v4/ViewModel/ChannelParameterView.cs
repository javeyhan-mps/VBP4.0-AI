using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Common.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VirtualBenchPro_v4.ViewModel
{
    public class ChannelParameterView : ObservableObject
    {

        public ChannelParameterView(ProjectConfig _proConfig)  //string pn
        {
            ProConfig = _proConfig;

            //Pages = ProConfig.Chips.FirstOrDefault().Pages;   
            Pages = ProConfig.Chips.FirstOrDefault().Regions.FirstOrDefault().ConfigurationModes.FirstOrDefault().Pages;

            //BasicSetting = new ObservableCollection<MPS.Common.Model.ParameterGroup>(Pages.Where(o => o.PageName == "CH0").FirstOrDefault().Categories.Where(o => o.Name == "BasicSetting").FirstOrDefault().Subcategories.Where(o=>o.Name=="Basic").FirstOrDefault().ParameterGroups);
            //AdvancedSetting = new ObservableCollection<MPS.Common.Model.Category>(Pages.Where(o => o.PageName == "CH0").FirstOrDefault().Categories);

            BasicSetting = Pages.FirstOrDefault(o => o.PageName == "CH0").Configurations.FirstOrDefault(o => o.Name == "Basic Configuration").Categories.FirstOrDefault().Subcategories.FirstOrDefault().ParameterGroups;
            AdvancedSetting = Pages.FirstOrDefault(o => o.PageName == "CH0").Configurations.FirstOrDefault(o => o.Name == "Advanced Configuration").Categories;

            if (!Pages.Any(o => o.IsSelected))
            {

                Pages.FirstOrDefault().IsSelected = true;

            }
            RadioPageSelect(Pages);

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

        private ObservableCollection<Page> _pages;

        public ObservableCollection<Page> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                RaisePropertyChanged("Pages");
            }
        }

        private ObservableCollection<MPS.Common.Model.ParameterGroup> _basicSetting;

        public ObservableCollection<MPS.Common.Model.ParameterGroup> BasicSetting
        {
            get { return _basicSetting; }
            set
            {
                _basicSetting = value;
                RaisePropertyChanged("BasicSetting");
            }
        }

        private ObservableCollection<MPS.Common.Model.Category> _advancedSetting;

        public ObservableCollection<MPS.Common.Model.Category> AdvancedSetting
        {
            get { return _advancedSetting; }
            set
            {
                _advancedSetting = value;
                RaisePropertyChanged("AdvancedSetting");
            }
        }

        private int _CategorySelectedId = -1;

        public int CategorySelectedId
        {
            get { return _CategorySelectedId; }
            set
            {
                _CategorySelectedId = value;
                RaisePropertyChanged("CategorySelectedId");
            }
        }


        public ICommand RadioPageSelectCommand => new RelayCommand<object>(RadioPageSelect);
        private void RadioPageSelect(object treeim)
        {
            try
            {
                var pages = treeim as ObservableCollection<Page>;
                var selectedPage = pages.Where(p => p.IsSelected == true).First();
                BasicSetting = selectedPage.Configurations.FirstOrDefault(o => o.Name == "Basic Configuration").Categories.FirstOrDefault().Subcategories.FirstOrDefault().ParameterGroups;
                AdvancedSetting = selectedPage.Configurations.FirstOrDefault(o => o.Name == "Advanced Configuration").Categories;
                CategorySelectedId = (CategorySelectedId == -1 || CategorySelectedId > AdvancedSetting.Count - 1) ? 0 : CategorySelectedId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }


}
