using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductIndex()
        {
            return View();
        }
    }
}
