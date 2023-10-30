using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papouch_teploměr.ViewModel
{
    public class ViewModel:ViewModelBase
    {
        private string[] ports;
        private string port;
        private float temperature;
        private string title;
        private string status;
        private ObservableCollection<Chart> temperatures;
        private string location;

        public ViewModel()
        {
            LogManager_ViewModel = new LogManager_ViewModel();
            Temperatures = new ObservableCollection<Chart>();
        }

        public string[] Ports
        {
            get
            {
                return ports;
            }
            set
            {
                ports = value;
                UpdateUI();
            }
        }

        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                UpdateUI();
            }
        }
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                UpdateUI();
            }
        }
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
                UpdateUI();
            }
        }

        public ObservableCollection<Chart> Temperatures
        {
            get
            {
                return temperatures;
            }
            set
            {
                temperatures = value;
                UpdateUI();
            }
        }

        public LogManager_ViewModel LogManager_ViewModel { get; set; }
    }
}
