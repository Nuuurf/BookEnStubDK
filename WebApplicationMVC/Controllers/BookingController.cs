using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.ComponentModel;
using WebApplicationMVC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Serialization;

namespace WebApplicationMVC.Controllers {
    public class BookingController : Controller {
        public IActionResult Index() {
            return View();
        }

        // API URL: /Booking/ConfirmBoooking/
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] List<TempBooking> data) {
            List<NewBooking> bookingList = new List<NewBooking>();
            NewBooking booking = new NewBooking();

            //Convert to correct Model
            foreach (TempBooking item in data) {
                string[] timeParts = item.Time.Split('-');
                string startTime = timeParts[0].Trim();
                string endTime = timeParts[1].Trim();
                DateTime date = DateTime.ParseExact(item.Date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                TimeSpan.TryParseExact(startTime, "hh\\.mm", CultureInfo.InvariantCulture, out TimeSpan timeStart);
                TimeSpan.TryParseExact(endTime, "hh\\.mm", CultureInfo.InvariantCulture, out TimeSpan timeEnd);

                DateTime combinedDateTimeStart = date.Date + timeStart;
                DateTime combinedDateTimeEnd = date.Date + timeEnd;

                booking.TimeStart = combinedDateTimeStart.ToUniversalTime();
                booking.TimeEnd = combinedDateTimeEnd.ToUniversalTime();
                booking.Notes = "";
                bookingList.Add(booking);
            }

            //string json = JsonConvert.SerializeObject(bookingList);
            string jsonString = JsonConvert.SerializeObject(bookingList).ToString();
            string jsonStringWithoutBackslashes = jsonString.Replace("\\", "");

            Console.WriteLine(jsonString);


            bool success = false;
            if (bookingList.Count > 1) {
                return Ok();
            } else {
                success = await SendBooking(jsonStringWithoutBackslashes, false, "https://localhost:7021/Booking/");
            }

            if(success) {
                return Ok("Booking confirmed successfully");
            } else {
                return BadRequest("Error occured or booking timeslot is full");
            }
            
        }

        public async Task<bool> SendBooking(string appointments, bool multipleBookings, string apiUrl) {
            bool success = false;
            string modified = appointments.Replace("[", "").Replace("]", "");
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(modified);

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string camelCaseJson = JsonConvert.SerializeObject(jsonObject, settings);

            HttpContent content = new StringContent(camelCaseJson, Encoding.UTF8, "application/json");


            using (HttpClient client = new HttpClient()) {
                try {
                    // Make the GET request to the API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode) {
                        // Read and parse the response content
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Assuming success when the response is received
                        success = true;
                    } else {
                        // Handle unsuccessful response
                        // You might log the error or perform additional actions here
                        success = false;
                    }
                } catch (Exception ex) {
                    // Handle exception
                    // You might log the exception or perform additional actions here
                    success = false;
                }
            }

            return success;
        }

        // API URL: /Booking/Confirm
        // Added to prevent Error page
        [HttpGet]
        public IActionResult Confirm() {
            return RedirectToAction("Index");
        }

        // API URL: /Booking/Confirm
        [HttpPost]
        public IActionResult Confirm([FromBody] string data) {
            if (data == "Confirm") {
                return View();
            } else {
                return BadRequest("Invalid data or value");
            }
        }

        // API URL: /Booking/GetAvailaibleTimes/{date}
        [HttpGet]
        public async Task<ActionResult> GetAvailableTimes(string date) {
            // Simulated URL for fetching data (replace this with your actual API endpoint)
            string apiUrl = $"https://localhost:7021/{date}";

            using (HttpClient client = new HttpClient()) {
                try {
                    // Make the GET request to the API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode) {
                        // Read and parse the response content
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Assuming you want to return JSON
                        return Content(responseData, "application/json");
                    } else {
                        // Handle unsuccessful response
                        return Json(new { error = $"Error: {response.StatusCode}" });
                    }
                } catch (Exception ex) {
                    // Handle exception
                    return Json(new { error = $"Exception: {ex.Message}" });
                }
            }
        }
    }
}
