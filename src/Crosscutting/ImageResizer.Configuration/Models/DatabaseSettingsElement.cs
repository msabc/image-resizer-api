namespace ImageResizer.Configuration.Models
{
    public record DatabaseSettingsElement
    {
        public required string ResizerConnectionString { get; set; }
    }
}
