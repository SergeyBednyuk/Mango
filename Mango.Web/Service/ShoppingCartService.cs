using Mango.Web.Models;
using Mango.Web.Models.ShoppingCart;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public sealed class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;
        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = SD.ShoppingCartAPIBase + "/api/shoppingcart/applycoupon"
            });
        }

        public async Task<ResponseDto?> GetShoppingCartByUserIdAsync(int userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/shoppingcart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveCouponAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = SD.ShoppingCartAPIBase + "/api/shoppingcart/removecoupon"
            });
        }

        public async Task<ResponseDto?> RemoveFromShoppingCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = SD.ShoppingCartAPIBase + "/api/shoppingcart/deletecartdetails" + cartDetailsId
            });
        }

        public async Task<ResponseDto?> UpsertShoppingCartAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = SD.ShoppingCartAPIBase + "/api/shoppingcart/cartupsert"
            });
        }
    }
}
