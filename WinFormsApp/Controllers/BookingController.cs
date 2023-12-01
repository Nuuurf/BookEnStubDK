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
        /// <param name="brf"></param>
        /// <returns></returns>

        public async Task<List<Booking>> getBookingsFromAPI(BookingRequestFilter brf)
        {
            //A list to hold the bookings
            List<Booking> bookings = new List<Booking>();
            //Constructs the URL that is to be used.
            string apiUrl = $"Booking?" +
                $"start={brf.Start.ToString("yyyy-MM-dd")}" +
                $"&end={brf.End.ToString("yyyy-MM-dd")}" +
                $"&showAvailable={brf.ShowAvailable}" +
                $"&StubId={brf.StubId}" +
                $"&OrderId={brf.OrderId}" +
                $"&CustomerEmail={brf.CustomerEmail}" +
                $"&CustomerPhone={brf.CustomerPhone}";
            //Store the bookings that the method gets back from the API
            bookings = await _apiService.GetAsync<List<Booking>>(apiUrl);
            //returns bookinglist
            return bookings;

        }
    }
}
