namespace ImageResizer.Configuration.Models
{
    public record QueueSettingsElement
    {
        public required string ConnectionString { get; set; }

        public required string ThumbnailsQueueName { get; set; }
    }
}
