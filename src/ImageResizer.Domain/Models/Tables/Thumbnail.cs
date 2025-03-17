namespace ImageResizer.Domain.Models.Tables
{
    public class Thumbnail
    {
        public Guid Id { get; set; }

        public Guid FileUploadId { get; set; }

        public required string Uri { get; set; }

        public int Height { get; set; }
    }
}
