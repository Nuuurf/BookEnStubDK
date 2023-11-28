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

        public BookingController() { 
            _apiService = new ApiService();
        
        }

        public async Task<List<Booking>> getBookingsFromAPI(DateTime? start, DateTime? end, bool showAvailable)
        {
            //https://localhost:7021/Booking?start=2024-08-11&end=2024-08-12&showAvailable=false

            List<Booking> bookings = new List<Booking>();

            string apiUrl = $"Booking?start=2024-08-11&end=2024-08-12&showAvailable=false";

            bookings = await _apiService.GetAsync<List<Booking>>(apiUrl);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the GET request to the API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and parse the response content
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Assuming you want to return JSON
                        //return Content(responseData, "application/json");
                    }
                    else
                    {
                        // Handle unsuccessful response
                        //return Json(new { error = $"Error: {response}" });
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    //return Json(new { error = $"Exception: {ex.Message}" });
                }
                return bookings;
            }
        }
    }
}
