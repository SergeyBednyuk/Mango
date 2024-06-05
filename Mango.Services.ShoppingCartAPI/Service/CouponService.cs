using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public sealed class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto?> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"api/coupon/GetByCode/" + couponCode);
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDes = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDes != null && responseDes.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDes.Result));
            }
            return null;
        }
    }
}
