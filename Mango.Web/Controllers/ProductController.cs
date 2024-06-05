using Mango.Web.Models.Product;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
	public sealed class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> ProductIndex()
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

		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductCreate(CreateProductRequestDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.AddProductAsync(model);
				if (response != null && response.IsSuccess)
				{
                    TempData["success"] = "Product create succesfully";
                    return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}

			return View(model);
		}

		public async Task<IActionResult> ProductDelete(int productId)
		{
			var response = await _productService.GetProductByIdAsync(productId);
			if (response != null && response.IsSuccess)
			{
				var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());

				return View(product);
			}

			return NotFound();
		}

		[HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
		{
			var response = await _productService.DeleteProductAsync(model.ProductId);
			if(response != null && response.IsSuccess)
			{
                TempData["success"] = "Product delete succesfully";
                return RedirectToAction(nameof(ProductIndex));
            }
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(model);
		}

		public async Task<IActionResult> ProductEdit(int productId)
		{
			var response = await _productService.GetProductByIdAsync(productId);
			if (response != null && response.IsSuccess)
			{
				var prodoct = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());

				return View(prodoct);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.UpdateProductAsync(model);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product updated succesfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}

			return View(model);
		}

	}
}
