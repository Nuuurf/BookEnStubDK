using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class AvailableBookingsForTimeframe
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int AvailableStubs { get; set; }
    }
}
