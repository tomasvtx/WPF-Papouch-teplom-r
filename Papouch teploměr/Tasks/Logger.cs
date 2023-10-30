using Logger;
using NET_Helper;
using Papouch_teploměr.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Logger.Enums;

namespace Papouch_teploměr.Tasks
{
    public static class Logger
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// logger and save log
        /// </summary>
        /// <param name="title"></param>
        /// <param name="processState"></param>
        /// <param name="state"></param>
        /// <param name="mainViewModel"></param>
        /// <returns></returns>
        public async static Task<Task> Logger_Title(this LogManager_ViewModel logManager_ViewModel, string title, MethodBase methodBase, StavProcesuModel processStateView, bool ResetTimer = false)
        {
            try
            {
                if (App.main == null)
                {
                    return Task.CompletedTask;
                }

                //await App.MyDispatcher.InvokeAsync(() =>App.ModelView.MainViewModel.Newcsrool?.ScrollIntoView(App.ModelView.MainViewModel.ErrList.LastOrDefault()));
                var _Logger = await App.main.Dispatcher.InvokeAsync(() => LOG_Tasks.Logger_Title(title, processStateView, typeof(App), App._AppConfigure, methodBase, App.NET_Version, ResetTimer), App.ProductionPriority);

                await App.main.Dispatcher.InvokeAsync(() =>
                {
                    _Logger.Log_for_MVVM.Width = 298;
                    logManager_ViewModel.Title = _Logger.Title;
                    logManager_ViewModel.State = _Logger.State;

                    ///Add to screen by MVVM
                    logManager_ViewModel.ErrList.Add(new Logger_ItemViewModel(_Logger.Log_for_MVVM));

                    while (logManager_ViewModel.ErrList.Count > 50)
                    {
                        logManager_ViewModel.ErrList.Remove(logManager_ViewModel.ErrList.FirstOrDefault());
                    }


                    ///scroolDown logger
                    logManager_ViewModel.New_Scroll?.ScrollIntoView(logManager_ViewModel?.ErrList?.LastOrDefault());

                }, App.ProductionPriority);

                stopwatch.Restart();
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }

            return Task.CompletedTask;
        } 
    }
}
