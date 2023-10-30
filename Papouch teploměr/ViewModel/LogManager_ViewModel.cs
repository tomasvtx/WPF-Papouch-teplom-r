using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Papouch_teploměr.ViewModel
{
    public class LogManager_ViewModel : ViewModelBase
    {
        private string title;
        private string state;
        private ObservableCollection<Logger_ItemViewModel> errList;

        public LogManager_ViewModel()
        {
            ErrList = new ObservableCollection<Logger_ItemViewModel>();
        }


        /// <summary>
        /// Scroll viewer - logger
        /// </summary>
        public ListView New_Scroll;

        /// <summary>
        /// Main title for MainWindow
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                UpdateUI();
            }
        }



        /// <summary>
        /// Message of program state
        /// </summary>
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                UpdateUI();
            }
        }


        /// <summary>
        /// logger
        /// </summary>
        public ObservableCollection<Logger_ItemViewModel> ErrList
        {
            get => errList;
            set
            {
                errList = value;
                UpdateUI();
            }
        }
    }
}