using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using MPS.Common;
using MPS.Common.Command;
using MPS.Common.Model;
using MPS.Common.Sqlite;
using MPS.Communication;
using MPS.Instrument;
using MPS.Monitor;
using MPS.VBP.PartFactory;
using MPS.VBP.PartFactory.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VirtualBench_PC_MTP;
using VirtualBenchPro_v4.Classes;
using VirtualBenchPro_v4.Views.CustomElements;
//using VirtualBenchPro_v4.Views.MonitorViews;
using VirtualBenchPro_v4.Views.DeviceViews;

namespace VirtualBenchPro_v4.ViewModel
{
    public class DeviceView : ObservableObject
    {
        readonly object _lockObj = new object();
        private MainWindow main_window;
        
        public DeviceView(ProjectConfig obj, MainWindow mw)
        {
            ProConfig = obj;
            UserLicense = mw.LicenseInfo;
            _logDataDic = new Dictionary<string, ObservableCollection<LogModel>>();
            main_window = mw;
          
        }

        #region Properties
        public AbstractCommunication _communicationObj;

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

        //public LicenseInfo UserLicense;

        private LicenseInfo _userLicense;

        public LicenseInfo UserLicense
        {
            get { return _userLicense; }
            set { _userLicense = value;
                RaisePropertyChanged("UserLicense");
            }
        }



        private MPS.Common.Model.Chip _currentChip;
        public MPS.Common.Model.Chip CurrentChip
        {
            get { return _currentChip; }
            set
            {
                _currentChip = value;
                RaisePropertyChanged("CurrentChip");
            }
        }

        private ConfigurationMode _currentConfigMode;

        public ConfigurationMode CurrentConfigMode
        {
            get { return _currentConfigMode; }
            set
            {
                _currentConfigMode = value;
                RaisePropertyChanged("CurrentConfigMode");
            }
        }

        private MPS.Common.Model.Page _currentPage;

