using Mango.Web.Models;
using Mango.Web.Models.Product;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public sealed class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AddProductAsync(CreateProductRequestDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = product,
                Url = SD.ProductAPIBase + "/api/product/"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/product/" + productId
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product/" + productId
            });
        }

        public async Task<ResponseDto?> GetProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = product,
                Url = SD.ProductAPIBase + "/api/product"
            });
        }
    }
}
