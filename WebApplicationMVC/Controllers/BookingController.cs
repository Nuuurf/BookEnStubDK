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

        // URL: /Booking/Index/
        public IActionResult Index() {
            return View();
        }

        // URL: /Booking/ConfirmBooking/
        [HttpPost]
        public async Task<IActionResult> BookAppointment(string fullName, string email, string phoneNumber, string? notes, string jsonString) {
            List<NewBooking> bookingList = new List<NewBooking>();
            int id = -1;
            // Check if booking string is empty
            if (string.IsNullOrEmpty(jsonString) || jsonString == "[]") {
                return RedirectToAction("Index");
            } else {
                // Fix for exception if notes are null :)
                if (notes == null) { notes = String.Empty; }

                List<string> dateTimes = JsonConvert.DeserializeObject<List<string>>(jsonString)!;

                foreach (var dateTimeString in dateTimes) {
                    if (DateTime.TryParse(dateTimeString, out DateTime parsedDateTime)) {
                        // Add to the list if the parsing is successful
                        bookingList.Add(new NewBooking() {
                            TimeStart = parsedDateTime,
                            TimeEnd = parsedDateTime.AddHours(1),
                            Notes = notes
                        });
                    } else {
                        return BadRequest("Invalid DateTime format");
                    }
                }

                try {
                    Customer customer = new Customer {
                        FullName = fullName,
                        Email = email,
                        Phone = phoneNumber
                    };
                    id = await SendBooking(bookingList, customer);
                } catch (Exception ex) {
                    Response.StatusCode = 500;
                    return View("BookingError", ex.Message);
                }
            }
            return View("BookingConfirmed", id);
        }

        // Send Booking to API Backend
        public async Task<int> SendBooking(List<NewBooking> appointments, Customer customer) {
            int id = -1;
            var data = new {
                Appointments = appointments,
                Customer = customer
            };
            string json = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

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

        // URL: /Booking/Confirm
        // Added to prevent error page and force user to "Index" page
        [HttpGet]
        public IActionResult Confirm() {
            return RedirectToAction("Index");
        }

        // API URL: /Booking/Confirm
        [HttpPost]
        public IActionResult Confirm(string jsonDataInput) {
            Console.WriteLine(jsonDataInput);
            if (jsonDataInput == "Confirm") {
                return View();
            } else {
                return RedirectToAction("Index");
            }
        }

        // URL: /Booking/GetAvailaibleTimes/{date}
        [HttpGet]
        public async Task<ActionResult> GetAvailableTimes(string date) {
            // Simulated URL for fetching data (replace this with your actual API endpoint)
            string apiUrl = $"https://localhost:7021/Booking?start={date}&showAvailable=true";

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
