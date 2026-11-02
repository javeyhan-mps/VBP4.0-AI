//using MPS.Common.AutoHelper;
using MPS.Common.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VirtualBenchPro_v4.Views.DeviceViews;
using MPS.AutoUpdate.Interact;

namespace VirtualBenchPro_v4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //System.Threading.Mutex mutex;
        public static readonly Mutex muTex = new Mutex(false, ConstantModel.APPNAME);
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var noRun = !muTex.WaitOne(0, false);
                if (noRun)
                {
                    MessageBox.Show("Error!This program is running.", "Application", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Environment.Exit(0);
                }

              //  CheckUpdateStatus.StartApplication(ConstantModel.APPNAME); // Check if Admin needed, restart and pop up Run as Window. If not, skip.
                if (CheckUpdateStatus.CheckUpdate())
                {
                    var ver = string.IsNullOrEmpty(CheckUpdateStatus.Version) ? "" : string.Format(" ({0})", CheckUpdateStatus.Version);
                    //if (MessageBox.Show(string.Format("A new version{0} is available.  Would you like to install it now?", ver), "Update to new version?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    //{
                    //    CheckUpdateStatus.DoUpdateProcess(ConstantModel.UPDATEAPPNAME, ConstantModel.APPNAME);
                    //}

                    var backupShutdownMode = ShutdownMode;
                    // Temp change ShutdownMode to prevent App exit when WinUpdate closed.
                    ShutdownMode = ShutdownMode.OnExplicitShutdown;
                    var upWin = new WinUpdate(ver);
                    upWin.ShowDialog();
                    // Restore to original ShutdownMode;
                    ShutdownMode = backupShutdownMode;
                }
                //CheckUpdateStatus.StartApplication(ConstantModel.APPNAME);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);

            }

        }

    }
}
