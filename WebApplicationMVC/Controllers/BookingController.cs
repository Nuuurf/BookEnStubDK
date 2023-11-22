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
            //NewBooking booking = new NewBooking();
            int id = -1;

            //Convert to correct Class
            foreach (TempBooking item in data) {
                NewBooking booking = new NewBooking();
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

            try {
                id = await SendBooking(bookingList);
            } catch (Exception ex) {
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
            return Ok(new { ID = id });
        }

        public async Task<int> SendBooking(List<NewBooking> appointments) {
            int id = -1;
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(appointments);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                try {
                    // Make the GET request to the API
                    HttpResponseMessage response = await client.PostAsync("https://localhost:7021/Booking/", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode) {
                        // Read and parse the response content
                        string responseData = await response.Content.ReadAsStringAsync();
                        int.TryParse(responseData, out id);
                    } else {
                        // Handle unsuccessful response
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        string[] errorParts = errorMessage.Split(':'); // Splitting by ':' character
                        string actualErrorMessage = errorParts.Length > 1 ? errorParts[1].Trim() : errorMessage;

                        throw new Exception(actualErrorMessage);
                    }
                } catch (Exception ex) {
                    // Handle exception
                    throw;
                }
            }
            return id;
        }

        private string ConvertJSONToCamelCase(string jsonString) {
            List<Dictionary<string, object>> data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonString);

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string camelCaseJson = JsonConvert.SerializeObject(data, settings);

            return camelCaseJson;
        }

        // API URL: /Booking/Confirm
        // Added to prevent Error page and force user to specific page
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
                return BadRequest("Invalid booking data");
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
                        return Json(new { error = $"Error: {response}" });
                    }
                } catch (Exception ex) {
                    // Handle exception
                    return Json(new { error = $"Exception: {ex.Message}" });
                }
            }
        }
    }
}
