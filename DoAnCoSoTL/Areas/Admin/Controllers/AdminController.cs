using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSoTL.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
