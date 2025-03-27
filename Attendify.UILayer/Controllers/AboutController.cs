using Microsoft.AspNetCore.Mvc;

namespace Attendify.UILayer.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
