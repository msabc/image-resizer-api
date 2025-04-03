namespace ImageResizer.Configuration.Models
{
    public record BlobSettingsElement
    {
        public required string ImagesContainerName { get; set; }

        public required string ThumbnailsContainerName { get; set; }
    }
}
