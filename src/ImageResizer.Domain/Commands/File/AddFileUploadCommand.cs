namespace ImageResizer.Domain.Commands.File
{
    public record AddFileUploadCommand
    {
        public required string Uri { get; set; }

        public required string Name { get; set; }

        public required Guid CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Height { get; set; }
    }
}