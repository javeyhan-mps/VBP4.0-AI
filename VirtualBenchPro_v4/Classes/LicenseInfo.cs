using MPS.Common;
using MPS.Common.Log;
using MPS.Common.Model;
using MPS.Common.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBenchPro_v4.Views.CustomElements;
using VirtualBenchPro_v4.Views.DeviceViews;

namespace VirtualBenchPro_v4.Classes
{
    [Serializable]
    public class LicenseInfo : ObservableObject
    {
        private readonly string LicenseXmlPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "license.xml");

        private Authorization _authorizationManager;

        public static bool IsXMLDeserialize { get; set; } = false;

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                RaisePropertyChanged("Code");
            }
        }

        private string _Key;

        public string Key
        {
            get { return _Key; }
            set
            {
                _Key = value;
                RaisePropertyChanged("Key");
            }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        private string _customer;

        public string Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                RaisePropertyChanged("Customer");
            }
        }

        private string _expDate;

        public string ExpDate
        {
            get { return _expDate; }
            set
            {
                _expDate = value;
                RaisePropertyChanged("ExpDate");
            }
        }

        private bool _hasPMBusTool;

        public bool HasPMBusTool
        {
            get { return _hasPMBusTool; }
            set 
            { 
                _hasPMBusTool = value;
                RaisePropertyChanged("HasPMBusTool");
            }
        }

        private bool _isMPSUser;

        public bool IsMPSUser
        {
            get { return _isMPSUser; }
            set
            {
                _isMPSUser = value;
                RaisePropertyChanged("IsMPSUser");
            }
        }

        private bool _isMPSAE;

        public bool IsMPSAE
        {
            get { return _isMPSAE; }
            set
            {
                _isMPSAE = value;
                RaisePropertyChanged("IsMPSAE");
            }
        }

        public ObservableCollection<string> _chipList_public;
        public ObservableCollection<string> _chipList_internal;

        public LicenseInfo()
        {
            _authorizationManager = new Authorization();
            if (IsXMLDeserialize)
            {
                _chipList_public = new ObservableCollection<string>();
            }
            else
            {
                _chipList_public = _authorizationManager.GetPublicProductList();
            }
            _chipList_internal = new ObservableCollection<string>();

        }


        public void Login(string licensecode = null, string licensekey = null)
        {
            string errmsg = string.Empty;

            if (!string.IsNullOrEmpty(licensecode) && !string.IsNullOrEmpty(licensekey))
            {
                Code = licensecode;
                Key = licensekey;
            }

            var lifo = _authorizationManager.Login(Code, Key, ref errmsg, out string connnectionStatusCode);

            if (connnectionStatusCode == "200" || connnectionStatusCode == "401" || connnectionStatusCode == "403")
            {
                UpdateLicenseInfo(lifo);
                UpdateLocalLicenseXmlFile();
            }
            else // Connection issue or Exception
            {
                Status = "OFFLINE";
                UpdateLocalLicenseXmlFile();
            }
        }

        public void Register()
        {
            var errorMsg = "";
            try
            {
                var win = new WinLicenseRegister(Code, Key);
                var res = win.ShowDialog();
                if (res == true)
                {
                    BusyIndicator.LoadingIndicator.LoadingUIRun1("Validate");

                    var licenseInfo = _authorizationManager.Login(WinLicenseRegister.code, WinLicenseRegister.key, ref errorMsg, out string connnectionStatusCode);

                    BusyIndicator.LoadingIndicator.LoadingUIStop();

                    if (licenseInfo != null && string.IsNullOrEmpty(errorMsg))
                    {
                        UpdateLicenseInfo(licenseInfo);
                        UpdateLocalLicenseXmlFile();
                    }
                }
                
            }
            catch (Exception ex)
            {
                BusyIndicator.LoadingIndicator.LoadingUIStop();
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                var ui = new WinError(errorMsg);
                ui.ShowDialog();
            }

        }



        public void UpdateLicenseInfo(LicenseInfo info)
        {
            Code = info.Code;
            Key = info.Key;
            Status = info.Status;
            Customer = info.Customer;
            ExpDate = info.ExpDate;
            HasPMBusTool = info.HasPMBusTool;
            IsMPSUser = info.IsMPSUser;
            IsMPSAE = info.IsMPSAE;
            _chipList_internal = info._chipList_internal;
            _chipList_public = info._chipList_public;
        }

        public void ClearLicenseInfo()
        {
            Code = null;
            Key = null;
            Status = null;
            Customer = null;
            ExpDate = null;
            HasPMBusTool = false;
            IsMPSUser = false;
            IsMPSAE = false;
            _chipList_internal.Clear();
            _chipList_public = _authorizationManager.GetPublicProductList();

            System.IO.File.Delete(LicenseXmlPath);
        }

        #region license parser
        public string ParseLocalLicenseXmlFile()
        {
            string errorMsg = string.Empty;
            try
            {
                if (File.Exists(LicenseXmlPath))
                {
                    var decryptedText = AesOperation.DecryptFile(LicenseXmlPath);
                    File.WriteAllText(LicenseXmlPath + "dec", decryptedText);
                    // Set IsXMLDeserialize to true to prevent adding duplicate chips in public list.
                    IsXMLDeserialize = true;
                    var info = SerializeHelper.XMLDeserialize<LicenseInfo>(LicenseXmlPath + "dec");
                    // Set IsXMLDeserialize back to false for normal use of LicenseInfo to add public list in constructor.
                    IsXMLDeserialize = false;
                    UpdateLicenseInfo(info);
                }
                else
                {
                    errorMsg = "XmlDoNotExist";
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(LicenseXmlPath);
                errorMsg = "Not able to retrieve your license info, please re-enter your license/secret.";
                LogHelper.WriteLog(ex);
            }
            finally
            {
                
                if (File.Exists(LicenseXmlPath + "dec"))
                {
                    File.Delete(LicenseXmlPath + "dec");
                }
              
            }
            return errorMsg;
        }

        public string UpdateLocalLicenseXmlFile()
        {
            string errorMsg = string.Empty;
            try
            {
                //Update the license.xml
                SerializeHelper.XMLSerialize(this, LicenseXmlPath);
                //Encrypt license info
                string encryptedText = AesOperation.EncryptFile(LicenseXmlPath);
                File.WriteAllText(LicenseXmlPath, encryptedText, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                errorMsg = "Updating license xml file encounters an issue.";
                LogHelper.WriteLog(ex);
            }
            return errorMsg;
        }
        #endregion


    }
}
