using System.ComponentModel.DataAnnotations;

namespace ImageResizer.Application.Models.Request.Auth
{
    public record SignUpRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 12, ErrorMessage = "Password must be between 12 and 50 characters long.")]
        public required string Password { get; set; }
    }
}
