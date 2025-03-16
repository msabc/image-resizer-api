namespace ImageResizer.Domain.Models.Tables
{
    public class FileUpload
    {
        public Guid Id { get; set; }

        public required string Uri { get; set; }

        public required string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedByUserId { get; set; }

        public int Height { get; set; }

        public string? ResizedUri { get; set; }
    }
}
