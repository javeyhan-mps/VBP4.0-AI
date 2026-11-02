using MPS.Common;
using MPS.Common.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBenchPro_v4.Classes;

namespace VirtualBenchPro_v4
{
    public class ConstantModel
    {
        public static ProjectSaveStatus ProjectSaveStatus = new ProjectSaveStatus(null);
        public static CipherSqliteHelper sqlite = new CipherSqliteHelper();
        public static Settings settings = new Settings(true);
        public static bool FileStatus;
        public static string mpsLicenseServer = "Staging";
        public static string mpsWebServer = "Staging";
        public static string WSproductionURL = "https://www.monolithicpower.com/";
        public static string WSStagingURL = "https://mps-staging.monolithicpower.com/";
        public const string UPDATEAPPNAME = "MPS.AutoUpdateExcue.exe";
        public const string APPNAME = "VirtualBenchPro_v4.exe";

        public const byte Min_USBI2C_FwRev = 0x03;     //Rev3
        public const byte Latest_USBI2C_FwRev = 0x1C;     //Rev28
        public const string Latest_USBI2C_FwFilePath = @".\Dongle_Firmware\MPS_PMBus_Dongle_Rev28.mps";

        public const byte Min_AUTOMOTIVE_FwRev = 0x70;  //Rev112
        public const byte Latest_AUTOMOTIVE_FwRev = 0x76;  //Rev118
        public const string Latest_AUTOMOTIVE_FwFilePath = @".\Dongle_Firmware\MPS_Automotive_Dongle_Rev118.mps";
    }
}
