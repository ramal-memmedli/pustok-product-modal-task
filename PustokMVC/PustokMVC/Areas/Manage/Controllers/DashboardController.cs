using Microsoft.AspNetCore.Mvc;

namespace PustokMVC.Areas.Manage.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
