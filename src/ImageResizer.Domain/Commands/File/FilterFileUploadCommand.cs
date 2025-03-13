namespace ImageResizer.Domain.Commands.File
{
    public record FilterFileUploadCommand
    {
        public Guid CreatedByUserId { get; set; }

        public DateTime? CreatedBefore { get; set; }

        public DateTime? CreatedAfter { get; set; }

        public required List<string> FileExtensions { get; set; }

        public string? Name { get; set; }
    }
}