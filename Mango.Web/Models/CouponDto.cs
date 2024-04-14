namespace Mango.Web.Models
{
    public sealed record CouponDto
    {
        public int CouponId { get; init; }
        public string CouponCode { get; init; }
        public double DiscountAmount { get; init; }
        public int MinAmount { get; init; }
    }
}
