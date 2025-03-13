namespace ImageResizer.Application.Models.Response.Image
{
    public record FilterImageDto
    {
        public Guid Id { get; set; }

        public required string Uri { get; set; }

        public required string FileName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
