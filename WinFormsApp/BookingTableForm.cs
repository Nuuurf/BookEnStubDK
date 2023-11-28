using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;

namespace WinFormsApp
{
    public partial class BookingTableForm : Form
    {
        BookingController _bookingController;
        List<Booking> bookings;
        public BookingTableForm()
        {
            _bookingController = new BookingController();
            bookings = new List<Booking>();

            InitializeComponent();
        }

        private void chkBox_Today_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Today.Checked)
            {
                dtp_StartDate.Enabled = false;
                dtp_EndDate.Enabled = false;
            }
            else
            {
                dtp_StartDate.Enabled = true;
                dtp_EndDate.Enabled = true;
            }
        }

        private async void btn_Add_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not implemented", "Not implemented");
            bookings = await _bookingController.getBookingsFromAPI(null,null,false);
        }
    }
}
