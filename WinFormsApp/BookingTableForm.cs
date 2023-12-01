using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WinFormsApp.Controllers;
using WinFormsApp.Exceptions;
using WinFormsApp.Models;

namespace WinFormsApp
{
    /// <summary>
    /// Tabel class for showing the data. Contains a filter function.
    /// </summary>
    public partial class BookingTableForm : Form
    {
        BookingController _bookingController;
        ApiService _apiService;
        List<Booking> _bookings;

        /// <summary>
        /// Dates that the functions use for their search.
        /// If the filter are changed, so will these fields.
        /// </summary>
        DateTime _start;
        DateTime _end;
        int? _stubID;
        int? _orderID;
        string _email;
        string _phone;

        /// <summary>
        /// Only controller for this class. Initialize all elements.
        /// </summary>
        public BookingTableForm()
        {
            _bookingController = new BookingController();
            _apiService = new ApiService();
            _bookings = new List<Booking>();

            SetToday();
            InitializeComponent();
            initializeDatepickers();
            getData(new BookingRequestFilter());
        }

        /// <summary>
        /// Initialize datepickers. Ensures end date cant be less than Start date.
        /// </summary>
        private void initializeDatepickers()
        {
            //Sets end dates minimum to start dates current value.
            dtp_EndDate.MinDate = dtp_StartDate.Value;

            //Sets both date pickers value to todays date.
            dtp_StartDate.Value = DateTime.Now;
            dtp_EndDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Set dates-fields to todays date.
        /// </summary>
        private void SetToday()
        {
            //Sets _start to current date.
            _start = DateTime.Now;
            //Reduces procesing to only a date. Removes the time aspect.
            _start = new DateTime(_start.Year, _start.Month, _start.Day, 0, 0, 0);
            _end = _start;
        }
        /// <summary>
        /// Contacts the API and populates the table.
        /// Possible errors: No connection, No bookings found.
        /// </summary>
        private async void getData(BookingRequestFilter brf)
        {
            //Do to possbile errors, try/catch needed.
            try
            {
                dataGridView1.AutoGenerateColumns = true;
                //Tries to get bookings from API.
                _bookings = await _bookingController.getBookingsFromAPI(brf);
                //Displays the list of bookings in the table.

                foreach (Booking booking in _bookings)
                {
                    booking.TimeStart = booking.TimeStart.ToLocalTime();
                    booking.TimeEnd = booking.TimeEnd.ToLocalTime();
                }

                dataGridView1.DataSource = _bookings;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.Name == "Notes")
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                }
            }
            catch (HttpRequestException e)
            {
                //Pops up a message box if there is no connection to the API.
                MessageBox.Show(e.Message, "No connection to API");
            }
            catch (NoBookingException e)
            {
                //Gives an exception if there isn't a booking a current date.
                MessageBox.Show("No bookings found", "Error");
                //Clears the current tabel.
                dataGridView1.DataSource = null;
                _bookings.Clear();
                dataGridView1.DataSource = _bookings;
            }
        }
        /// <summary>
        /// When today checkbox changes, certain elements will be disabled/enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBox_Today_CheckedChanged(object sender, EventArgs e)
        {
            //Checks if the checkbox is checked.
            if (chkBox_Today.Checked)
            {
                dtp_StartDate.Enabled = false;
                dtp_EndDate.Enabled = false;
                SetToday();
                dtp_StartDate.Value = DateTime.Now;
                dtp_EndDate.Value = DateTime.Now;
            }
            //Enables the datetime pickers.
            else
            {
                dtp_StartDate.Enabled = true;
                dtp_EndDate.Enabled = true;
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED YET
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Add_Click(object sender, EventArgs e)
        {

            AddBookingForm frm = new AddBookingForm();
            frm.Show();

            //MessageBox.Show("This function is not implemented", "Not implemented");
        }

        /// <summary>
        /// Updates the minimum value for end date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtp_StartDate_ValueChanged(object sender, EventArgs e)
        {
            dtp_EndDate.MinDate = dtp_StartDate.Value;
        }

        /// <summary>
        /// Refreshes the table to match filters or get newest bookings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //If the checkbox is ticked, it sets day pickers to current date.
            //and gets the data of bookings for current date.
            if (chkBox_Today.Checked)
            {
                SetToday();
            }
            //Free use of the date pickers and get the data for bookings.
            else
            {
                _start = dtp_StartDate.Value;
                _end = dtp_EndDate.Value;
                
            }

            //Filter
            try
            {
                if(txt_StubID.Text.Length > 0)
                {
                    _stubID = int.Parse(txt_StubID.Text);
                }
                else 
                { 
                    _stubID = null; 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Stub ID: must be a number");
                txt_StubID.Text = "";
            }

            try
            {
                if(txt_OrderID.Text.Length > 0)
                {
                    _orderID = int.Parse(txt_OrderID.Text);
                }
                else 
                { 
                    _orderID = null; 
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Order ID: must be a number");
                txt_OrderID.Text = "";
            }
            
            
            _email = txt_email.Text;
            _phone = txt_Phone.Text;

            getData(new BookingRequestFilter(_start,_end,false,_stubID,_orderID,_email,_phone));
        }

        /// <summary>
        /// Sorts colums when clicked. Lowest to highest.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Selects a column based on it's given index.
            DataGridViewColumn selectedColumn = dataGridView1.Columns[e.ColumnIndex];

            // Gets the list of bookings and sets it to the table.
            List<Booking> dataSource = (List<Booking>)dataGridView1.DataSource;

            // New list for sorting the index.
            List<Booking> sortedData = new List<Booking>();

            //Switch case used to determ which index column is clicked and
            //executes the sorting method.
            switch (e.ColumnIndex)
            {
                case 0:
                    sortedData = dataSource.OrderBy(item => item.Id).ToList();
                    break;

                case 1:
                    sortedData = dataSource.OrderBy(item => item.StubId).ToList();
                    break;

                case 2:
                    sortedData = dataSource.OrderBy(item => item.Customer.FullName).ToList();
                    break;

                case 3:
                    sortedData = dataSource.OrderBy(item => item.TimeStart).ToList();
                    break;

                case 4:
                    sortedData = dataSource.OrderBy(item => item.TimeEnd).ToList();
                    break;
                case 5:
                    sortedData = dataSource.OrderBy(item => item.Notes).ToList();
                    break;

                default:
                    sortedData = dataSource.OrderBy(item => item.TimeStart).ToList();
                    break;
            }
            dataGridView1.DataSource = sortedData;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MessageBox.Show("This should open the details tab on the selected row ", "Not implemented");
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            btn_Edit.Enabled = true;
            btn_Delete.Enabled = true;
            btn_Details.Enabled = true;
        }

        private void btn_Details_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This should open the details tab on the selected row ", "Not implemented");
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This should open the Edit tab on the selected row ", "Not implemented");
        }

        private async void btn_Delete_Click(object sender, EventArgs e)
        {
            Booking? selectedBooking = new Booking();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedBooking = dataGridView1.SelectedRows[0].DataBoundItem as Booking;
            }
            if (selectedBooking != null)
            {
                // Display a confirmation dialog
                DialogResult result = MessageBox.Show($"Are you sure you want to delete booking {selectedBooking.Id}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's response
                if (result == DialogResult.Yes)
                {
                    await _apiService.DeleteAsync($"Booking/{selectedBooking.Id}");

                    getData(new BookingRequestFilter());
                }
                else
                {
                    // User canceled the deletion
                    // You can add additional logic or simply do nothing
                }
            }

            //MessageBox.Show("This should open the Delete box on the selected row ", "Not implemented");
        }
    }
}
