using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papouch_teploměr.ViewModel
{
    public class Chart
    {
        public DateTime Datum { get; set; }
        public float Temperature { get; set; }
        public bool RowAffected { get; set; }
        public string Location { get; internal set; }
    }
}
