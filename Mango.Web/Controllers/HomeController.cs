using Mango.Web.Models;
using Mango.Web.Models.Product;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
			var response = await _productService.GetProductsAsync();
			if (response != null && response.IsSuccess)
			{
				var result = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
				return View(result);
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(new List<ProductDto>());
		}

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());

                return View(product);
            }

            return NotFound();
        }

        //[Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
