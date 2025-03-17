namespace ImageResizer.Configuration.Models
{
    public record ImageSettingsElement
    {
        public int MaxFileSizeInMB { get; set; }

        public required string[] SupportedExtensions { get; set; }

        public required int DefaultThumbnailHeightInPixels { get; set; }
    }
}
