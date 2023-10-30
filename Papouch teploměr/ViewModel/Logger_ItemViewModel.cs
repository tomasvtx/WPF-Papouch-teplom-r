using Logger;
using System.Windows.Media;
using System;

namespace Papouch_teploměr.ViewModel
{
    public class Logger_ItemViewModel : ViewModelBase
    {
        private LOG_Tasks.Log Logger;

        public Logger_ItemViewModel(LOG_Tasks.Log log)
        {
            Logger = log;
        }

        public string Message
        {
            get
            {
                return Logger.Message;
            }
            set
            {
                Logger.Message = value;
                UpdateUI();
            }
        }

        public DateTime Date
        {
            get
            {
                return Logger.Date;
            }
            set
            {
                Logger.Date = value;
                UpdateUI();
            }
        }

        public string ProcessState
        {
            get
            {
                return Logger.ProcessState;
            }
            set
            {
                Logger.ProcessState = value;
                UpdateUI();
            }
        }

        public SolidColorBrush Color
        {
            get
            {
                return Logger.Color;
            }
            set
            {
                Logger.Color = value;
                UpdateUI();
            }
        }

        public string ProcessStateString
        {
            get
            {
                return Logger.ProcessStateString;
            }
            set
            {
                Logger.ProcessStateString = value;
                UpdateUI();
            }
        }

        public string Time
        {
            get
            {
                return Logger.Time;
            }
            set
            {
                Logger.Time = value;
                UpdateUI();
            }
        }


        public int Width
        {
            get
            {
                return Logger.Width;
            }
            set
            {
                Logger.Width = value;
                UpdateUI();
            }
        }
    }
}