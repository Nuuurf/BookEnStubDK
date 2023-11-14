using Microsoft.AspNetCore.Mvc;

namespace WebApplicationMVC.Controllers {
    public class BookingController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
