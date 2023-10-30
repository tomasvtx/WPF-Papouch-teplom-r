using Logger;
using NET_Helper;
using Papouch_teploměr.Tasks;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Logger.Enums;

namespace Papouch_teploměr
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            logger.DataContext = App.viewModel.LogManager_ViewModel;
            logger.LoggerRegister_for_Scrool(ref App.viewModel.LogManager_ViewModel.New_Scroll);

            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                await App.viewModel.LogManager_ViewModel.Logger_Title($"Exit request\n", MethodBase.GetCurrentMethod(), ProcessStateModel.GET(ProcessState.PREPARE_TO_EXIT));

                await Papouch_teploměr.Tasks.SQL.SQL_Completed();

                App.CancellationTokenSource.Cancel();
                App.SerialPort?.Close();
                App.SerialPort?.Dispose();
                await Task.Delay(1000);

                e.Cancel = false;
                App.Current.Shutdown();
            }
            catch (Exception dd)
            {
                await System.Reflection.MethodBase.GetCurrentMethod().SW_ERROR_reportAsync(App.Current.Dispatcher, dd);
                e.Cancel = false;
            }
        }
    }
}
