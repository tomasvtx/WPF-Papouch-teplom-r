using Logger;
using NET_Helper;
using Papouch_teploměr.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Logger.Enums;
using static Oracle_SQL.Tasks.Oracle_Command_User;

namespace Papouch_teploměr.Tasks
{
    internal class SQL
    {

        public static async Task ExecuteSQL(Chart item, bool Repeat)
        {
            try
            {
                MyPerformanceMonitor.ComputerInfoHelper computerInfoHelper = new MyPerformanceMonitor.ComputerInfoHelper();

                var Data = new List<UPDATE>()
                        {
                              new UPDATE { Column_Name = "TEPLOTA", Value_Float = Repeat ? item.Temperature : App.viewModel.Temperature, SQL_Type = Oracle_SQL.MODEL.SQL_Type.Float },
                               new UPDATE { Column_Name = "LOCATION", Value = Repeat ? item.Location : App.viewModel.Location, SQL_Type = Oracle_SQL.MODEL.SQL_Type.String},
                                  new UPDATE { Column_Name = "PCMODEL", Value = computerInfoHelper.GetPCModel(), SQL_Type = Oracle_SQL.MODEL.SQL_Type.String},
                                    new UPDATE { Column_Name = "CPU", Value = computerInfoHelper.GetPCModel(), SQL_Type = Oracle_SQL.MODEL.SQL_Type.String},
                                      new UPDATE { Column_Name = "USERNAME", Value = computerInfoHelper.GetUsername(), SQL_Type = Oracle_SQL.MODEL.SQL_Type.String},
                                   new UPDATE { Column_Name = "DATUM",Value_Date = item?.Datum ?? DateTime.MinValue, Value = "SYSDATE", SQL_Type = Oracle_SQL.MODEL.SQL_Type.DateTime, IsInternal_ORA_func = !Repeat },
                                     new UPDATE { Column_Name = "DATESOURCE",Value = Repeat ? computerInfoHelper.GetPCModel() : "SERVER_DB", SQL_Type = Oracle_SQL.MODEL.SQL_Type.String },
                          };
                await await App.viewModel.LogManager_ViewModel.Logger_Title($"Ukládám teplotu\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(DB_ProcessState.INIT_DB));

                await await VložitDataAsync(App.MAIN_DB_CONF, "TEPLOTA", Data, App.CancellationTokenSource.Token).ContinueWith(async task =>
                {
                    if (!Repeat)
                    {
                        await await App.main.Dispatcher.InvokeAsync(async () => App.viewModel.Temperatures.Add(new Chart { Location = App.viewModel.Location, Datum = DateTime.Now, RowAffected = (await task).Dokončeno, Temperature = App.viewModel.Temperature }), App.ProductionPriority);
                    }

                    item.RowAffected = (await task).Dokončeno;
                    if ((await task).Dokončeno)
                    {
                        await await App.viewModel.LogManager_ViewModel.Logger_Title($"Změřena teplota uložena\n{(await task).SQLDotaz}\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(DB_ProcessState.DB_UPDATE_DATA));
                        await SQL_Completed();
                    }
                    else
                    {
                        await await App.viewModel.LogManager_ViewModel.Logger_Title($"Změřenou teplotu nelze uložit\n{(await task).SQLDotaz}\n{(await task).Výjimka}", MethodBase.GetCurrentMethod(), ProcessStateModel.GET(DB_ProcessState.DB_ERROR));
                    }
                });
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }

        public static async Task SQL_Completed()
        {
            try
            {        
                List<Task> tasks = new List<Task>();
                foreach (var _item in App.viewModel.Temperatures.Where(ee => ee.RowAffected == false).ToList())
                {
                    tasks.Add(ExecuteSQL(_item, true));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }
    }
}
