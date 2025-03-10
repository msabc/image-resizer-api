namespace ImageResizer.Domain.Models.Tables
{
    public class Image
    {
        public Guid Id { get; set; }

        public required string Url { get; set; }
    }
}
