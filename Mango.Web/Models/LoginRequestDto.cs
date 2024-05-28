using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public record LoginRequestDto
    {
        [Required]
        public string UserName { get; init; }
        [Required]
        public string UserPassword { get; init; }
    }
}
