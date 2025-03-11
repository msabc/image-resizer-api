namespace ImageResizer.Domain.Models.Tables
{
    public class FileUpload
    {
        public Guid Id { get; set; }

        public required string Url { get; set; }

        public required string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedByUserId { get; set; }
    }
}
