namespace Mango.Web.Models
{
    public record LoginRequestDto
    {
        public string UserName { get; init; }
        public string UserPassword { get; init; }
    }
}
