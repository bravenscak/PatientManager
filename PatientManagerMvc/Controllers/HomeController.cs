using Microsoft.AspNetCore.Mvc;

namespace PatientManagerMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
