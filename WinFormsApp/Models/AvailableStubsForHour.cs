using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class AvailableStubsForHour
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public List<int> AvailableStubIds { get; set; }
    }
}
