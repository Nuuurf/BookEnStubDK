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

            string uri = "https://localhost:7021/Booking/" + phone;


            List<BookingHistoryItem> bhi = new List<BookingHistoryItem>();

            try {
                //Use api service to get generic object
                bhi = await ApiService.Get<List<BookingHistoryItem>>(uri);

                //sort
                bhi.Sort((x, y) => y.TimeStart.CompareTo(x.TimeStart));

                //send json objects to frontend to be made into html
                //result = Ok(bhi);

                //send html to frontend
                HTMLgenerator<BookingHistoryItem> htmlGen = new HTMLgenerator<BookingHistoryItem>();
                return Content(htmlGen.GenerateTable(bhi), "text/html");
            }
            catch {
                result = StatusCode((int)ApiService.ResponseCode);
            }
            return result;
        }
    }
}
