using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.Product
{
    public sealed record class ProductDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; }
        public double Price { get; init; }
        public string Description { get; init; }
        public string CategoryName { get; init; }
        public string ImageUrl { get; init; }
        [Range(1, 100)]
        public int Count { get; init; } = 1;
    }
}
