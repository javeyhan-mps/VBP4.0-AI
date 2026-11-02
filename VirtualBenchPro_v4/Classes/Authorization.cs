using MPS.Common.Log;
using MPS.PowerSuite.Shared.Licensing.Client;
using MPS.PowerSuite.Shared.Licensing.Client.Enum;
using MPS.PowerSuite.Shared.Licensing.Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace VirtualBenchPro_v4.Classes
{
    public class Authorization
    {
        private string PATH = @"DataBase\";

        private List<string> _publicProducts = new List<string>()
        {
         "MPQ8886", "MPM82504", "MPQ8883", "MPQ8645P", "MP8796B", "MPM3695-10", "MPM3695-25","mEZDPD1620A", "mEZDPD1620AS",
         "mEZDPD4506AS", "mEZDPD4506A", "MP5470", "MP8861", "MP8869", "MPQ8875A", "MPM54304", "MP8854", "MPM3695-100","MP8843",
         "MP8845", "MP5479", "MPQ4210", "MPQ4230", "MP28167-A", "MPQ5031" ,"MP4245", "MPQ2026", "MPQ2022", "MPQ2024", "MPQ2026A", "MPQ79500FS",
         "MPQ79700FS", "MPQ8873", "MPQ7920", "MP3924", "MPQ5857","MP5424", "MPQ8847A","MP8833A", "MPQ7928","MPQ70240FS","MPQ7930","MPQ7795","MP5496",
            "MP5416","MP5515","MP8846","MP8847","MP8128","MP8859","MP8869W","MP8869S","MP8868","MPQ2977","MP2984","MP2980","MP8864","MP8865","MP8867",
            "MPQ4214","MPQ7228","MPQ7225","MP8891", "MP8892", "MPQ2967","MPQ2286","MP4255","MP4248","MPQ4242","MPQ4242B","MPM54524","MPQ4272","MPM3690-50D","MPQ2946",
            "MPM4730","MPQ2022A","MPQ2024A","MPM3593","MPM54313","MPM54322","MPQ8861","MPQ77240FS","MPQ4263","MP5475","MPM3695-20","MP5516","MP8870","MPM3698",
            "MP2658","MPQ4232", "MP5520","MP5491","MPQ5472","MPQ4201","MPQ4208","MP4201","MPM54532GPB","MPQ4241","MPQ7210","MP3921","MPQ3446","MP28167-B","MPM54522",
            "MPQ2283","MPQ2285","MPQ2287","MPQ2288","MPQ2284","MPQ70241FS","MPQ70242FS","MPQ70244FS","MPQ70245FS","MP3439","MP8856GQE","MP5446","MP5446A","MP4246","MPQ8880","MPQ8880A",
            "MP8850","MPQ5856","MPQ5856H","MPQ5858","MP2764","MPQ70160FS","MPQ74201FS","MPQ3445","MP8853","MPQ70331FS","MPQ4232A","MPQ5864","MPQ5862","MPQ77220FS"
        };

        private LicenseClient _licenseClient;

        public Authorization()
        {
            //_licenseClient = new LicenseClient(ApplicationTypeEnum.VBPro, LicenseServerTypeEnum.Production);
            switch (ConstantModel.mpsLicenseServer)
            {
                case "Production":
                    _licenseClient = new LicenseClient(ApplicationTypeEnum.VBPro, LicenseServerTypeEnum.Production);
                    break;
                case "Staging":
                    _licenseClient = new LicenseClient(ApplicationTypeEnum.VBPro, LicenseServerTypeEnum.Staging);
                    break;
                default:
                    throw new ArgumentException("Invalid mpsLicenseServer value:" + ConstantModel.mpsLicenseServer);
            }
            _publicProducts.Sort();
        }

        public LicenseInfo Login(string licenseCode, string secretKey, ref string errorMsg, out string connectionStatusCode)
        {
            LicenseInfo licenseInfo = null;
            errorMsg = "";
            connectionStatusCode = null;

            try
            {
                if (_licenseClient != null)
                {
                    var licenseInfoFromServer = _licenseClient.LogIn(licenseCode, secretKey);

                    if (licenseInfoFromServer != null)
                    {
                        licenseInfo = ParseLicenseInfoFromServer(licenseInfoFromServer);
                        licenseInfo._chipList_public = GetPublicProductList();

                        switch (licenseInfoFromServer.ConnectionStatusCode)
                        {
                            case "200":
                                licenseInfo._chipList_internal = GetPersonalProductList(ref errorMsg);
                                break;
                            case "401":
                            case "403":
                                errorMsg = "Invalid License Code or Secret Key. Please try again.";
                                break;
                            default:
                                errorMsg = "VB Pro cannot communicate with the MPS web-service, Please check your Internet connection.";
                                break;
                        }

                        connectionStatusCode = licenseInfoFromServer.ConnectionStatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Not able to retrieve your license info, please re-enter your license/secret.";
                LogHelper.WriteLog(ex);
            }

            return licenseInfo;
        }

        public ObservableCollection<string> GetPublicProductList()
        {
            try
            {
                if (_licenseClient != null)
                {
                    var contentInfo = _licenseClient.GetPublicContents();

                    if (contentInfo != null)
                    {
                        switch (contentInfo.ConnectionStatusCode)
                        {
                            case "200":
                                var publicList = contentInfo.Contents["productList"];
                                var localParts = GetLocalParts();
                                var updateList = ProductListHelper.GetUpdateProductListNameAndSort(publicList, localParts);
                                return new ObservableCollection<string>(updateList);

                            default:
                                //since we will always return a copy of public product list, we don't care about the error here.
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }

            return new ObservableCollection<string>(_publicProducts);
        }

        public ObservableCollection<string> GetPersonalProductList(ref string errorMsg)
        {
            errorMsg = "";

            try
            {
                if (_licenseClient != null)
                {
                    var contentInfo = _licenseClient.GetPersonalContents();

                    if (contentInfo != null)
                    {
                        switch (contentInfo.ConnectionStatusCode)
                        {
                            case "200":
                                var publicList = contentInfo.Contents["productList"];
                                var localParts = GetLocalParts();
                                var updateList = ProductListHelper.GetUpdateProductListNameAndSort(publicList, localParts);
                                errorMsg = "";
                                return new ObservableCollection<string>(updateList);
                            case "401":
                            case "403":
                                errorMsg = "Invalid License Code or Secret Key. Please try again.";
                                break;
                            default:
                                errorMsg = "VB Pro cannot communicate with the MPS web-service, Please check your Internet connection.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
                //TODO: add trace statement
            }

            return new ObservableCollection<string>();
        }

        private LicenseInfo ParseLicenseInfoFromServer(LicenseInfoFromServer lifo)
        {
            if (lifo != null)
            {
                var licenseInfo = new LicenseInfo();
                licenseInfo.Key = lifo.KeyCodePair.SecretCode;
                licenseInfo.Code = lifo.KeyCodePair.ApiKey;
                licenseInfo.Customer = lifo.ContactName;
                licenseInfo.ExpDate = lifo.ExpirationDate ?? "--";

                if (lifo.MessageFromService == "Authenticated" && lifo.LicenseState == LicenseStatusEnum.Active
                    && !lifo.IsExpired)
                {
                    licenseInfo.Status = "ACTIVE";
                }
                else if (lifo.IsExpired)
                {
                    licenseInfo.Status = "EXPIRED";
                }
                else if (lifo.LicenseState == LicenseStatusEnum.InActive)
                {
                    licenseInfo.Status = "INACTIVE";
                }

                if (lifo.Features != null)
                {
                    licenseInfo.HasPMBusTool = lifo.Features.Contains("PMBus_Tool");
                    licenseInfo.IsMPSUser = lifo.Features.Contains("MPS_User");
                    licenseInfo.IsMPSAE = lifo.Features.Contains("AE_Empowered");
                }

                return licenseInfo;
            }

            return null;
        }

        private List<string> GetLocalParts()
        {
            DirectoryInfo theFolder = new DirectoryInfo(PATH);
            return theFolder.GetDirectories().Select(O => O.Name).ToList();
        }
    }
}
