namespace ImageResizer.Domain.Commands.Thumbnail
{
    public class AddThumbnailCommand
    {
        public Guid FileUploadId { get; set; }

        public required string Uri { get; set; }

        public int Height { get; set; }
    }
}
