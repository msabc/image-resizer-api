namespace ImageResizer.Application.Models.Request.Thumbnail
{
    public record SaveThumbnailRequest
    {
        public Guid FileUploadId { get; set; }

        public required string FileName { get; set; }

        public required string Uri { get; set; }

        public required int Height { get; set; }
    }
}
