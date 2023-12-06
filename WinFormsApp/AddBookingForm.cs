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
        private List<AvailableStubsForHour> _availableBookingsList;
        private List<AvailableStubsForHour> _selectedTimeSlot;


        public AddBookingForm()
        {
            _apiService = new ApiService();
            _availableBookingsList = new List<AvailableStubsForHour>();
            _selectedTimeSlot = new List<AvailableStubsForHour>();
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
            btn_Add.Enabled = false;
        }

        private async Task updateComboBox()
        {
            cmb_AvailableTimeSlot.Items.Clear();
            try
            {
                await getAviableTimeSlotsFromAPI();


                foreach (AvailableStubsForHour availableBookings in _availableBookingsList)
                {
                    availableBookings.TimeStart = availableBookings.TimeStart.ToLocalTime();
                    availableBookings.TimeEnd = availableBookings.TimeEnd.ToLocalTime();
                    string builder = availableBookings.TimeStart.Hour + " - " + availableBookings.TimeEnd.Hour + ", Stubs: " +
                        availableBookings.AvailableStubIds.Count;


                    cmb_AvailableTimeSlot.Items.Add(builder);

                }
            }
            catch (HttpRequestException e)
            {
                //Pops up a message box if there is no connection to the API.
                MessageBox.Show(e.Message, "No connection to API");
            }

        }

        private async Task getAviableTimeSlotsFromAPI()
        {
            string date = dtp_BookingDate.Value.ToString("yyyy-MM-dd");
            string url = $"Booking?start={date}&showAvailable=true&sortOption=0";
            _availableBookingsList = await _apiService.GetAsync<List<AvailableStubsForHour>>(url);

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
            AvailableStubsForHour selectedTimeSlot = _availableBookingsList[cmb_AvailableTimeSlot.SelectedIndex];

            if (selectedTimeSlot.AvailableStubIds.Count > 0)
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
            AvailableStubsForHour selectedTimeSlot = _availableBookingsList[cmb_AvailableTimeSlot.SelectedIndex];

            _selectedTimeSlot.Add(selectedTimeSlot);

            UpdateListBox();
        }

        /// <summary>
        /// Updates the contents of the selected time slots ListBox based on the current state of the _selectedTimeSlot list.
        /// </summary>
        private void UpdateListBox()
        {
            // Enable the selected time slots ListBox
            lbo_SelectedTime.Enabled = true;

            // Clear existing items in the ListBox
            lbo_SelectedTime.Items.Clear();

            // Iterate through each selected time slot and add a formatted string representation to the ListBox
            foreach (AvailableStubsForHour selectedTimeSlot in _selectedTimeSlot)
            {
                // Format the selected time slot information
                string formattedTimeSlot = $"{selectedTimeSlot.TimeStart.Date.ToString("dd-MM-yyyy")}: " +
                                           $"{selectedTimeSlot.TimeStart.TimeOfDay} - {selectedTimeSlot.TimeEnd.TimeOfDay}";

                // Add the formatted time slot information to the ListBox
                lbo_SelectedTime.Items.Add(formattedTimeSlot);
            }

            // If no time slots are selected, disable the ListBox and display a message
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

        /// <summary>
        /// Handles the Click event of the Remove button.
        /// Removes the selected time slot from the list of selected time slots.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            // Get the selected time slot from the list box
            AvailableStubsForHour selectedTimeSlot = _selectedTimeSlot[lbo_SelectedTime.SelectedIndex];

            // Remove the selected time slot from the list of selected time slots
            _selectedTimeSlot.Remove(selectedTimeSlot);

            // Update the list box to reflect the changes
            UpdateListBox();

            // Disable the Remove button since there might not be any selected time slots after removal
            btn_Remove.Enabled = false;
        }

        /// <summary>
        /// Updates the state of the OK button based on certain conditions.
        /// </summary>
        private void UpdateButtonOk(object sender, EventArgs e)
        {
            // Check if there is at least one selected time slot
            bool bookingCondition = _selectedTimeSlot.Count > 0;

            // Check if the full name text box has non-empty content
            bool fullNameCondition = txtBox_FullName.Text.Trim().Length > 0;

            // Check if the email text box has non-empty content
            bool emailCondition = txtBox_Email.Text.Trim().Length > 0;

            // Check if the phone number text box has non-empty content
            bool phoneNumberCondition = txtBox_PhoneNumber.Text.Trim().Length > 0;

            // Enable the OK button only if all conditions are met
            if (bookingCondition && fullNameCondition && emailCondition && phoneNumberCondition)
            {
                btn_OK.Enabled = true; // Enable the OK button
            }
            else
            {
                btn_OK.Enabled = false; // Disable the OK button
            }
        }

        /// <summary>
        /// Every time the listbox named: lbo_SelectedTime changes enabled state this event will be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbo_SelectedTime_EnabledChanged(object sender, EventArgs e)
        {
            UpdateButtonOk(sender, e);
        }

        /// <summary>
        /// Every time the text in the textbox named: txtBox_FullName changes this event will be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_FullName_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonOk(sender, e);
        }

        /// <summary>
        /// Every time the text in the textbox named: txtBox_Email changes this event will be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Email_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonOk(sender, e);
        }

        /// <summary>
        /// Every time the text in the textbox named: txtBox_PhoneNumber changes this event will be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonOk(sender, e);
        }

        /// <summary>
        /// When the "ok"-button is clicked this event will be called, 
        /// it will attempt to gather infomation in the text boxes and create a request to the API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_OK_Click(object sender, EventArgs e)
        {
            btn_OK.Enabled = false;
            //Local Variables
            string url = "Booking";
            BookingRequest bookingRequest = new BookingRequest();

            //Build the BookingRequest-Object
            bookingRequest = BookingRequestBuilder();

            //API-Post request
            try
            {
                int a = await _apiService.PostAsync<BookingRequest, int>(url, bookingRequest);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private BookingRequest BookingRequestBuilder()
        {
            BookingRequest bookingRequest = new BookingRequest();
            DTOCustomer customer = new DTOCustomer() { FullName = txtBox_FullName.Text, Email = txtBox_Email.Text, Phone = txtBox_PhoneNumber.Text };


            foreach (AvailableStubsForHour selectedBookings in _selectedTimeSlot)
            {
                bookingRequest.Appointments.Add(new NewBooking() { TimeStart = selectedBookings.TimeStart.ToUniversalTime(), TimeEnd = selectedBookings.TimeEnd.ToUniversalTime(), Notes = txtBox_Notes.Text });

            }
            bookingRequest.Customer = customer;

            return bookingRequest;
        }

        private void txtBox_PhoneNumber_Leave(object sender, EventArgs e)
        {
            updateCustomerFields(txtBox_PhoneNumber.Text);
        }

        private async void updateCustomerFields(string phone)
        {
            string url = $"Customer/{phone}";
            try
            {
                DTOCustomer customer = await _apiService.GetAsync<DTOCustomer>(url);
                if (customer == null)
                {
                    txtBox_Email.Text = "";
                    txtBox_FullName.Text = "";
                    throw new Exception("Customer didnt exist");
                }
                txtBox_Email.Text = customer.Email;
                txtBox_FullName.Text = customer.FullName;
            }
            catch
            {
                txtBox_Email.Text = "";
                txtBox_FullName.Text = "";
            }
        }
    }
}
