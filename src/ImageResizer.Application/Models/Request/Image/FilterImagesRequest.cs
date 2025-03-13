namespace ImageResizer.Application.Models.Request.Image
{
    public record FilterImagesRequest
    {
        public DateTime? CreatedBefore { get; set; }

        public DateTime? CreatedAfter { get; set; }

        public required List<string> FileExtensions { get; set; }

        public string? Name { get; set; }
    }
}
