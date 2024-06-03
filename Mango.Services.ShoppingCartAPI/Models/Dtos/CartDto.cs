namespace Mango.Services.ShoppingCartAPI.Models.Dtos
{
    public sealed class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
