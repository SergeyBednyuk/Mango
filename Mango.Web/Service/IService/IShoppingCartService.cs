using Mango.Web.Models;
using Mango.Web.Models.ShoppingCart;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService
    {
        Task<ResponseDto?> GetShoppingCartByUserIdAsync(int userId);
        Task<ResponseDto?> UpsertShoppingCartAsync(CartDto cart);
        Task<ResponseDto?> RemoveFromShoppingCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cart);
        Task<ResponseDto?> RemoveCouponAsync(CartDto cart);
    }
}
