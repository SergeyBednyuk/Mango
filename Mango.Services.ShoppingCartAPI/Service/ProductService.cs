using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public sealed class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/" + id);
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDes = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDes != null && responseDes.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDes.Result));
            }
            return null;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDes = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDes != null && responseDes.IsSuccess) 
            {
                return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDes.Result));
            }
            return new List<ProductDto>();
        }
    }
}
