using MPS.Common;
using MPS.Common.Log;
using MPS.Common.Model;
using MPS.Common.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBenchPro_v4.Classes
{
    [Serializable]
    public class Settings : ObservableObject
    {
        private readonly string settingsXmlPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Settings.xml");

        private string _updateServerLocation;
        public string UpdateServerLocation
        {
            get { return _updateServerLocation; }
            set
            {
                _updateServerLocation = value;
                RaisePropertyChanged(nameof(UpdateServerLocation));
            }
        }

        public Settings()
        {
            // Do not delete. SerializeHelper.XMLSerialize need this Constructor.
        }

        public Settings(bool isLoadSettingsFile)
        {
            if (isLoadSettingsFile)
            {
                ParseLocalSettingsXmlFile();
            }
        }

        public void ParseLocalSettingsXmlFile()
        {
            try
            {
                if (File.Exists(settingsXmlPath))
                {
                    var decryptedText = AesOperation.DecryptFile(settingsXmlPath);
                    File.WriteAllText(settingsXmlPath + "dec", decryptedText, Encoding.UTF8);
                    var settings = SerializeHelper.XMLDeserialize<Settings>(settingsXmlPath + "dec");
                    UpdateSettings(settings);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(settingsXmlPath);
                LogHelper.WriteLog(ex);
            }
            finally
            {

                if (File.Exists(settingsXmlPath + "dec"))
                {
                    File.Delete(settingsXmlPath + "dec");
                }
            }
        }

        public void UpdateSettings(Settings settings)
        {
            UpdateServerLocation = settings.UpdateServerLocation;
        }

        public void SaveSettings()
        {
            try
            {
                // Update the Settings.xml
                SerializeHelper.XMLSerialize(this, settingsXmlPath);
                // Encrypt
                string encryptedText = AesOperation.EncryptFile(settingsXmlPath);
                File.WriteAllText(settingsXmlPath, encryptedText, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        public static string GetUpdateServerLocationDomain(string updateServerLocation)
        {
            switch (updateServerLocation)
            {
                case "US":
                    return "com";
                case "CN":
                    return "cn";
                default:
                    return null;
            }
        }
    }
}
