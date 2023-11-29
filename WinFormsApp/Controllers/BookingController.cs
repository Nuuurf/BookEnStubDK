using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    internal class BookingController
    {
        ApiService _apiService;

        public BookingController()
        {
            _apiService = new ApiService();

        }

        /// <summary>
        /// This method gets a list of bookings for a range of dates
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="showAvailable"></param>
        /// <returns></returns>

        public async Task<List<Booking>> getBookingsFromAPI(DateTime timeStart, DateTime timeEnd, bool showAvailable)
        {
            //A list to hold the bookings
            List<Booking> bookings = new List<Booking>();
            //Constructs the URL that is to be used.
            string apiUrl = $"Booking?start={timeStart.ToString("yyyy-MM-dd")}&end={timeEnd.ToString("yyyy-MM-dd")}&showAvailable={showAvailable}";
            //Store the bookings that the method gets back from the API
            bookings = await _apiService.GetAsync<List<Booking>>(apiUrl);
            //returns bookinglist
            return bookings;

        }
    }
}
