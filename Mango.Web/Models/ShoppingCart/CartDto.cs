namespace Mango.Web.Models.ShoppingCart
{
    public sealed record class CartDto
    {
        public CartHeaderDto CartHeader { get; init; }
        public IEnumerable<CartDetailsDto> CartDetails { get; init; }
    }
}
