using ImageResizer.Application.Attributes.Validation;

namespace ImageResizer.Application.Models.Request.Image
{
    public record FilterImagesRequest
    {
        public DateTime? CreatedBefore { get; set; }

        public DateTime? CreatedAfter { get; set; }

        [MaxNumberOfItems(20)]
        public required List<string> FileExtensions { get; set; }

        public string? Name { get; set; }
    }
}
