namespace ImageResizer.Application.Models.Response.Image
{
    public record GenerateSasUrlResponse
    {
        public required string Url { get; set; }
    }
}
