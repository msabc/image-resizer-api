namespace ImageResizer.Application.Models.Response.Image
{
    public record UploadImageResponse
    {
        public required string Uri { get; set; }

        public required Guid Id { get; set; }
    }
}
