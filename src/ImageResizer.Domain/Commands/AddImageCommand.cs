namespace ImageResizer.Domain.Commands
{
    public record AddImageCommand
    {
        public required string Url { get; set; }
    }
}