        public MPS.Common.Model.Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged("currentPage");
            }
        }

        private ObservableCollection<RegisterRegion> _registerRegionforBk = new ObservableCollection<RegisterRegion>();
        public ObservableCollection<RegisterRegion> RegisterRegionforBk
        {
            get { return _registerRegionforBk; }
            set
            {
                _registerRegionforBk = value;
                RaisePropertyChanged("RegisterRegionforBk");
            }
        }


        private ObservableCollection<MPS.Common.Model.Register> _currentRegisterMap;
        public ObservableCollection<MPS.Common.Model.Register> CurrentRegisterMap
        {
            get { return _currentRegisterMap; }
            set
            {
                _currentRegisterMap = value;
                RaisePropertyChanged("CurrentRegisterMap");
            }
        }        
        
        private ObservableCollection<MPS.Common.Model.Register> _blackboxviewRegisterMap = new ObservableCollection<Register>();
        public ObservableCollection<MPS.Common.Model.Register> BlackBoxViewRegisterMap
        {
            get { return _blackboxviewRegisterMap; }
            set
            {
                _blackboxviewRegisterMap = value;
                RaisePropertyChanged("BlackBoxViewRegisterMap");
            }
        }


        private ObservableCollection<Register> _BlackBoxRegisterMap = new ObservableCollection<Register>();

        public ObservableCollection<Register> BlackBoxRegisterMap
        {
            get { return _BlackBoxRegisterMap; }
            set 
            { 
                _BlackBoxRegisterMap = value;
                RaisePropertyChanged("BlackBoxRegisterMap");
            }
        }


        private string _BlackBoxFilterText;

        public string BlackBoxFilterText
        {
            get { return _BlackBoxFilterText; }
            set 
            { 
                _BlackBoxFilterText = value;
                BlackBoxViewRegisterMap.Clear();
                foreach (var reg in BlackBoxRegisterMap)
                {
                    if (reg.Name.ToLower().Contains(_BlackBoxFilterText.ToLower()))
                    {
                        BlackBoxViewRegisterMap.Add(reg);
                    }
                }
            }
        }



        private ObservableCollection<LogModel> _logdata;
        public ObservableCollection<LogModel> LogData
        {
            get { return _logdata; }
            set
            {
                _logdata = value;
                RaisePropertyChanged("LogData");
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

        private int _SelectedRegsiterIndex = -1;

        public int SelectedRegsiterIndex
        {
            get { return _SelectedRegsiterIndex; }
            set
            {
                _SelectedRegsiterIndex = value;
                RaisePropertyChanged("SelectedRegsiterIndex");
            }
        }

        private int _SubcategorySelectedId;

        public int SubcategorySelectedId
        {
            get { return _SubcategorySelectedId; }
            set
            {
                _SubcategorySelectedId = value;
                RaisePropertyChanged("SubcategorySelectedId");
            }
        }

        private System.Windows.Media.Imaging.BitmapImage _overview;

        public System.Windows.Media.Imaging.BitmapImage Overview
        {
            get { return _overview; }
            set
            {
                _overview = value;
                RaisePropertyChanged("Overview");
            }
        }


        private string _descriptionTitle;

        public string DescriptionTitle
        {
            get { return _descriptionTitle; }
            set
            {
                _descriptionTitle = value;
                RaisePropertyChanged("DescriptionTitle");
            }
        }

        private string _decription;

        public string Description
        {
            get { return _decription; }
            set
            {
                _decription = value;
                RaisePropertyChanged("Description");
            }
        }

        private string _featuresTitle;

        public string FeaturesTitle
        {
            get { return _featuresTitle; }
            set
            {
                _featuresTitle = value;
                RaisePropertyChanged("FeaturesTitle");
            }
        }

        private string _features;

        public string Features
        {
            get { return _features; }
            set
            {
                _features = value;
                RaisePropertyChanged("Features");
            }
        }

        private string _diagramTitle;

        public string DiagramTitle
        {
            get { return _diagramTitle; }
            set
            {
                _diagramTitle = value;
                RaisePropertyChanged("DiagramTitle");
            }
        }

        private System.Windows.Media.Imaging.BitmapImage _diagram;

        public System.Windows.Media.Imaging.BitmapImage Diagram
        {
            get { return _diagram; }
            set
            {
                _diagram = value;
                RaisePropertyChanged("Diagram");
            }
        }

        private AbstractPart _currentPart;
        public AbstractPart CurrentPart
        {
            get
            {
                return this._currentPart;
            }
            set
            {
                this._currentPart = value;
                RaisePropertyChanged("CurrentPart");
            }
        }

        private AbstractMonitor _currentMonitor;
        public AbstractMonitor CurrentMonitor
        {
            get
            {
                return this._currentMonitor;
            }
            set
            {
                this._currentMonitor = value;
                RaisePropertyChanged("CurrentMonitor");
            }
        }

        private int _tcSelectedIndex;

        public int tcSelectedIndex
        {
            get { return _tcSelectedIndex; }
            set
            {
                _tcSelectedIndex = value;
                RaisePropertyChanged("tcSelectedIndex");
            }
        }

        //private readonly Dictionary<string, AbstractPart> _partDictionary = new Dictionary<string, AbstractPart>();   

        //private Dictionary<string, Dictionary<string, ObservableCollection<LogModel>>> _logDataDic;
        public Dictionary<string, ObservableCollection<LogModel>> _logDataDic;

        //private Dictionary<string, Dictionary<string, bool>> _isOpenFileDic = new Dictionary<string, Dictionary<string, bool>>();   //Dic[ChipID][PageName]
        public Dictionary<string, bool> _isOpenFileDic = new Dictionary<string, bool>();   //Dic[ChipID]

        private Visibility _monitoringVisibility;
        public Visibility MonitoringVisibility
        {
            get { return _monitoringVisibility; }
            set
            {
                _monitoringVisibility = value;
                RaisePropertyChanged(nameof(MonitoringVisibility));
            }
        }

        #endregion

        //public CipherSqliteHelper sqlite = new CipherSqliteHelper();

        #region UserCommand

        public ICommand AddDeviceCommand => new RelayCommand<string>(AddDeviceMethodMask, null);
        public void AddDeviceMethodMask(string info = null)
        {
            AddDeviceMethod(info, true);
        }
        public void AddDeviceMethod(string info = null, bool needSelectChip = true)
        {
            try
            {
                lock (_lockObj)
                {
                    string stringname = string.Empty;
                    string addressChoosed = string.Empty;
                    // ConstantModel.ProjectSaveStatus.SaveStatus(ProConfig);
                    bool isVirtual = (info == "Virtual");
                    if (!isVirtual && info != null)
                    {
                        stringname = info.Split(' ')[0];
                        addressChoosed = info.Split(' ')[1];
                    }
                    ProjectConfig newConfig=null;
                 
                    AddDevices(info,isVirtual,stringname,newConfig,addressChoosed,needSelectChip);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in AddDeviceMethod: " + ex.Message);
            }
        }

        public void AddDevices(string info, bool isVirtual, string stringname, ProjectConfig newConfig, string addressChoosed, bool needSelectChip, bool displayChooseChipWindow=true) 
        {
            while (true)
            {
                if (displayChooseChipWindow) 
                {
                    if (info == null || isVirtual)
                    {
                        ChooseChipWindow choosewin = new ChooseChipWindow(UserLicense);
                        var flg = choosewin.ShowDialog();
                        if (flg != null & flg == false)
                            return;
                        stringname = (choosewin.DataContext as ChooseChipWindowViewModel).SelectedChip;
                    }
                }
                
                //newConfig = MPS.Common.DataBaseHelper.LoadConfig(stringname);
                
                newConfig = MPS.VBP.PartFactory.DataBaseHelper.LoadConfig(stringname, UserLicense.IsMPSAE);

                if (newConfig == null)
                {
                    return;
                }

                if (!newConfig.Chips.FirstOrDefault().ChipName.Equals(stringname))
                {
                    var errUI = new WinError($"Part number \"{stringname}\" does not match the one in specification file.");
                    errUI.ShowDialog();
                    return;
                }

                if (!isVirtual && addressChoosed == string.Empty && !newConfig.Chips.FirstOrDefault().ChipName.Contains("MPF52000") && !newConfig.Chips.FirstOrDefault().ChipName.Contains("MP2658"))
                {
                    MatchAddressWindow mtawin = new Views.DeviceViews.MatchAddressWindow(newConfig.Chips.FirstOrDefault());
                    var dresult = mtawin.ShowDialog();
                    if (mtawin.Back)
                    {
                        stringname = null;
                        continue;
                    }
                    if (dresult == null || !(bool)dresult)
                    {
                        return;
                    }
                    addressChoosed = (mtawin.DataContext as MatchAddressView).AddressSelected;
                    if (!mtawin.Back)
                        break;
                }
                else   //  isVirtual || addressChoosed != string.Empty
                {
                    break;
                }

            }

            //To prevent add identical address
            if (!newConfig.Chips.FirstOrDefault().ChipName.Contains("MPF32010") && !newConfig.Chips.FirstOrDefault().ChipName.Contains("MPF52000") && !newConfig.Chips.FirstOrDefault().ChipName.Contains("MP2658"))
            {
                if (ProConfig.Chips.Any(o => o.ChipAddr == addressChoosed) || (ProConfig.Chips.Any(o => o.ChipAddr.Contains("SPI")) && addressChoosed.Contains("SPI")))
                {
                    //MessageBox.Show(addressChoosed + " already exists.");
                    string ErrMessage = addressChoosed.Contains("SPI") ? "SPI" : addressChoosed;
                    var errUI = new WinError(ErrMessage + " already exists.");
                    errUI.ShowDialog();
                    var cp = ProConfig.Chips.FirstOrDefault(o => o.ChipAddr == addressChoosed);
                    ChangeChipSelectionMethod(cp);
                    return;
                }
                //else if (ProConfig.Chips.Any(o => o.ChipName == newConfig.Chips.FirstOrDefault().ChipName && addressChoosed == "SPI"))
                //{
                //    //MessageBox.Show($"{newConfig.Chips.FirstOrDefault().ChipName}({addressChoosed})" + " already exists.");
                //    var errUI = new WinError($"{newConfig.Chips.FirstOrDefault().ChipName}({addressChoosed})" + " already exists.");
                //    errUI.ShowDialog();
                //    var cp = ProConfig.Chips.FirstOrDefault(o => o.ChipAddr == addressChoosed && o.ChipName == newConfig.Chips.FirstOrDefault().ChipName);
                //    ChangeChipSelectionMethod(cp);
                //    return;
                //}
            }
            else
            {
                if (ProConfig.Chips.Any(o => o.ChipName == newConfig.Chips.FirstOrDefault().ChipName) && !isVirtual)
                {
                    //MessageBox.Show(newConfig.Chips.FirstOrDefault().ChipName + " already exists.");
                    var errUI = new WinError(newConfig.Chips.FirstOrDefault().ChipName + " already exists.");
                    errUI.ShowDialog();
                    return;
                }

                if (addressChoosed.Contains("COM") && addressChoosed != "COMx")
                {
                    DeviceObject.Devices.PMBus.DeviceName = addressChoosed;
                }
                else
                {
                    newConfig.Chips.FirstOrDefault().ChipAddr = "-";
                }
            }

            UpdateProjectConfig(addressChoosed, isVirtual, newConfig);

            if (needSelectChip)
            {
                ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());
            }
            if (CurrentChip != null && CurrentChip.Monitor != null)
                CurrentChip.Monitor.LogData = LogData;

            main_window.ProConfigs = ProConfig;
            main_window.Configs = ProConfig;
        }

        public void UpdateProjectConfig(string addressChoosed, bool isVirtual, ProjectConfig newConfig)
        {
            if (ProConfig.Chips.Count == 0)
            {
                ProConfig.Product = newConfig.Product;
                ProConfig.BaseProduct = newConfig.BaseProduct;
                ProConfig.CreatedDate = newConfig.CreatedDate;
                ProConfig.CreatedTool = newConfig.CreatedTool;
                ProConfig.CreatedToolVersion = newConfig.CreatedToolVersion;
                ProConfig.LastModifiedDate = newConfig.LastModifiedDate;
                ProConfig.LastModifiedTool = newConfig.LastModifiedTool;
                ProConfig.LastModifiedToolVersion = newConfig.LastModifiedToolVersion;
                ProConfig.SpecVersion = newConfig.SpecVersion;
                main_window.ChangeProjectView(2);

            }

            foreach (var cp in newConfig.Chips)
            {
                if (isVirtual)
                {
                    var virtualDevices = ProConfig.Chips.Where(chip => chip.ChipAddr.Contains('v'));
                    string virtualAddr = string.Empty;
                    if (virtualDevices.Count() == 0)
                    {
                        virtualAddr = "v1";
                    }
                    else
                    {
                        virtualAddr = "v" + (virtualDevices.Max(chip => Convert.ToInt16(chip.ChipAddr.Substring(1, chip.ChipAddr.Length - 1))) + 1).ToString();
                    }
                    cp.ChipAddr = virtualAddr;

                }
                else
                {
                    if (!string.IsNullOrEmpty(addressChoosed))
                    {
                        cp.ChipAddr = addressChoosed;
                    }

                }

                //Create Monitor
                cp.Monitor = MonitorManager.CreateInstance(cp, false, UserLicense.IsMPSAE);
                if (cp.Monitor != null)
                {
                    if (cp.Monitor.LineGraphs.Count == 0)
                    {
                        cp.Monitor.LoadMonitorValue();
                    }
                    cp.Monitor.isRunning = true;
                }

                while (ProConfig.Chips.Any(chip => chip.ChipID == cp.ChipID))
                {
                    cp.ChipID = cp.ChipID + "*";
                }
                ProConfig.Chips.Add(cp);

                //var templogdic = new Dictionary<string, ObservableCollection<LogModel>>();
                //foreach (var page in cp.Pages)
                //{
                //    templogdic.Add(page.PageName, new ObservableCollection<LogModel>());
                //}
                //_logDataDic.Add(cp.ChipAddr, templogdic);
                if (_logDataDic.ContainsKey(cp.ChipID))
                    _logDataDic.Remove(cp.ChipID);
                _logDataDic.Add(cp.ChipID, new ObservableCollection<LogModel>());
            }
        }

        public ICommand DeleteChipCommand => new RelayCommand<object>(DeleteChipMethod, null);

        public void DeleteChipMethod(object _selectedChip)
        {
            try
            {
                var selectedChip = _selectedChip as MPS.Common.Model.Chip;
                if (selectedChip == null)
                    return;

                lock (_lockObj)
                {
             
                    _logDataDic.Remove(selectedChip.ChipID);
                    if (_isOpenFileDic.ContainsKey(selectedChip.ChipID))
                    {
                        _isOpenFileDic.Remove(selectedChip.ChipID);
                    }

                    if (ProConfig.Chips.Count == 1)
                    {
                        ClearBindingData();                        
                        main_window.ChangeProjectView(1);
                        ProConfig.Chips.Remove(selectedChip);
                    }
                    else 
                    {
                        if (selectedChip.Monitor != null)
                        {
                            selectedChip.Monitor.Dispose();
                        }
                        if (selectedChip == CurrentChip)
                        {
                            ProConfig.Chips.Remove(selectedChip);
                            ChangeChipSelectionMethod(ProConfig.Chips.FirstOrDefault());
                        }
                        else
                        {
                            ProConfig.Chips.Remove(selectedChip);
                        }
                    }
                    //tcSelectedIndex = 0;
                    ChangeSelectedTab();
                    main_window.ProConfigs = ProConfig;
                    main_window.Configs = ProConfig;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in DeleteDeviceMethod: " + ex.Message);
            }
        }

        public ICommand ChangeChipSelectionCommand => new RelayCommand<object>(ChangeChipSelectionMethod, null);

        public void ChangeChipSelectionMethod(object _selectedChip)
        {
            try
            {
                if (_selectedChip == null || CurrentChip == _selectedChip)
                    return;

                lock (_lockObj)
                {
                    //if (CurrentChip != null && CurrentChip.Monitor != null)
                    //{
                    //    CurrentChip.Monitor.isRunning = false;
                    //}

                    //Then update CurrentChip
                    CurrentChip = _selectedChip as MPS.Common.Model.Chip;
                    CurrentChip.IsSelected = true;


                    _communicationObj = CommunicationFactory.CreateInstance(CurrentChip.ChipAddr, CurrentChip.ChipName);

                    //Overview  
                    Description = string.Empty;
                    Features = string.Empty;
                    if (CurrentChip.Specifications.Count > 0)
                    {
                        //SqliteHelper sqlite = new SqliteHelper();
                        var spec = ConstantModel.sqlite.LoadSpecInfos(CurrentChip.Specifications);
                        DescriptionTitle = spec.DescriptionTitle;
                        foreach (var line in spec.Description)
                        {
                            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                                continue;

                            Description = Description + line + "\r";
                        }
                        FeaturesTitle = spec.FeaturesTitle;
                        foreach (var line in spec.Features)
                        {
                            Features = Features + line + "\r";
                        }
                        DiagramTitle = spec.DiagramTitle;
                        Diagram = Views.CustomElements.Help_GroupBox.Bytes2BitmapImage(spec.Diagram);
                    }


                    //if (!CurrentChip.Pages.Any(p => p.IsSelected == true))
                    //{
                    //    CurrentChip.Pages.FirstOrDefault().IsSelected = true;
                    //}

                    if (CurrentPart == null || CurrentPart.Partnumber != CurrentChip.ChipName)
                    {
                        if (CurrentPart != null)
                        {
                            CurrentPart.Dispose();
                        }
                        CurrentPart = PartnumberFactory.CreateInstance(CurrentChip);
                    }
                    PageSelect();

                    //if (CurrentChip.Monitor != null)
                    //{
                    //    if (CurrentChip.Monitor.LineGraphs.Count == 0)
                    //    {
                    //        CurrentChip.Monitor.LoadMonitorValue();
                    //    }
                    //    CurrentChip.Monitor.isRunning = true;
                    //    CurrentChip.Monitor.isSelected = true;
                    //    loadWaveforms();
                    //}

                    ChangeSelectedTab();

                    LogData = _logDataDic[CurrentChip.ChipID];

                    //Read once after adding
                    if (CurrentChip.ChipName != "MP2658")
                    {
                        if (!_isOpenFileDic.ContainsKey(CurrentChip.ChipID) || !_isOpenFileDic[CurrentChip.ChipID])
                        {
                            if (LogData.Count == 0)
                            {
                                try
                                {
                                    //main_window.UpdateAddressAvaliable(CurrentChip);
                                    CurrentChip.Monitor.UpdateAddressConnection();
                                    if (CurrentChip.IsAddrAvalible != null && (bool)CurrentChip.IsAddrAvalible)
                                    {
                                        //_communicationObj.ReadRegisters(LogData, CurrentChip);
                                        Task.Factory.StartNew(() =>
                                        {
                                            //lock (main_window._lockObj)
                                            //{
                                            try
                                            {
                                                //terminate all monitors
                                                foreach (var chip in ProConfig.Chips)
                                                {
                                                    if (chip != null && chip.Monitor != null)
                                                    {
                                                        chip.Monitor.isRunning = false;
                                                    }
                                                }


                                                main_window.UIEnabled = false;
                                                BusyIndicator.LoadingIndicator.LoadingUIRun1("Initial read");
                                                Stopwatch sw = new Stopwatch();
                                                sw.Start();
                                                while (!DeviceObject.Devices.PMBus.SetStatus(true, "Initial read"))
                                                {
                                                    if (sw.ElapsedMilliseconds >= 30000)   //30s
                                                    {
                                                        throw new TimeoutException();
                                                    }
                                                    System.Threading.Thread.Sleep(5);
                                                }

                                                _communicationObj.ReadRegisters(LogData, CurrentChip);
                                                if (CurrentChip.ChipName == "MP8850" || CurrentChip.ChipName == "MPM2896-20" || CurrentChip.ChipName == "MPM3898-50")
                                                {
                                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                                    {
                                                        PageSelect();
                                                    }));
                                                }
                                                BusyIndicator.LoadingIndicator.LoadingUIStop();
                                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                                {
                                                    var ui = new WinDone();
                                                    ui.ShowDialog();
                                                }));
                                                main_window.UIEnabled = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                BusyIndicator.LoadingIndicator.LoadingUIStop();
                                                string msg = "Initial read failed.";

                                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                                {
                                                    var ui = new WinError(msg);
                                                    ui.ShowDialog();
                                                }));

                                                //OperationEnabled = true;
                                                main_window.UIEnabled = true;
                                            }
                                            finally
                                            {

                                                if (_isOpenFileDic.ContainsKey(CurrentChip.ChipID))
                                                {
                                                    //_isOpenFileDic[CurrentChip.ChipID][currentpage] = false;
                                                    _isOpenFileDic[CurrentChip.ChipID] = false;

                                                }

                                                DeviceObject.Devices.PMBus.SetStatus(false, "Initial read");

                                                //restart all monitor
                                                foreach (var chip in ProConfig.Chips)
                                                {
                                                    if (chip != null && chip.Monitor != null)
                                                    {
                                                        chip.Monitor.isRunning = true;
                                                    }
                                                }
                                            }
                                            //}
                                        });
                                    }
                                    else
                                    { 
                                    }

                                }
                                catch (Exception ex)
                                {
                                    var x = ex.Message;
                                    Console.WriteLine("Initial read fail");
                                }


                            }
                        }
                    }

                    if (CurrentChip.Monitor != null)
                    {
                        //if (CurrentChip.Monitor.LineGraphs.Count == 0)
                        //{
                        //    CurrentChip.Monitor.LoadMonitorValue();
                        //}
                        //CurrentChip.Monitor.isRunning = true;
                        if (CurrentChip.ChipName == "MPQ8896" || CurrentChip.ChipName == "MPQ70170FS" || CurrentChip.ChipName == "MPQ70171FS")
                        {
                            CurrentChip.Monitor.LogData = LogData;
                        }
                        loadWaveforms();
                        if (CurrentChip.ChipName == "MP2772" || CurrentChip.ChipName == "MP2764" || CurrentChip.ChipName == "MP2764A")
                        {
                            loadBatteryLines();
                            loadJeitaLines();
                        }
                    }

                    if (CurrentChip.ChipName == "MP5513")
                    {
                        BlackBoxRegisterMap = CurrentChip.Regions.FirstOrDefault(o => o.RegionName.Contains("I2C")).ConfigurationModes.FirstOrDefault().Pages.FirstOrDefault(o => o.PageName == "BlackBox").AllRegisters;
                        if (BlackBoxViewRegisterMap == null)
                        {
                            BlackBoxViewRegisterMap = new ObservableCollection<Register>(); 
                        }
                        BlackBoxViewRegisterMap.Clear();
                        foreach (var reg in BlackBoxRegisterMap)
                        {
                            BlackBoxViewRegisterMap.Add(reg);
                        }
                    }
                    else
                    {
                        BlackBoxRegisterMap = null;
                        BlackBoxViewRegisterMap = null;
                    }
                    RegisterRegionforBk.Clear();
                    foreach (var item in CurrentChip.Regions)
                    {
                        // Hide the OTP Region of MP5417. Which might be used in later release.
                        if (CurrentChip.ChipName == "MP5417" && item.RegionName == "OTP Region")
                        {
                            continue;
                        }

                        if (UserLicense.IsMPSAE)
                        {
                            RegisterRegionforBk.Add(item);
                        }
                        else if (item.RegionName != "Debug")
                        {
                            RegisterRegionforBk.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ChangeChipSelectionMethod: " + ex.Message);
            }
        }

        public ICommand PageSelectCommand => new RelayCommand(PageSelect);

        private void PageSelect()
        {
            try
            {
                if (CurrentChip == null)
                    return;

                if (!CurrentChip.Regions.Any(o => o.IsSelected == true))
                {
                    CurrentChip.Regions.FirstOrDefault().IsSelected = true;
                }
                else
                {
                    var selectedRegion = CurrentChip.Regions.FirstOrDefault(o => o.IsSelected == true);
                    if (selectedRegion.RegionName == "Overview")
                    {
                        return;
                    }
                }

                var currentRegion = CurrentChip.Regions.FirstOrDefault(o => o.IsSelected == true);

                if (currentRegion.ConfigurationModes.Count > 1)
                {
                    
                    foreach (var mode in currentRegion.ConfigurationModes)
                    {
                        mode.IsSelected = false;
                    }
                    if (!string.IsNullOrEmpty(currentRegion.ModeSelectParameter.SelectedModeName))
                    {
                        currentRegion.ConfigurationModes.FirstOrDefault(o => o.ModeName == currentRegion.ModeSelectParameter.SelectedModeName).IsSelected = true;
                    }
                    else
                    {
                        currentRegion.ConfigurationModes.FirstOrDefault().IsSelected = true;
                        currentRegion.ModeSelectParameter.SelectedModeName = currentRegion.ConfigurationModes.FirstOrDefault(o => o.IsSelected == true).ModeName;
                    }
                }
                else  //only got one config mode
                {
                    currentRegion.ConfigurationModes.FirstOrDefault().IsSelected = true;

                }
                CurrentConfigMode = currentRegion.ConfigurationModes.FirstOrDefault(o => o.IsSelected == true);
                CurrentPart.RegisterRegionName = "";
                CurrentPart.RegisterRegionName = currentRegion.RegionName;
                if (!CurrentConfigMode.Pages.Any(p => p.IsSelected == true))
                {
                    CurrentConfigMode.Pages.FirstOrDefault().IsSelected = true;
                }
                ////var selectedPageName = CurrentConfigMode.Pages.Where(p => p.IsSelected == true).First().PageName;
                //CurrentPage = CurrentConfigMode.Pages.FirstOrDefault(p => p.IsSelected == true);
                foreach (var page in CurrentConfigMode.Pages)
                {
                    if (page.IsSelected)
                    {
                        CurrentPage = page;
                        break;
                    }
                }

                if (CurrentPart != null)
                {
                    CurrentPart.ControlPage = CurrentPage;
                    //if (UserLicense.Customer != null)
                    //{
                    //    CurrentPart.Customer = UserLicense.Customer;
                    //}
                    //else 
                    //{
                    //    CurrentPart.Customer = "No_License";
                    //}
                  
                }

                var BasicCtag = CurrentPage.Configurations.FirstOrDefault(o => o.Name == "Basic Configuration");
                if (BasicCtag != null)
                {
                    BasicSetting = BasicCtag.Categories.FirstOrDefault().Subcategories.FirstOrDefault().ParameterGroups;
                }
                else
                {
                    BasicSetting = null;
                }
                var AdvancedCatg = CurrentPage.Configurations.FirstOrDefault(o => o.Name == "Advanced Configuration");
                if (AdvancedCatg != null)
                {
                    AdvancedSetting = AdvancedCatg.Categories;
                }
                else
                {
                    AdvancedSetting = null;
                }

                //CategorySelectedId = CategorySelectedId < 0 ? 0 : CategorySelectedId;
                //SubcategorySelectedId = SubcategorySelectedId < 0 ? 0 : SubcategorySelectedId;          
                CategorySelectedId = 0;
                SubcategorySelectedId = 0;

                //Add only registers in current page
                if (CurrentRegisterMap != null)
                {
                    CurrentRegisterMap.Clear();
                }
                else
                {
                    CurrentRegisterMap = new ObservableCollection<MPS.Common.Model.Register>();
                }
                
                //foreach (var reg in CurrentPage.AllRegisters.OrderBy(o => o.Page).ThenBy(o => o.Address))
                foreach (var reg in CurrentPage.AllRegisters.OrderBy(o => o.Page).ThenBy(o => (o.Address_H << 8) + o.Address))
                {
                    if (reg.Disabled)
                    {
                        continue;
                    }
                    CurrentRegisterMap.Add(reg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in PageSelect: " + ex.Message);
            }
        }

        public ICommand CommunicationCommand => new RelayCommand<string>(CommunicationMethod);

        private bool isFirstTimeWriteRAMDone = true;
        public bool isShowWriteROMWarningWindowWithReadbackConent = true;
        public bool isShowWriteROMWarningWindowWithoutReadbackConent = true;
        private void CommunicationMethod(string param)
        {
            if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
            {
                Task.Factory.StartNew(() =>
                {
                    //lock (main_window._lockObj)
                    {
                        try
                        {
                            main_window.UIEnabled = false;

                            bool isReadbackSupported = CheckReadbackEnabled();
                            bool isShowWriteROMWarningWindow = isReadbackSupported ? isShowWriteROMWarningWindowWithReadbackConent : isShowWriteROMWarningWindowWithoutReadbackConent;
                            if (param == "WriteROM" && isShowWriteROMWarningWindow && CurrentChip.ChipName != "MP3718")
                            {
                                WriteROMWarningWindow writeROMWarningWindow = null;
                                bool? res = null;
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    writeROMWarningWindow = new WriteROMWarningWindow(isReadbackSupported);
                                    res = writeROMWarningWindow.ShowDialog();
                                });

                                if (writeROMWarningWindow.IsDontShowAgainChecked)
                                {
                                    if (isReadbackSupported)
                                    {
                                        isShowWriteROMWarningWindowWithReadbackConent = false;
                                    }
                                    else
                                    {
                                        isShowWriteROMWarningWindowWithoutReadbackConent = false;
                                    }
                                }

                                if (res != true)
                                {
                                    main_window.UIEnabled = true;
                                    return;
                                }
                            }
                            BusyIndicator.LoadingIndicator.LoadingUIRun1(param);
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (!DeviceObject.Devices.PMBus.SetStatus(true, "communciation"))
                            {
                                if (sw.ElapsedMilliseconds >= 30000)   //30s
                                {
                                    throw new TimeoutException();
                                }
                                System.Threading.Thread.Sleep(100);
                            }
                            switch (param)
                            {
                                case "WriteROM":
                                    var needAll_ = _isOpenFileDic.ContainsKey(CurrentChip.ChipID) ? _isOpenFileDic[CurrentChip.ChipID] : false;
                                    //_communicationObj.WriteRegisters(LogData, CurrentChip, needAll_, false);
                                    //System.Threading.Thread.Sleep(500);
                                    _communicationObj.WriteMTP(LogData, CurrentChip, needAll_);
                                    break;
                                case "ReadROM":
                                    _communicationObj.RestorMTP(LogData, CurrentChip);
                                    //_communicationObj.ReadRegisters(LogData, CurrentChip, false);
                                    break;
                                case "WriteRAM":
                                    var needAll = _isOpenFileDic.ContainsKey(CurrentChip.ChipID) ? _isOpenFileDic[CurrentChip.ChipID] : false;
                                    _communicationObj.WriteRegisters(LogData, CurrentChip, needAll, false);
                                    break;
                                case "ReadRAM":
                                    CurrentChip.Monitor.LogData = LogData;
                                    _communicationObj.ReadRegisters(LogData, CurrentChip, false);
                                    if (CurrentChip.ChipName == "MP8850" || CurrentChip.ChipName == "MPM2896-20" || CurrentChip.ChipName == "MPM3898-50")
                                    {
                                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                        {
                                            PageSelect();
                                        }));
                                    }
                                    CurrentChip.Monitor.isUpdateFaultStatus = true;
                                    break;
                                case "Readback":
                                    CurrentChip.Monitor.LogData = LogData;
                                    _communicationObj.ReadbackMethod(LogData, CurrentChip, false);
                                    break;
                                case "ClearFault":
                                    _communicationObj.Clear(LogData);
                                    _communicationObj.ReadRegisters(LogData, CurrentChip, false);
                                    break;
                                case "ReadFault":
                                    CurrentChip.Monitor.LogData = LogData;
                                    CurrentChip.Monitor.isUpdateFaultStatus = true;                                    
                                    break;
                                case "ReadMask":
                                    //CurrentChip.Monitor.LogData = LogData;
                                    CurrentChip.Monitor.ReadStatusMethod(LogData);
                                    break;
                                default:
                                    break;
                            }

                            DeviceObject.Devices.PMBus.SetStatus(false, "communication");

                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            if (param == "WriteRAM")
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    var msg = "Write operation complete.Please\nread the RAM back and verify the\nresults in the Register Map!";
                                    WinDone ui;
                                    if (isFirstTimeWriteRAMDone)
                                    {
                                        ui = new WinDone(msg, 3000, 120, 250);
                                        isFirstTimeWriteRAMDone = false;
                                    }
                                    else
                                    {
                                        ui = new WinDone(msg, 1000, 120, 250);
                                    }
                                    ui.ShowDialog();
                                }));
                            }
                            else if (param != "Readback")
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    var ui = new WinDone();
                                    ui.ShowDialog();
                                }));
                            }
                            main_window.UIEnabled = true;

                            if (param.Contains("Write") || param.Contains("Read"))
                            {
                                if (_isOpenFileDic.ContainsKey(CurrentChip.ChipID))
                                {
                                    _isOpenFileDic[CurrentChip.ChipID] = false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DeviceObject.Devices.PMBus.SetStatus(false, "communication");
                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            string msg = ex.Message;
                            if (ex.Message != "Cancelled")
                            {
                                if (ex.GetType() == typeof(TimeoutException))
                                {
                                    msg = "Time out, please try again.";
                                }
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    var ui = new WinError(msg);
                                    ui.ShowDialog();
                                }));
                            }
                            ////OperationEnabled = true;
                            main_window.UIEnabled = true;
                        }
                        //finally
                        //{
                        //    if (param.Contains("Write") || param.Contains("Read"))
                        //    {
                        //        if (_isOpenFileDic.ContainsKey(CurrentChip.ChipID))
                        //        {
                        //            //_isOpenFileDic[CurrentChip.ChipID][currentpage] = false;
                        //            _isOpenFileDic[CurrentChip.ChipID]= false;

                        //        }
                        //    }
                        //    //DeviceObject.Devices.PMBus.SetStatus(false, "communication");


                        //}
                    }
                });
            }
            else
            {
                main_window.UIEnabled = false;
                var ui = new WinDeviceDisconnected();
                ui.ShowDialog();
                main_window.UIEnabled = true;
            }
        }

        private bool CheckReadbackEnabled()
        {
            var pn = (EnumPartnumber)Enum.Parse(typeof(EnumPartnumber), CurrentChip.ChipName.Replace('-', '_'));
            switch (pn)
            {
                case EnumPartnumber.MP8796B:
                case EnumPartnumber.MPM3695_10:
                case EnumPartnumber.MPM3695_25:
                case EnumPartnumber.MPM3695_100:
                case EnumPartnumber.mEZDPD1620A:
                case EnumPartnumber.mEZDPD1620AS:
                case EnumPartnumber.MPM82504:
                case EnumPartnumber.MPM3701:
                case EnumPartnumber.MPM3690_50D:
                case EnumPartnumber.CDM1680:
                case EnumPartnumber.MP9795B:
                case EnumPartnumber.MPM3695_20:
                case EnumPartnumber.MP8796K:
                case EnumPartnumber.MPM3690S_15:
                case EnumPartnumber.MPM3895_25:
                case EnumPartnumber.MPM3695GMJ_15:
                case EnumPartnumber.MPQ8645P:
                case EnumPartnumber.MPM3695L_20:
                    return true;
                default:
                    return false;
            }
        }

        public ICommand OperateChipComand => new RelayCommand<string>(OperateChipMethod);

        private void OperateChipMethod(string param)
        {
            try
            {
                bool isConnected = false;
                if (CurrentChip != null)
                    isConnected = (CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) ? true : false;
                switch (param)
                {
                    case "MTPexport":
                        var tempConfig = new ProjectConfig();
                        var res = main_window.ConfigCheck_Before_SaveProj(ProConfig, out tempConfig, true);
                        if (!res) return;

                        var ucsm_view = (MPS.VBP.StateMachineDiagram.FlowChart.FlowchartEditor)main_window.ucsm_view;
                        var sRamInformation = ucsm_view.GetSRamInformation();
                        var mtpUC = UtilityMTPFactory.ExportTxtMethod(tempConfig, UserLicense.IsMPSAE, sRamInformation, isConnected);
                        mtpUC.ShowDialog();
                        break;
                    case "Compare":
                        UtilityMTPFactory.CompareTxtMethod(CurrentChip, isConnected);
                        break;
                    case "ClearLog":
                        LogData.Clear();
                        break;
                    case "CopyLog":
                        CopyLog();
                        break;
                    case "ScanI2C":
                        ScanI2CMethod();
                        break;
                    case "AddDevice":
                        AddDeviceMethodMask();
                        break;
                    case "Virtual":
                        AddDeviceMethodMask("Virtual"); //Virtual
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in OperateChipMethod: " + ex.Message);
            }
        }


        public ICommand MonitorTabChangedCommand => new RelayCommand<TabItem>(MonitorTabChangedMethod, null);

        public void MonitorTabChangedMethod(TabItem tabitem)
        {
            if (CurrentChip != null)
            {
                if (tabitem.Header.ToString() == "Waveforms" && CurrentChip.Monitor != null)
                {
                    loadWaveforms();
                }
                else if (tabitem.Header.ToString() == "Register Map")
                {
                    if (SelectedRegsiterIndex == -1)
                    {
                        SelectedRegsiterIndex = 0;
                    }
                }
            }

        }



        public ICommand ChangeAddressCommand => new RelayCommand(ChangeAddressMethod, null);

        public void ChangeAddressMethod()
        {
            try
            {
                if (ProConfig == null || ProConfig.Chips.Count == 0 || CurrentChip == null || CurrentChip.ChipAddr.Contains("v"))
                    return;


                MatchAddressWindow mtawin = new Views.DeviceViews.MatchAddressWindow(CurrentChip, false);
                var dresult = mtawin.ShowDialog();
                if (dresult == null || !(bool)dresult)
                {
                    return;
                }
                var changedAddr = (mtawin.DataContext as MatchAddressView).AddressSelected;

                //To prevent add identical address
                if (ProConfig.Chips.Any(o => o.ChipAddr == changedAddr)) /*&& changedAddr != "SPI")*/
                {
                    //MessageBox.Show(changedAddr + " already exists.");
                    var errUI = new WinError(changedAddr + " already exists.");
                    errUI.ShowDialog();
                    return;
                }

                if (!changedAddr.Contains("COM"))
                {
                    if (changedAddr != "SPI")
                    {
                        byte chipAddr = Convert.ToByte(changedAddr, 16);
                        CurrentChip.ChipAddr = changedAddr;
                        _communicationObj.Address = chipAddr;
                        CurrentChip.Monitor.SlaveAddr = chipAddr;
                    }
                    else
                    {
                        CurrentChip.ChipAddr = changedAddr;
                    }
                    //ProConfig.Chips.FirstOrDefault(o => o == CurrentChip).Pages.FirstOrDefault().IsSelected = true;
                    ProConfig.Chips.FirstOrDefault(o => o == CurrentChip).Regions.FirstOrDefault().ConfigurationModes.FirstOrDefault().Pages.FirstOrDefault().IsSelected = true;

                    PageSelect();
                }
                else  //COM
                {
                    if (changedAddr != "COMx")
                    {
                        DeviceObject.Devices.PMBus.DeviceName = changedAddr;
                    }

                    CurrentChip.ChipAddr = changedAddr;
                    ProConfig.Chips.FirstOrDefault(o => o == CurrentChip).Regions.FirstOrDefault().ConfigurationModes.FirstOrDefault().Pages.FirstOrDefault().IsSelected = true;
                    PageSelect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ChangeAddressMethod: " + ex.Message);
            }

        }

        public ICommand SetFriendlyNameCommand => new RelayCommand(SetFriendlyNameMethod, null);

        public void SetFriendlyNameMethod()
        {
            var win = new SetFriendlyNameWindow(CurrentChip);
            win.ShowDialog();
        }

        public ICommand showHelpInfoCommand => new RelayCommand<string>(showHelpInfoMethod, null);

        private void showHelpInfoMethod(string key)
        {
            //SqliteHelper sqlite = new SqliteHelper();
            var helpGroupList = ConstantModel.sqlite.GetGroupListbyId(key);
            if (helpGroupList.Value == null && helpGroupList.Key == null)
                return;

            var win = new Views.Help_Window(helpGroupList.Key, helpGroupList.Value);
            win.ShowDialog();
        }

        public ICommand CmdTextGotFocusCommand => new RelayCommand(CmdTextGotFocusCommandMethod);

        public void CmdTextGotFocusCommandMethod()
        {
            lock (CurrentPart.lockFocus)
            {
                CurrentPart.CanLsbMonitor = false;
            }

        }
        public ICommand CmdTextLostFocusCommand => new RelayCommand(CmdTextLostFocusCommandMethod);

        //public void CmdTextLostFocusCommandMethod(string text)
        public void CmdTextLostFocusCommandMethod()
        {
            if (CurrentPart != null)
            {
                lock (CurrentPart.lockFocus)
                {
                    CurrentPart.CanLsbMonitor = true;
                }
            }
        }
        //CmdTextFocusCommand
        public ICommand ParamItemFocusCommand => new RelayCommand<object>(ParamItemFocusMethod);

        public void ParamItemFocusMethod(object obj)
        {
            //var item = obj as System.Windows.Controls.TabItem;
            var item = obj as System.Windows.Controls.TabControl;
            item?.Focus();
        }

        public ICommand UtilityCommand => new RelayCommand<BitControl>(UtilityCommandMethod);

        public void UtilityCommandMethod(BitControl bt)
        {
            if (GetisBitControlButton(bt))
            {
                if (CurrentChip.IsAddrAvalible == true || CurrentChip.ChipAddr.Contains("v"))
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            main_window.UIEnabled = false;
                            BusyIndicator.LoadingIndicator.LoadingUIRun1("Execute");
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (!DeviceObject.Devices.PMBus.SetStatus(true, "button param command"))
                            {
                                if (sw.ElapsedMilliseconds >= 3000)   //30s
                                {
                                    throw new TimeoutException();
                                }
                                //System.Threading.Thread.Sleep(500);
                            }

                            _communicationObj.UtilityMethod(LogData, bt, CurrentChip);

                            DeviceObject.Devices.PMBus.SetStatus(false, "button param command");

                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                var ui = new WinDone();
                                ui.ShowDialog();
                            }));
                            main_window.UIEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            DeviceObject.Devices.PMBus.SetStatus(false, "button param command");
                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            string msg = ex.Message;
                            if (ex.Message != "Cancelled")
                            {
                                if (ex.GetType() == typeof(TimeoutException))
                                {
                                    msg = "Time out, please try again.";
                                }
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    var ui = new WinError(msg);
                                    ui.ShowDialog();
                                }));
                            }
                            //OperationEnabled = true;
                            main_window.UIEnabled = true;
                        }
                    });
                }
                else
                {
                    main_window.UIEnabled = false;
                    var ui = new WinError("Device not found: Please check the device connection.");
                    ui.ShowDialog();
                    main_window.UIEnabled = true;
                }
            }
            else //isToggle
            {
                bool isUpdateVirtualDeviceNeeded = false;
                if (!((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v") ||
                    CurrentChip.ChipName == "MP2764" || CurrentChip.ChipName == "MP2764A" || bt.BitName == "SPI Enable"))
                {
                    isUpdateVirtualDeviceNeeded = true;
                }
                //if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v") || 
                //    CurrentChip.ChipName == "MP2764" || bt.BitName == "SPI Enable")
                //{
                Task.Factory.StartNew(() =>
                {
                    //lock (main_window._lockObj)
                    {
                        try
                        {
                            main_window.UIEnabled = false;
                            BusyIndicator.LoadingIndicator.LoadingUIRun1(bt.isToggleOn ? "Enable" : "Disable");
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (!DeviceObject.Devices.PMBus.SetStatus(true, "region param command"))
                            {
                                if (sw.ElapsedMilliseconds >= 3000)   //30s
                                {
                                    throw new TimeoutException();
                                }
                                //System.Threading.Thread.Sleep(500);
                            }

                            var isDeviceVirtualBakup = _communicationObj.isVirtual;
                            if (isUpdateVirtualDeviceNeeded)
                            {
                                if (_communicationObj.isVirtual == false)
                                {
                                    _communicationObj.isVirtual = true;
                                }
                            }

                            _communicationObj.UtilityMethod(LogData, bt, CurrentChip);

                            if (isUpdateVirtualDeviceNeeded)
                            {
                                if (_communicationObj.isVirtual != isDeviceVirtualBakup)
                                {
                                    _communicationObj.isVirtual = isDeviceVirtualBakup;
                                }
                            }
                            DeviceObject.Devices.PMBus.SetStatus(false, "button param command");
                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                var ui = new WinDone();
                                ui.ShowDialog();
                            }));
                            main_window.UIEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            DeviceObject.Devices.PMBus.SetStatus(false, "button param command");
                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            string msg = ex.Message;
                            if (ex.Message != "Cancelled")
                            {
                                if (ex.GetType() == typeof(TimeoutException))
                                {
                                    msg = "Time out, please try again.";
                                }
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    var ui = new WinError(msg);
                                    ui.ShowDialog();
                                }));
                            }
                            //OperationEnabled = true;
                            bt.isToggleOn = !bt.isToggleOn;
                            main_window.UIEnabled = true;
                        }
                    }
                });
                //}
                //else
                //{
                //    bt.isToggleOn = !bt.isToggleOn;
                //    main_window.UIEnabled = false;
                //    //var ui = new WinDeviceDisconnected();
                //    var ui = new WinError("Device not found: Please check the device connection.");
                //    ui.ShowDialog();
                //    main_window.UIEnabled = true;
                //}
                //}
            }
        }

        private static bool GetisBitControlButton(BitControl bitControl)
        {
            if (string.IsNullOrEmpty(bitControl?.PossibleValues))
                return false;

            var possibleValueList = bitControl.PossibleValues.Split(';').Select(i => i.Trim()).ToList();
            if (possibleValueList.Count() == 1)
            {
                var possibleValue = possibleValueList[0];
                var splittedPossibleValue = possibleValue.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                var possibleValueName = splittedPossibleValue[0];

                if (possibleValueName == "WO")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public ICommand SelectionChangedCommand => new RelayCommand<BitControl>(SelectionChangedMethod);
        public void SelectionChangedMethod(BitControl bt)
        {
            if (CurrentChip != null && bt.CurrentBitValue != bt.SourceBitValue)
            {
                if ((CurrentChip.ChipName == "MPQ2286" || CurrentChip.ChipName == "MPQ70160FS" || CurrentChip.ChipName == "MPQ70161FS" || CurrentChip.ChipName == "MPQ70165FS" ||
                    CurrentChip.ChipName == "MPQ7932" || CurrentChip.ChipName == "MPQ70166FS" || CurrentChip.ChipName == "MP2421B" || CurrentChip.ChipName == "MPQ2283" || CurrentChip.ChipName == "MPQ2284" ||
                    CurrentChip.ChipName == "MPQ2285" || CurrentChip.ChipName == "MPQ2287" || CurrentChip.ChipName == "MPQ2288" || CurrentChip.ChipName == "MP2422B") &&
                    bt.BitName == "WRITE_PROTECT" && CurrentChip.Regions.FirstOrDefault(pt => pt.IsSelected == true).RegionName == "Parameters")
                {
                    if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            //lock (main_window._lockObj)
                            {
                                try
                                {
                                    main_window.UIEnabled = false;
                                    string Outstring = "";
                                    //if (bt.CurrentBitValue.Contains("commands"))
                                    if (bt.CurrentBitValue.Contains("0b00000000") || bt.CurrentBitValue.Contains("0x00")) // Enable writes to all commands.  
                                    {
                                        Outstring = "Enable";
                                    }
                                    else
                                    {
                                        Outstring = "Disable";
                                    }
                                    BusyIndicator.LoadingIndicator.LoadingUIRun1(Outstring);
                                    Stopwatch sw = new Stopwatch();
                                    sw.Start();
                                    while (!DeviceObject.Devices.PMBus.SetStatus(true, "Write protect command"))
                                    {
                                        if (sw.ElapsedMilliseconds >= 30000)   //30s
                                        {
                                            throw new TimeoutException();
                                        }
                                        System.Threading.Thread.Sleep(500);
                                    }

                                    _communicationObj.SelectionChangedMethod(LogData, bt, CurrentChip);

                                    BusyIndicator.LoadingIndicator.LoadingUIStop();
                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        var ui = new WinDone();
                                        ui.ShowDialog();
                                    }));
                                    main_window.UIEnabled = true;
                                }
                                catch (Exception ex)
                                {
                                    BusyIndicator.LoadingIndicator.LoadingUIStop();
                                    string msg = ex.Message;
                                    if (ex.Message != "Cancelled")
                                    {
                                        if (ex.GetType() == typeof(TimeoutException))
                                        {
                                            msg = "Time out, please try again.";
                                        }
                                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                        {
                                            var ui = new WinError(msg);
                                            ui.ShowDialog();
                                        }));
                                    }
                                    main_window.UIEnabled = true;
                                }
                                finally
                                {
                                    DeviceObject.Devices.PMBus.SetStatus(false, "Write protect command");
                                }
                            }
                        });
                    }
                    else
                    {
                        main_window.UIEnabled = false;
                        var ui = new WinDeviceDisconnected();
                        ui.ShowDialog();
                        main_window.UIEnabled = true;
                        bt.CurrentBitValue = bt.SourceBitValue;
                    }
                }
                else if (CurrentChip.ChipName == "MP2658")
                {
                    bt.CurrentBitValue = bt.SourceBitValue = bt.CurrentBitValue;
                    var bitValue = bt.CurrentBitValue.Contains("0b0") ? 0 : 1;
                    var reg = CurrentChip.Regions[0].ConfigurationModes[0].Pages[0].AllRegisters.FirstOrDefault();
                    if (reg != null)
                    {
                        reg.Value = (uint)((reg.Value & (0xFF - (1 << bt.Position))) + (bitValue << bt.Position));
                    }
                    LogData.Add(new LogModel()
                    {
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RegisterName = bt.ParamName,
                        RegisterAddress = "REG00",
                        RegisterValue = bt.CurrentBitValue
                    });
                }
                else if ((CurrentChip.ChipName == "MP8850" || CurrentChip.ChipName == "MPM2896-20" || CurrentChip.ChipName == "MPM3898-50") && bt.BitName == "PAGE_SELECTION")
                {
                    //if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                main_window.UIEnabled = false;
                                BusyIndicator.LoadingIndicator.LoadingUIRun1("SwitchPage");
                                Stopwatch sw = new Stopwatch();
                                sw.Start();
                                while (!DeviceObject.Devices.PMBus.SetStatus(true, ""))
                                {
                                    if (sw.ElapsedMilliseconds >= 30000)   //30s
                                    {
                                        throw new TimeoutException();
                                    }
                                    System.Threading.Thread.Sleep(500);
                                }

                                _communicationObj.SelectionChangedMethod(LogData, bt, CurrentChip);
                                bt.CurrentBitValue = bt.SourceBitValue = bt.CurrentBitValue;

                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    PageSelect();
                                }));
                                BusyIndicator.LoadingIndicator.LoadingUIStop();
                                main_window.UIEnabled = true;
                            }
                            catch (Exception ex)
                            {
                                BusyIndicator.LoadingIndicator.LoadingUIStop();
                                bt.CurrentBitValue = bt.SourceBitValue;
                                string msg = ex.Message;
                                if (ex.Message != "Cancelled")
                                {
                                    if (ex.GetType() == typeof(TimeoutException))
                                    {
                                        msg = "Time out, please try again.";
                                    }
                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        var ui = new WinError(msg);
                                        ui.ShowDialog();
                                    }));
                                }
                                    
                                main_window.UIEnabled = true;
                            }
                            finally
                            {
                                DeviceObject.Devices.PMBus.SetStatus(false, "");
                            }
                            
                        });
                    }
                    //else
                    //{
                    //    main_window.UIEnabled = false;
                    //    var ui = new WinDeviceDisconnected();
                    //    ui.ShowDialog();
                    //    main_window.UIEnabled = true;
                    //    bt.CurrentBitValue = bt.SourceBitValue;
                    //}
                }
                else if(CurrentChip.ChipName == "MPQ78232FS" && bt.BitName == "Dummy Clocks")
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            main_window.UIEnabled = false;
                            BusyIndicator.LoadingIndicator.LoadingUIRun1("Switch Dummy Clock");
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (!DeviceObject.Devices.PMBus.SetStatus(true, ""))
                            {
                                if (sw.ElapsedMilliseconds >= 30000)   //30s
                                {
                                    throw new TimeoutException();
                                }
                                System.Threading.Thread.Sleep(500);
                            }

                            _communicationObj.SelectionChangedMethod(LogData, bt, CurrentChip);

                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            main_window.UIEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            BusyIndicator.LoadingIndicator.LoadingUIStop();
                            main_window.UIEnabled = true;
                        }
                        finally
                        {
                            DeviceObject.Devices.PMBus.SetStatus(false, "");
                        }

                    });
                }
                else
                {
                    return;
                }
            }
        }

        //public ICommand AdanceSettingSelectionChangedCommand => new RelayCommand<string>(AdanceSettingSelectionChangedMethod);
        //public void AdanceSettingSelectionChangedMethod(string ItemName)
        //{
        //    if (ItemName == "Internal")
        //    {
        //        MessageBox.Show("Please input the password!!");
        //    }
        //}


        public ICommand DiagramEnlargeCommand => new RelayCommand<BitmapImage>(DiagramEnlargeCommandMethod);

        public void DiagramEnlargeCommandMethod(BitmapImage sender)
        {
            Image_Viewer iv = new Image_Viewer(sender);
            iv.ShowDialog();
        }

        public ICommand ScollLeftCommand => new RelayCommand<object>(ScollLeftMethod);
        public ICommand ScollRightCommand => new RelayCommand<object>(ScollRightMethod);
        public void ScollLeftMethod(object obj)
        {
            ListBox lb = obj as ListBox;
            ScrollViewer sv = GetScrollViewer(lb) as ScrollViewer;
            sv.LineLeft();
        }
        public void ScollRightMethod(object obj)
        {
            ListBox lb = obj as ListBox;
            ScrollViewer sv = GetScrollViewer(lb) as ScrollViewer;
            sv.LineRight();
        }
        public static DependencyObject GetScrollViewer(DependencyObject o)
        {
            var sv = o as ScrollViewer;
            if (sv != null)
            { return o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }



        public ICommand showRegisterMapDetailCommand => new RelayCommand(showRegisterMapDetailMethod, null);
        private void showRegisterMapDetailMethod()
        {
            try
            {
                main_window.UIEnabled = false;
                var win = new WinDetailedRegisterMap(CurrentChip, LogData);
                win.Width = main_window.ActualWidth - 300;
                try
                {
                    win.ShowDialog();
                }
                catch (Exception ex)
                {
                    var ui = new WinError(ex.Message) { Owner = win };
                    ui.ShowDialog();
                    win.Close();
                }
            }
            catch (Exception ex)
            {
                var ui = new WinError(ex.Message);
                ui.ShowDialog();
            }
            finally
            {
                //DeviceObject.Devices.PMBus.SetStatus(false, "showRegisterMapDetailMethod");
                if ((CurrentChip.IsAddrAvalible.HasValue && (bool)CurrentChip.IsAddrAvalible) || CurrentChip.ChipAddr.Contains("v"))
                {
                    CommunicationMethod("ReadRAM");
                }
                main_window.UIEnabled = true;

            }
        }

        public RelayCommand<object> MonitoringFloatCommand => new RelayCommand<object>(MonitoringFloatCommandExecute);
        private void MonitoringFloatCommandExecute(object obj)
        {
            var scrollViewer = obj as ScrollViewer;
            var mainWindow = FindVisualParent<MainWindow>(scrollViewer);
            var stackPanel = FindVisualChildren<StackPanel>(scrollViewer).First();
            var stackPanelActualWdith = stackPanel.ActualWidth;
            var stackPanelActualHeight = stackPanel.ActualHeight;

            MonitoringVisibility = Visibility.Collapsed;

            var controlManager = new ControlManager(scrollViewer);
            controlManager.DetachFromParent();
            var window = new FloatingWindow()
            {
                DataContext = this,
                FloatingWindowContent = scrollViewer,
                WindowTitle = "Monitoring",
                ContentWidth = stackPanelActualWdith,
                ContentHeight = stackPanelActualHeight,
            };
            window.Closed += (s, e) =>
            {
                controlManager.ReattachToParent();
                MonitoringVisibility = Visibility.Visible;
                mainWindow.Activate();
            };
            window.Show();
            window.Activate();
        }

        #endregion


        #region Method
        //public void loadWaveforms()
        //{
        //    if (CurrentChip.Monitor == null || CurrentChip.Monitor.LineGraphs.Count == 0)
        //        return;

        //    var tempRunflag = CurrentChip.Monitor.isRunning;
        //    CurrentChip.Monitor.isRunning = false;

        //    if (CurrentChip.Monitor.LineGraphs.Count == 0)
        //    {
        //        return;
        //    }
        //    List<MPS.Monitor.Charts> existcharts = new List<MPS.Monitor.Charts>();
        //    foreach (var item in CurrentChip.Monitor.LineGraphs)
        //    {
        //        existcharts.Add(item);
        //    }
        //    CurrentChip.Monitor.LineGraphs.Clear();
        //    foreach (var item in existcharts)
        //    {
        //        SolidColorBrush font_color = (SolidColorBrush)new BrushConverter().ConvertFrom("#0a3c7f");
        //        item.MyAxisYCollection = new AxesCollection
        //        {
        //            new Axis { Title = "Voltage", Separator = new LiveCharts.Wpf.Separator() { Step = 1 }, LabelFormatter = value => (value + "V") },
        //            new Axis { Title = "Current", Foreground = Brushes.ForestGreen, Separator = new LiveCharts.Wpf.Separator() { Step = 0.5 }, LabelFormatter = value => (value + "A") },
        //            //new Axis { Title = "Temperatrue", Foreground = Brushes.IndianRed, Position = AxisPosition.RightTop, Separator = new LiveCharts.Wpf.Separator() { Step = 10 }, LabelFormatter = value => (value + "°C") },
        //        };
        //        if (item.MySeries.Any(o => o.Title.Contains("Temperature")))
        //        {
        //            item.MyAxisYCollection.Add(new Axis { Title = "Temperatrue", Foreground = Brushes.IndianRed, Position = AxisPosition.RightTop, Separator = new LiveCharts.Wpf.Separator() { Step = 10 }, LabelFormatter = value => (value + "°C") });
        //        }


        //        CurrentChip.Monitor.LineGraphs.Add(item);

        //    }
        //    foreach (var l in CurrentChip.Monitor.LineGraphs)
        //        l.IsVisble = false;

        //    CurrentChip.Monitor.LineGraphs[0].IsVisble = true;


        //    CurrentChip.Monitor.isRunning = tempRunflag;


        //}


        public void loadWaveforms()
        {
            if (CurrentChip.Monitor == null || CurrentChip.Monitor.LineGraphs.Count == 0)
                return;

            var tempRunflag = CurrentChip.Monitor.isRunning;
            CurrentChip.Monitor.isRunning = false;

            if (CurrentChip.Monitor.LineGraphs.Count == 0)
            {
                return;
            }
            List<MPS.Monitor.Charts> existcharts = new List<MPS.Monitor.Charts>();
            foreach (var item in CurrentChip.Monitor.LineGraphs)
            {
                existcharts.Add(item);
            }
            CurrentChip.Monitor.LineGraphs.Clear();
            var existAxisYcollection = new AxesCollection();
            foreach (var item in existcharts)
            {
                existAxisYcollection.Clear();
                foreach (var axisY in item.MyAxisYCollection)
                {
                    existAxisYcollection.Add(axisY);
                }
                //item.MyAxisYCollection = new AxesCollection();   //Cause exception: Specified element is already the logical child of another element. Disconnect it first
                item.MyAxisYCollection.Clear();
                foreach (var axisY in existAxisYcollection)
                {
                    var newAxisY = new Axis { Title = axisY.Title, Foreground = axisY.Foreground, Separator = axisY.Separator, LabelFormatter = axisY.LabelFormatter, Position = axisY.Position };
                    item.MyAxisYCollection.Add(newAxisY);
                }
                CurrentChip.Monitor.LineGraphs.Add(item);
            }

            foreach (var l in CurrentChip.Monitor.LineGraphs)
                l.IsVisble = false;

            CurrentChip.Monitor.LineGraphs[0].IsVisble = true;


            CurrentChip.Monitor.isRunning = tempRunflag;

          


        }

        public void loadBatteryLines()
        {
            if (CurrentChip.Monitor == null || CurrentChip.Monitor.LineAxisYCollection == null || CurrentChip.Monitor.LineAxisYCollection.Count == 0)
                return;

            var existAxisYcollection = new AxesCollection();
            foreach (var axisY in CurrentChip.Monitor.LineAxisYCollection)
            {
                existAxisYcollection.Add(axisY);
            }
            CurrentChip.Monitor.LineAxisYCollection.Clear();
            foreach (var axisY in existAxisYcollection)
            {
                var newAxisY = new Axis { Title = axisY.Title, Position = axisY.Position, ShowLabels = axisY.ShowLabels };
                CurrentChip.Monitor.LineAxisYCollection.Add(newAxisY);
                existAxisYcollection.Clear();
            }
        }

        public void loadJeitaLines()
        {
            if (CurrentChip.Monitor == null || CurrentChip.Monitor.MyLineSeriesCollection.Count == 0 || CurrentChip.Monitor.MyLineSeriesCollection == null)
                return;
            var existAxisYcollection = new AxesCollection();
            foreach (var LineAxisY in CurrentChip.Monitor.MyLineSeriesCollection)
            {
                existAxisYcollection.Clear();
                foreach (var axisY in LineAxisY.LineAxisYCollection)
                {
                    existAxisYcollection.Add(axisY);
                }
                LineAxisY.LineAxisYCollection.Clear();
                foreach (var axisY in existAxisYcollection)
                {
                    var newAxisY = new Axis { Title = axisY.Title, Position = axisY.Position, ShowLabels = axisY.ShowLabels };
                    LineAxisY.LineAxisYCollection.Add(newAxisY);
                }                
            }
        }

        public void ScanI2CMethod()
        {
            try
            {
                main_window?.SystemViewVM?.ScanAddressMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ScanI2CMethod: " + ex.Message);
            }
            //finally
            //{
            //    DeviceObject.Devices.PMBus.SetStatus(false);
            //}
        }     

        public void ClearBindingData()
        {
            try
            {
                lock (_lockObj)
                {
                    if (ProConfig != null)
                    {
                        foreach (var item in ProConfig.Chips)
                        {
                            if (item.Monitor == null)
                            {
                                continue;
                            }
                            item.Monitor.Dispose();
                        }
                        ProConfig.Chips.Clear();
                    }

                    CurrentChip = null;

                    if (BasicSetting != null)
                    {
                        BasicSetting.Clear();
                    }
                    if (AdvancedSetting != null)
                    {
                        AdvancedSetting.Clear();
                    }
                    if (CurrentRegisterMap != null)
                    {
                        CurrentRegisterMap.Clear();
                    }
                    if (_logDataDic != null)
                    {
                        _logDataDic.Clear();
                    }
                    if (_isOpenFileDic != null)
                    {
                        _isOpenFileDic.Clear();
                    }
                    if (BlackBoxRegisterMap != null)
                    {
                        BlackBoxRegisterMap.Clear();
                    }
                    if (BlackBoxViewRegisterMap != null)
                    {
                        BlackBoxViewRegisterMap.Clear();
                    }
                    if (RegisterRegionforBk != null)
                    {
                        RegisterRegionforBk.Clear();
                    }
                    //_partDictionary.Clear();
                    CurrentPart?.Dispose();
                    CurrentPart = null;
                    LogData = null;
                    CategorySelectedId = -1;
                    _communicationObj = null;
                    Description = string.Empty;
                    DescriptionTitle = string.Empty;
                    Features = string.Empty;
                    FeaturesTitle = string.Empty;
                    Diagram = null;
                    DiagramTitle = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in ClearBindingData: " + ex.Message);
            }
        }

        public void ChangeSelectedTab()
        {
            //switch (tcSelectedIndex)
            //{
            //    case -1:   //unselected yet
            //        if (CurrentChip.ChipName.Contains("MP8845") || CurrentChip.ChipName.Contains("MP8843") || CurrentChip.ChipName.Contains("MPQ8883") || 
            //            CurrentChip.ChipName.Contains("mEZDPD3603A") || CurrentChip.ChipName.Contains("MP8856") || CurrentChip.ChipName.Contains("MPQ7241"))
            //        {
            //            tcSelectedIndex = 2;
            //        }
            //        else if (CurrentChip.ChipName.Contains("MP5470") || CurrentChip.ChipName.Contains("MPQ4210") || CurrentChip.ChipName.Contains("MP28167-A") || 
            //            CurrentChip.ChipName.Contains("MPM54304") || CurrentChip.ChipName.Contains("MPQ8875A") || CurrentChip.ChipName.Contains("MPQ7928") || CurrentChip.ChipName.Contains("MPQ8873"))
            //        {
            //            tcSelectedIndex = 0;
            //        }
            //        break;
            //    case 0:   //Fault
            //        if (CurrentChip.ChipName.Contains("MP8845") || CurrentChip.ChipName.Contains("MP8843") || CurrentChip.ChipName.Contains("MPQ8883") || 
            //            CurrentChip.ChipName.Contains("mEZDPD3603A"))
            //        {
            //            tcSelectedIndex = 2;
            //        }
            //        break;
            //    case 1:   //Waveform
            //        if (CurrentChip.ChipName.Contains("MP5470") || CurrentChip.ChipName.Contains("MPQ4210") || CurrentChip.ChipName.Contains("MP28167-A") || CurrentChip.ChipName.Contains("MPM54304")
            //            || CurrentChip.ChipName.Contains("MPQ8875A") || CurrentChip.ChipName.Contains("MPQ7928") || CurrentChip.ChipName.Contains("MPQ7241") || CurrentChip.ChipName.Contains("MPQ8873"))
            //        {
            //            tcSelectedIndex = 0;
            //        }
            //        if (CurrentChip.ChipName.Contains("MP8845") || CurrentChip.ChipName.Contains("MP8843") || CurrentChip.ChipName.Contains("MPQ8883") || 
            //            CurrentChip.ChipName.Contains("mEZDPD3603A") || CurrentChip.ChipName.Contains("MP4255") || CurrentChip.ChipName.Contains("MPQ8856"))
            //        {
            //            tcSelectedIndex = 2;
            //        }
            //        break;
            //    default:
            //        break;
            //}

            if(CurrentChip == null)
            {
                tcSelectedIndex = 0;
                return;
            }

            if (CurrentChip.ChipName.Contains("MPQ8883") || CurrentChip.ChipName.Contains("mEZDPD3603A") || CurrentChip.ChipName.Contains("mEZDPD3603AS") ||
                CurrentChip.ChipName.Contains("MP8843") || CurrentChip.ChipName.Contains("MP8845") || CurrentChip.ChipName.Contains("MP5479") ||
                CurrentChip.ChipName.Contains("MP5514") || CurrentChip.ChipName.Contains("MP5424") || CurrentChip.ChipName.Contains("MP3439") ||
                CurrentChip.ChipName.Contains("MPM3593") || CurrentChip.ChipName.Contains("MP5515") || CurrentChip.ChipName.Contains("MP2658") ||
                CurrentChip.ChipName.Contains("MP28165"))
            {
                tcSelectedIndex = 4;
                SelectedRegsiterIndex = 0;
            } 
            else if (CurrentChip.ChipName.Contains("MPQ8896")||CurrentChip.ChipName.Contains("MPQ76300FS")) 
            {
                tcSelectedIndex = 1;
            }
            else
            {
                tcSelectedIndex = 0;
            }
   
        }

        //public void OpenConfigMethod()
        //{
        //    var openDiag = new OpenFileDialog
        //    {
        //        Filter = "(MPS Spec File)|*.spec|XML|*.xml",
        //        Title = "Load MPS Spec Configuration file",
        //    };
        //    var showDialog = openDiag.ShowDialog();
        //    var result = showDialog != null && (bool)showDialog;
        //    if (!result) return;
        //    var path = openDiag.FileName;
        //    if (!File.Exists(path)) return;
        //    main_window.Project_Directory = openDiag.FileName;
        //    try
        //    {
        //        ClearBindingData();
        //        lock (_lockObj)
        //        {
        //            var config = MPS.Common.Parser.SchemaParser.XmlToModel(path);

        //            #region check specversion
        //            ProjectConfig defaultConfig = new ProjectConfig();
        //            string CONST_CONFIG_FOLDER = "DataBase";
        //            CONST_CONFIG_FOLDER = string.Format(@"{0}\{1}", CONST_CONFIG_FOLDER, config.Product);
        //            string defaultPath = System.IO.Path.Combine(CONST_CONFIG_FOLDER, "DefaultDatasheet.Spec");
        //            if (!System.IO.File.Exists(defaultPath))
        //            {
        //                CONST_CONFIG_FOLDER = string.Format(@"{0}\{1}", CONST_CONFIG_FOLDER, config.BaseProduct);
        //                defaultPath = System.IO.Path.Combine(CONST_CONFIG_FOLDER, "DefaultDatasheet.Spec");
        //                if (System.IO.File.Exists(defaultPath))
        //                {
        //                    defaultConfig = MPS.Common.Parser.SchemaParser.XmlToModel(defaultPath, true);
        //                }
        //            }
        //            else
        //            {
        //                defaultConfig = MPS.Common.Parser.SchemaParser.XmlToModel(defaultPath, true);
        //            }
        //            if (!string.Equals(defaultConfig.SpecVersion, config.SpecVersion))
        //            {
        //                var dResult = MessageBox.Show("The spec is not latest version,Do you still want to Load?", "The file changed", MessageBoxButton.YesNo);
        //                if (dResult == MessageBoxResult.No)
        //                    return;
        //            }
        //            defaultConfig = null;
        //            #endregion

        //            foreach (var chip in config.Chips)
        //            {
        //                if (ProConfig.Chips.Count == 0)
        //                {
        //                    ProConfig.Product = config.Product;
        //                    ProConfig.BaseProduct = config.BaseProduct;
        //                    ProConfig.CreatedDate = config.CreatedDate;
        //                    ProConfig.CreatedTool = config.CreatedTool;
        //                    ProConfig.CreatedToolVersion = config.CreatedToolVersion;
        //                    ProConfig.LastModifiedDate = config.LastModifiedDate;
        //                    ProConfig.LastModifiedTool = config.LastModifiedTool;
        //                    ProConfig.LastModifiedToolVersion = config.LastModifiedToolVersion;
        //                    ProConfig.SpecVersion = config.SpecVersion;
        //                }

        //                chip.Monitor = MonitorManager.CreateInstance(chip, false);

        //                ProConfig.Chips.Add(chip);

        //                var templogdic = new Dictionary<string, ObservableCollection<LogModel>>();

        //                _logDataDic.Add(chip.ChipAddr, new ObservableCollection<LogModel>());

        //                _isOpenFileDic.Add(chip.ChipAddr, true);

        //            }
        //            ProConfig.Chips.LastOrDefault().IsSelected = true;
        //        }

        //        if (CurrentChip == null)
        //        {
        //            ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());
        //        }
        //        if (ProConfig.Chips.Count == 0)
        //            main_window.ChangeProjectView(1);
        //        else
        //            main_window.ChangeProjectView(2);

        //        main_window.Project_Title = openDiag.SafeFileName.Split('.')[0];

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occurs in OpenConfigMethod: " + ex.Message);
        //    }
        //}

        public void OpenConfigMethod()
        {
            var openDiag = new OpenFileDialog
            {
                Filter = "(MPS Spec File)|*.spec|XML|*.xml",
                Title = "Load MPS Spec Configuration file",
            };
            var showDialog = openDiag.ShowDialog();
            var result = showDialog != null && (bool)showDialog;
            if (!result) return;
            var path = openDiag.FileName;
            if (!File.Exists(path)) return;
            main_window.Project_Directory = openDiag.FileName;
            try
            {
                var config = MPS.Common.Parser.SchemaParser.XmlToModel(path);

                foreach (var chip in config.Chips)
                {
                    if (chip.ChipName.Contains('_'))
                    {
                        chip.ChipName = chip.ChipName.Replace('_', '-');
                    }
                }

                main_window.ImportedPartFilter(UserLicense, config);
                if (config.Chips.Count == 0)
                    return;

                foreach (var chip in config.Chips)
                {
                    ReadOnlyRegisterParser.ParseReadRegister(chip, UserLicense.IsMPSAE);

                    BitValueParser.ExplainBitValue(chip, UserLicense.IsMPSAE);
                }

                if (config.Chips.Count == 0)
                {
                    main_window.ChangeProjectView(1);
                    return;
                }   

                main_window.ChangeProjectView(2);
                ClearBindingData();

                #region prepare

                ProConfig.Product = config.Product;
                ProConfig.BaseProduct = config.BaseProduct;
                ProConfig.CreatedDate = config.CreatedDate;
                ProConfig.CreatedTool = config.CreatedTool;
                ProConfig.CreatedToolVersion = config.CreatedToolVersion;
                ProConfig.LastModifiedDate = config.LastModifiedDate;
                ProConfig.LastModifiedTool = config.LastModifiedTool;
                ProConfig.LastModifiedToolVersion = config.LastModifiedToolVersion;
                ProConfig.SpecVersion = config.SpecVersion;
                main_window.ChangeProjectView(2);

                foreach (var cp in config.Chips)
                {
                    if (cp.ChipAddr.Contains("v"))
                    {
                        var virtualDevices = ProConfig.Chips.Where(chip => chip.ChipAddr.Contains('v'));
                        string virtualAddr = string.Empty;
                        if (virtualDevices.Count() == 0)
                        {
                            virtualAddr = "v1";
                        }
                        else
                        {
                            virtualAddr = "v" + (virtualDevices.Max(chip => Convert.ToInt16(chip.ChipAddr.Substring(1, chip.ChipAddr.Length - 1))) + 1).ToString();
                        }
                        cp.ChipAddr = virtualAddr;

                    }
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(cp.ChipAddr))
                    //    {
                    //        if (cp.ChipAddr == "SPI")
                    //            cp.ChipAddr = "-";
                    //    }

                    //}

                    //Create Monitor
                    cp.Monitor = MonitorManager.CreateInstance(cp, false,UserLicense.IsMPSAE);
                    if (cp.Monitor != null)
                    {
                       
                        if (cp.Monitor.LineGraphs.Count == 0)
                        {
                            cp.Monitor.LoadMonitorValue();
                        }
                        cp.Monitor.isRunning = true;
                       
                    }

                    while (ProConfig.Chips.Any(chip => chip.ChipID == cp.ChipID))
                    {
                        cp.ChipID = cp.ChipID + "*";
                    }
                    ProConfig.Chips.Add(cp);

                    _logDataDic.Add(cp.ChipID, new ObservableCollection<LogModel>());
                    _isOpenFileDic.Add(cp.ChipID, true);
                }

                ChangeChipSelectionMethod(ProConfig.Chips.LastOrDefault());

                #endregion

                main_window.Project_Title = openDiag.SafeFileName.Split('.')[0];

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurs in OpenConfigMethod: " + ex.Message);
            }
        }

        

        public void CopyLog()
        {
            if (LogData == null || LogData.Count == 0)
                return;

            Clipboard.Clear();

            string logs = string.Format("{0}\t\t{1}\t{2}\t{3}\r\n", "Date and Time", "Register", "Address(Hex)", "Value");
            foreach (var log in LogData)
            {
                logs += string.Format("{0}\t{1}\t{2}\t{3}\r\n", log.DateTime, log.RegisterName, log.RegisterAddress, log.RegisterValue);
            }
            Clipboard.SetText(logs);

            //var saveDiag = new SaveFileDialog
            //{
            //    Filter = "(MPS Log File)|*.txt",
            //    Title = "Save as"
            //};

            //var showDialog = saveDiag.ShowDialog();
            //var result = showDialog != null && (bool)showDialog;

            //if (!result) return;
            //if (File.Exists(saveDiag.FileName))
            //{
            //    File.Delete(saveDiag.FileName);
            //}

            //using (StreamWriter sw = File.AppendText(saveDiag.FileName))
            //{
            //    sw.WriteLine("Part Number: " + CurrentChip.ChipName);
            //    sw.WriteLine("Chip Address: 0x" + CurrentChip.ChipAddr);
            //    sw.WriteLine("Current Channel: " + CurrentChip.Pages.FirstOrDefault(o => o.IsSelected == true).PageName);
            //    sw.WriteLine();
            //    sw.WriteLine(string.Format("{0,-20}\t{1,-30}\t{2,-10}\t{3,-10}", "Date and Time", "Register", "Address(HEX)", "Value(HEX)"));
            //    foreach (var log in LogData)
            //    {
            //        sw.WriteLine(string.Format("{0,-20}\t{1,-30}\t{2,-10}\t{3,-10}", log.DateTime, log.RegisterName, log.RegisterAddress, log.RegisterValue));
            //    }
            //}
        }

        public T FindVisualParent<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(depObj);


                if (parent != null && parent.GetType() == typeof(T))
                {
                    return (T)parent;
                }
                else
                {
                    return FindVisualParent<T>(parent);
                }
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child.GetType() == typeof(T))
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        #endregion
    }



}
