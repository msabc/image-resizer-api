using System.ComponentModel.DataAnnotations;

namespace ImageResizer.Application.Models.Request.Image
{
    public record ResizeRequest
    {
        public Guid Id { get; set; }

        [Range(10, 4000, ErrorMessage = "The number must be between 10 and 4000.")]
        public int Height { get; set; }
    }
}
