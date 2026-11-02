using MPS.Common.Log;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using VirtualBenchPro_v4.Views.CustomElements;
using ThreadState = System.Threading.ThreadState;

namespace VirtualBenchPro_v4.Classes
{
    
    internal class BusyIndicator : DependencyObject
    {
        public static readonly BusyIndicator LoadingIndicator;

        private Thread loadingThread;
        private Window loadingUI;
        static BusyIndicator()
        {
            LoadingIndicator = new BusyIndicator();
        }

        private BusyIndicator()
        {
        }


        //public void LoadingUIRun(ProcessEnum protype = ProcessEnum.Loading)
        public void LoadingUIRun()
        {
            if (loadingThread != null) return;
            try
            {
                loadingThread = new Thread(() =>
                {
                    //if (protype == ProcessEnum.Executing)
                    //{
                    //    loadingUI = new WinDataSaving();
                    //}
                    //else
                    //{
                        loadingUI = new WinLoading();
                    //}
                    loadingUI.Show();
                    loadingUI.Closed +=
                        (s, e) => loadingUI.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    Dispatcher.Run();

                })
                { IsBackground = true };

                loadingThread.SetApartmentState(ApartmentState.STA);
                loadingThread.Start();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        public void LoadingUIRun1(string param = null)
        {
            if (loadingThread != null) return;
            try
            {
                loadingThread = new Thread(() =>
                {
                    loadingUI = new WinExecuting(param);

                    loadingUI.ShowDialog();
                    loadingUI.Closed +=
                        (s, e) => loadingUI.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    Dispatcher.Run();

                })
                { IsBackground = true };

                loadingThread.SetApartmentState(ApartmentState.STA);
                loadingThread.Start();
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        public void BeginLoading(Action loadingMethod)
        {
            try
            {
                LoadingUIRun();

                loadingMethod();

                LoadingUIStop();
            }
            catch (Exception)
            {
                LoadingUIStop();
                throw;
            }

        }

        public void LoadingUIStop()
        {
            if (loadingThread == null || loadingUI == null) return;
            try
            {
                loadingUI.Dispatcher.Invoke(new Action(() => loadingUI.Close()));

                var stopWatch = new Stopwatch();

                stopWatch.Start();

                while (!loadingThread.ThreadState.ToString().Contains(ThreadState.Stopped.ToString()))
                {
                    if (stopWatch.ElapsedMilliseconds > 1000)
                    {
                        try
                        {
                            loadingThread.Abort();
                        }
                        catch (Exception ex)
                        {

                            LogHelper.WriteLog(ex);
                        }
                        break;
                    }

                    Thread.Sleep(100);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
            loadingThread = null;
        }
    }

    public enum ProcessEnum
    {
        Loading,
        Executing,
    }
}
