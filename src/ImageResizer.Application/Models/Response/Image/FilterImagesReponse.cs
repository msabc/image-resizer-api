namespace ImageResizer.Application.Models.Response.Image
{
    public record FilterImagesReponse
    {
        public required List<FilterImageDto> Images { get; set; }
    }
}
