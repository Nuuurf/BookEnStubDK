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
        List<Booking> _bookings;

        /// <summary>
        /// Dates that the functions use for their search.
        /// If the filter are changed, so will these fields.
        /// </summary>
        DateTime _start;
        DateTime _end;

        /// <summary>
        /// Only controller for this class. Initialize all elements.
        /// </summary>
        public BookingTableForm()
        {
            _bookingController = new BookingController();
            _bookings = new List<Booking>();

            SetToday();
            getData();
            InitializeComponent();
            initializeDatepickers();
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
        private async void getData()
        {
            //Do to possbile errors, try/catch needed.
            try
            {
                //Tries to get bookings from API.
                _bookings = await _bookingController.getBookingsFromAPI(_start, _end.AddDays(1), false);
                //Displays the list of bookings in the table.
                dataGridView1.DataSource = _bookings;
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
            MessageBox.Show("This function is not implemented", "Not implemented");
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
                getData();
            }
            //Free use of the date pickers and get the data for bookings.
            else
            {
                _start = dtp_StartDate.Value;
                _end = dtp_EndDate.Value;
                getData();
            }
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
                    sortedData = dataSource.OrderBy(item => item.TimeStart).ToList();
                    break;

                case 2:
                    sortedData = dataSource.OrderBy(item => item.TimeEnd).ToList();
                    break;

                case 3:
                    sortedData = dataSource.OrderBy(item => item.Notes).ToList();
                    break;

                case 4:
                    sortedData = dataSource.OrderBy(item => item.StubId).ToList();
                    break;

                default:
                    sortedData = dataSource.OrderBy(item => item.TimeStart).ToList();
                    break;
            }
            dataGridView1.DataSource = sortedData;
        }
    }
}
