using Logger;
using NET_Helper;
using Papouch_teploměr.ViewModel;
using PapouchSensor;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Logger.Enums;

namespace Papouch_teploměr.Tasks
{
    internal class Sensor
    {
        public static async Task Execute() => await Task.Run(async () =>
                                                        {
                                                        Repeat:;
                                                            try
                                                            {
                                                                App.viewModel.Ports = SerialPort.GetPortNames();
                                                                App.viewModel.Port = App.viewModel?.Ports?.FirstOrDefault(ww => ww == App._AppConfigure?.SerialPort?.FirstOrDefault(w=>w.Description =="MAIN")?.PortName) ?? "COM1";

                                                                await await App.viewModel.LogManager_ViewModel.Logger_Title($"Čekám na data ze senzoru\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(StavSenzoru.InitSensor));
                                                                PapouchSensor.PapouchSensorTM papouchSensorTM = new PapouchSensor.PapouchSensorTM(App.viewModel.Port);

                                                                PapouchSensor.PapouchSensorTM.TempOutput Sensor = papouchSensorTM.ReadTemperature(ref App.SerialPort);
                                                                await (Sensor.Error == string.Empty ? SensorOK(Sensor) : SensorNG(Sensor));
                                                                await SQL.ExecuteSQL(new Chart(), false);
                                                                await await App.viewModel.LogManager_ViewModel.Logger_Title($"HOTOVO\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(ProcessState.Done));
                                                               // await WAIT();
                                                            }
                                                            catch (Exception dd)
                                                            {
                                                                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
                                                            }
                                                            goto Repeat;
                                                        });

        private static async Task SensorOK(PapouchSensorTM.TempOutput Sensor)
        {
            try
            {
                App.viewModel.Temperature = Sensor.Temperature;
                await await App.viewModel.LogManager_ViewModel.Logger_Title($"Změřena teplota\n{App.viewModel.Temperature}\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(Sensor_ProcessState.SENSOR_READ));
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }
        private static async Task SensorNG(PapouchSensorTM.TempOutput Sensor)
        {
            try
            {
                App.viewModel.Temperature = float.NaN;
                await App.viewModel.LogManager_ViewModel.Logger_Title($"Chyba při měření teploty\n{Sensor.Error}\n", MethodBase.GetCurrentMethod(), StavProcesuModel.Ziskat(Sensor_ProcessState.SENSOR_ERROR));
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }

        private static async Task WAIT()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(App._AppConfigure?.SerialPort?.FirstOrDefault(w => w.Description == "MAIN")?.BCS_DELAY ?? 0))
                {
                    await Task.Delay(1);

                    if (App.CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }
    }
}
