namespace ImageResizer.Domain.Models.Queues
{
    public record ThumbnailProcessorMessage
    {
        public Guid FileUploadId { get; set; }

        public required string FileName { get; set; }

        public required string BlobUri { get; set; }

        public int Height { get; set; }
    }
}
