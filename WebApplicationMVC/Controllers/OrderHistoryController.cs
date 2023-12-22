using HtmlTags;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using WebApplicationMVC.Models;
using static System.Net.WebRequestMethods;

namespace WebApplicationMVC.Controllers {
    public class OrderHistoryController : Controller {

        private int? number = 0;

        public IActionResult Index() {
            return View(number);
        }

        [HttpPost]
        public async Task<IActionResult> BookingHistoryAsync(int? inputValue) {
            //get bookings for person with the phone number
            string uri = "https://localhost:7021/Booking/" + inputValue;

            IActionResult result = null;

            using (HttpClient client = new HttpClient()) {
                try {
                    //send request to api to get bookings connected with customer
                    HttpResponseMessage response = await client.GetAsync(uri);

                    //check if the reponcecode is successfull
                    if (response.IsSuccessStatusCode) {
                        //read responce data
                        string responseData = await response.Content.ReadAsStringAsync();

                        //deserialize request body into bookingHistoryItems
                        List<BookingHistoryItem> items = JsonConvert.DeserializeObject<List<BookingHistoryItem>>(responseData);

                        //Initialize model with items inserted.
                        BookingHistoryItemContainer container = new BookingHistoryItemContainer {
                            BookingItems = items
                        };

                        //Send to booking history view
                        result = View(container);
                    }
                    else if(response.StatusCode == HttpStatusCode.BadRequest) {
                        result = RedirectToAction("Index");
                    }
                    else {
                        //Write error screen if something if response is not successful code.
                        result = Json(new { error = $"Error: {response}" });
                    }
                }
                //Write error screen if some exception has occured during the running of method.
                catch (Exception ex) {
                    result = Json(new { error = $"Error: {Response}" });
                }
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByPhone(string phone) {
            IActionResult result = null;

            using(HttpClient client = new HttpClient()) {
                string uri = "https://localhost:7021/Booking/" + phone; //test value

                try {
                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode) {
                        string responseData = await response.Content.ReadAsStringAsync();

                        List<BookingHistoryItem> items = JsonConvert.DeserializeObject<List<BookingHistoryItem>>(responseData);

                        //sort items by date for better user experience
                        items.Sort((x, y) => y.TimeStart.CompareTo(x.TimeStart));

                        //Create a html table
                        var table = new TableTag();
                        table.AddClass("table table-boardered");

                        //add row headers to match the prexisting table format
                        table.AddHeaderRow(row => {
                            row.Header("Booking id");
                            row.Header("Noter");
                            row.Header("Start tidspunkt");
                            row.Header("Slut tidspunkt");
                        });

                        //insert the data from the booking history items
                        foreach(BookingHistoryItem item in items) {
                            //mark rows with bookings that are older than current date with red.
                            if (item.TimeEnd < DateTime.Now) {
                                table.AddBodyRow(row => {
                                    row.Cell(item.Id + "").Attr("class", "text-danger text-lg");
                                    row.Cell(item.Notes).Attr("class", "text-danger text-lg");
                                    row.Cell(item.TimeStart.ToString()).Attr("class", "text-danger text-lg");
                                    row.Cell(item.TimeEnd.ToString()).Attr("class", "text-danger text-lg");
                                });
                            }
                            else {
                                table.AddBodyRow(row => {
                                    row.Cell(item.Id + "");
                                    row.Cell(item.Notes);
                                    row.Cell(item.TimeStart.ToString());
                                    row.Cell(item.TimeEnd.ToString());
                                });
                            }
                            
                        }
                        
                        //convert table object into html content that can be sent to page.
                        result = Content(table.ToHtmlString(), "text/html");
                    }
                    else {
                        //return the status code if it is not successful
                        return StatusCode((int) response.StatusCode);
                    }
                }
                catch (Exception ex) {
                    //return status code 500 if some exception occures.
                    return StatusCode(500, "Some internal server error has occured " + ex.Message);
                }
            }

            //test string
            //string updatedHtml = "<tr><td>Booking 1</td><td>Details</td></tr><tr><td>Booking 2</td><td>Details</td></tr>";

            //return Content(updatedHtml, "text/html"); // Return HTML content as a string
            return result;
        }
    }
}
