using AppConfigure;
using Dialogs;
using Logger;
using NET_Helper;
using System;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Papouch_teploměr
{
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        public static ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
        public static MainWindow main;

        public static DaikinAppConfigure _AppConfigure;

        public static string MAIN_DB_CONF;
        public static string NET_Version;
        public static DispatcherPriority ProductionPriority = DispatcherPriority.Background;
        internal static CancellationTokenSource CancellationTokenSource;
        internal static SerialPort SerialPort;

        protected override async void OnStartup(StartupEventArgs e)
        {
            try { 
            ///Get and save NetFramework version
            NET_Version = NET_Helper.AssemblyDotNetVersion.GetAssemblyDotNetVersion(System.AppDomain.CurrentDomain.FriendlyName);

            App.CancellationTokenSource = new CancellationTokenSource();

            await XML_Settings(e);

            main = new MainWindow
            {
                DataContext = viewModel
            };
            await Dispatcher.NastavHlavniOkno(main, _AppConfigure);

                App.viewModel.Location = App._AppConfigure?.Line;

            await Tasks.Sensor.Execute();
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
            }
        }

        public static async Task XML_Settings(StartupEventArgs e)
        {
            try
            {
                if (!AppConfigure.ArgsCooperation.TryReadXML(out _AppConfigure, out string Error))
                {
                    await await Dispatcher.CurrentDispatcher.ShowXmlReadErrorAsync(Error, App.Current);
                    App.Current?.Shutdown();
                }

                ///Parsing arguments
                _AppConfigure.GetCooperationArguments(e.Args);

                MAIN_DB_CONF = _AppConfigure?.Database?.FirstOrDefault(ee => ee.Description == "MAIN")?.ConnectionString;
                if (MAIN_DB_CONF == null)
                {
                    AppConfigure.SystemInfoUtility systemInfoUtility = new SystemInfoUtility("");
                    await await Dispatcher.CurrentDispatcher.ShowErrorDialogAsync(Dialogs.ErrorDialog.Description.ConfigurationError, Logger.Msg.ArgsNotValid, $"{systemInfoUtility.GetOSInfo()}\n{typeof(App).Assembly.GetName().Name} {typeof(App).Assembly.GetName().Version}\nConnectionString is not set", App.Current, "App arguments", ErrorDialog.TypeMessage.Critical);
                }

            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
           }
        }
    }
}
