namespace ImageResizer.Configuration
{
    public record ConnectionStrings
    {
        public string ResizerDatabaseConnectionString { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public string QueueStorageConnectionString { get; set; }
    }
}
