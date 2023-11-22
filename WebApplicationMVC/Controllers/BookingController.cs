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
using System;

namespace WebApplicationMVC.Controllers {
    public class BookingController : Controller {

        //API URL: /Booking/Index/
        public IActionResult Index() {
            return View();
        }

        // API URL: /Booking/ConfirmBoooking/
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] List<string> dateTimes) {
            List<NewBooking> bookingList = new List<NewBooking>();
            int id = -1;
            
            foreach (var dateTimeString in dateTimes)
            {
                if (DateTime.TryParse(dateTimeString, out DateTime parsedDateTime))
                {
                    // Add to the list if the parsing is successful
                    bookingList.Add(new NewBooking()
                    {
                        TimeStart = parsedDateTime,
                        TimeEnd = parsedDateTime.AddHours(1)
                    });
                }
                else
                {
                    return BadRequest("Invalid datetime format");
                }
            }
            
            try {
                id = await SendBooking(bookingList);
            } catch (Exception ex) {
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
            return Ok(new { ID = id });
        }

        // Send Booking to API Backend
        // URL Booking being sent to: "https://localhost:7021/Booking/"
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
                } catch (Exception) {
                    // Handle exception
                    throw;
                }
            }
            return id;
        }

        // API URL: /Booking/Confirm
        // Added to prevent error page and force user to "Index" page
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
