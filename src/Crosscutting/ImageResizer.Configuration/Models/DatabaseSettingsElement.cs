namespace ImageResizer.Configuration.Models
{
    public record DatabaseSettingsElement
    {
        public required string ConnectionString { get; set; }
    }
}
