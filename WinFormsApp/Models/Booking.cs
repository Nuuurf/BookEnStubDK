using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int StubId { get; set; } //The assigned number for the stub assigned to this booking
        public Customer? Customer { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? Notes { get; set; }

        

        
    }
}
