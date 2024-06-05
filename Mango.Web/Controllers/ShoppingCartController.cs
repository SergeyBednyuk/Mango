using Mango.Web.Models.ShoppingCart;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public sealed class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            var shippingCart = await LoadShoppingCartByUserAsync();

            return View(shippingCart);
        }

        private async Task<CartDto> LoadShoppingCartByUserAsync()
        {
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault();
            if (userId != null)
            {
                var response = await _shoppingCartService.GetShoppingCartByUserIdAsync(Convert.ToInt32(userId.Value));
                if (response.IsSuccess && response.Result != null)
                {
                    var shoppingCart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                    return shoppingCart;
                }
            }
            return new CartDto();
        }
    }
}
