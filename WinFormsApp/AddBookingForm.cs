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
    public partial class AddBookingForm : Form
    {

        private ApiService _apiService;
        private List<AvailableBookingsForTimeframe> _availableBookingsList;
        private List<AvailableBookingsForTimeframe> _selectedTimeSlot;


        public AddBookingForm()
        {
            _apiService = new ApiService();
            _availableBookingsList = new List<AvailableBookingsForTimeframe>();
            _selectedTimeSlot = new List<AvailableBookingsForTimeframe>();
            InitializeComponent();

            initializeDatePicker();
            initializeComboBox();
        }

        private void initializeDatePicker()
        {
            dtp_BookingDate.MinDate = DateTime.Now;
        }

        private async void initializeComboBox()
        {
            await updateComboBox();
        }

        private async void dtp_BookingDate_ValueChanged(object sender, EventArgs e)
        {
            await updateComboBox();
        }

        private async Task updateComboBox()
        {
            cmb_AvailableTimeSlot.Items.Clear();
            await getAviableTimeSlotsFromAPI();


            foreach (AvailableBookingsForTimeframe availableBookings in _availableBookingsList)
            {

                string builder = availableBookings.TimeStart.Hour + " - " + availableBookings.TimeEnd.Hour + ", Stubs: " +
                    availableBookings.AvailableStubs;


                cmb_AvailableTimeSlot.Items.Add(builder);

            }
        }

        private async Task getAviableTimeSlotsFromAPI()
        {
            string date = dtp_BookingDate.Value.ToString("yyyy-MM-dd");
            string url = $"Booking?start={date}&showAvailable=true&sortOption=0";
            _availableBookingsList = await _apiService.GetAsync<List<AvailableBookingsForTimeframe>>(url);

            // https://localhost:7021/Booking?start=2023-11-30&showAvailable=true&sortOption=0

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cmb_AvailableTimeSlot_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmb_AvailableTimeSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            AvailableBookingsForTimeframe selectedTimeSlot = _availableBookingsList[cmb_AvailableTimeSlot.SelectedIndex];

            if (selectedTimeSlot.AvailableStubs > 0)
            {
                btn_Add.Enabled = true;
            }
            else
            {
                btn_Add.Enabled = false;
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            AvailableBookingsForTimeframe selectedTimeSlot = _availableBookingsList[cmb_AvailableTimeSlot.SelectedIndex];

            _selectedTimeSlot.Add(selectedTimeSlot);

            updateListBox();
        }

        private void updateListBox()
        {
            lbo_SelectedTime.Enabled = true;
            lbo_SelectedTime.Items.Clear();

            foreach (AvailableBookingsForTimeframe selectedTimeSlot in _selectedTimeSlot)
            {
                string builder = selectedTimeSlot.TimeStart.Date.ToString("dd-MM-yyyy") + ": " + selectedTimeSlot.TimeStart.TimeOfDay + " - " + selectedTimeSlot.TimeEnd.TimeOfDay;

                lbo_SelectedTime.Items.Add(builder);
            }

            if (_selectedTimeSlot.Count == 0)
            {
                lbo_SelectedTime.Enabled = false;
                lbo_SelectedTime.Items.Add("No bookings selected....");
            }

        }

        private void lbo_SelectedTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_Remove.Enabled = true;
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            AvailableBookingsForTimeframe selectedTimeSlot = _selectedTimeSlot[lbo_SelectedTime.SelectedIndex];

            _selectedTimeSlot.Remove(selectedTimeSlot);

            updateListBox();
            btn_Remove.Enabled = false;
        }



        private void updateButtonOk()
        {
            bool bookingCondition = _selectedTimeSlot.Count > 0;
            bool fullNameCondition = txtBox_FullName.Text.Trim().Length > 0;
            bool eMailCondition = txtBox_Email.Text.Trim().Length > 0;
            bool phoneNumberCondition = txtBox_PhoneNumber.Text.Trim().Length > 0;

            if (bookingCondition && fullNameCondition && eMailCondition && phoneNumberCondition)
            {
                btn_OK.Enabled = true;
            }
            else
            {
                btn_OK.Enabled = false;
            }
        }
        private void lbo_SelectedTime_EnabledChanged(object sender, EventArgs e)
        {
            updateButtonOk();
        }

        private void txtBox_FullName_TextChanged(object sender, EventArgs e)
        {
            updateButtonOk();
        }

        private void txtBox_Email_TextChanged(object sender, EventArgs e)
        {
            updateButtonOk();
        }

        private void txtBox_PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            updateButtonOk();
        }

        private async void btn_OK_Click(object sender, EventArgs e)
        {
            string url = "Booking";
            BookingRequest bookingRequest = new BookingRequest();
            DTOCustomer customer = new DTOCustomer() { FullName = txtBox_FullName.Text, Email = txtBox_Email.Text, Phone = txtBox_PhoneNumber.Text};

            foreach(AvailableBookingsForTimeframe selectedBookings in _selectedTimeSlot)
            {
                bookingRequest.Appointments.Add(new NewBooking() { TimeStart = selectedBookings.TimeStart, TimeEnd = selectedBookings.TimeEnd, Notes = txtBox_Notes.Text });

            }
            bookingRequest.Customer = customer;

            try{
                int a = await _apiService.PostAsync<BookingRequest, int>(url, bookingRequest);
                this.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            
        }
    }
}
