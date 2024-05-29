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

        public Task<ResponseDto?> AddProductAsync(CreateProductRequestDto product)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> GetProductByIdAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto?> GetProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = product,
                Url = SD.CouponAPIBase + "/api/product"
            });
        }
    }
}
