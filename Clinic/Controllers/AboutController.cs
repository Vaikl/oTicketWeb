using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class AboutController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}