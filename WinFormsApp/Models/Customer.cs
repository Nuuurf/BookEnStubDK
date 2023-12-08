using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class Customer
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public override string ToString() {
            if (FullName == null) {
                return "N/A";
            }
            return FullName;
        }
    }
}
