using Mango.Web.Models;
using Mango.Web.Models.Product;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(string productId);
        Task<ResponseDto?> AddProductAsync(CreateProductRequestDto product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto product);
        Task<ResponseDto> DeleteProductAsync(string productId);
    }
}
