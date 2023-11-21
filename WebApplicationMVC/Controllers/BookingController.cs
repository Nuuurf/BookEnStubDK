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

            //Convert to correct Class
            foreach (TempBooking item in data) {
                string[] timeParts = item.Time.Split('-');
                string startTime = timeParts[0].Trim();
                string endTime = timeParts[1].Trim();
                DateTime date = DateTime.ParseExact(item.Date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                TimeSpan.TryParseExact(startTime, "hh\\.mm", CultureInfo.InvariantCulture, out TimeSpan timeStart);
                TimeSpan.TryParseExact(endTime, "hh\\.mm", CultureInfo.InvariantCulture, out TimeSpan timeEnd);

                DateTime combinedDateTimeStart = date.Date + timeStart;
                DateTime combinedDateTimeEnd = date.Date + timeEnd;

                booking.TimeStart = combinedDateTimeStart.ToUniversalTime(); //Convert time
                booking.TimeEnd = combinedDateTimeEnd.ToUniversalTime();
                booking.Notes = "";
                bookingList.Add(booking);
            }

            string jsonString = JsonConvert.SerializeObject(bookingList).ToString();
            string jsonStringWithoutBackslashes = jsonString.Replace("\\", "");

            (bool success, int id) = (false, 0);
            (success, id) = await SendBooking(jsonStringWithoutBackslashes, "https://localhost:7021/Booking/Multiple");

            if (success) {
                return Ok(new { Success = success, ID = id });
            } else {
                return BadRequest("Error occured or booking timeslot is full");
            }

        }

        public async Task<(bool success, int id)> SendBooking(string appointments, string apiUrl) {
            bool success = false;
            int id = -1;
            string modified = appointments.Replace("[", "").Replace("]", "");
            string camelCaseJson = ConvertJSONToCamelCase(modified);
            string test = "[]";
            string appending = test.Insert(1, camelCaseJson);

            HttpContent content = new StringContent(appending, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                try {
                    // Make the GET request to the API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode) {
                        // Read and parse the response content
                        string responseData = await response.Content.ReadAsStringAsync();
                        int.TryParse(responseData, out id);
                        // Assuming success when the response is received
                        success = true;
                    } else {
                        // Handle unsuccessful response
                        success = false;
                    }
                } catch (Exception ex) {
                    // Handle exception
                    success = false;
                }
            }
            return (success, id);
        }

        private string ConvertJSONToCamelCase(string jsonString) {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string camelCaseJson = JsonConvert.SerializeObject(jsonObject, settings);

            return camelCaseJson;
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
