using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            var response = await _couponService.GetAllCouponsAsync();
            if(response != null && response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
                return View(result);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(new List<CouponDto>());
        }

        public async Task<IActionResult> CouponCreate()
        {
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.AddCouponAsync(model);
                if (response != null && response.IsSuccess)
                {
                    var result = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                    TempData["success"] = "Coupon create succesfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
             
            return View(model);
        }


        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.GetCouponByIdAsync(couponId);
            if (response != null && response.IsSuccess)
            {
				var result = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                return View(result);
			}

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {
            var response = await _couponService.DeleteCouponAsync(model.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon delete succesfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }
    }
}
