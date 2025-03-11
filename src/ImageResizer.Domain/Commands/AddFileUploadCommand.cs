namespace ImageResizer.Domain.Commands
{
    public record AddFileUploadCommand
    {
        public required string Url { get; set; }

        public required string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedByUserId { get; set; }
    }
}