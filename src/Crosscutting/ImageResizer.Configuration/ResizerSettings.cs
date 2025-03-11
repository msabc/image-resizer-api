using ImageResizer.Configuration.Models;

namespace ImageResizer.Configuration
{
    public class ResizerSettings
    {
        public const string ApplicationName = "ImageResizer.Api";

        public JWTSettingsElement JWTSettings { get; set; }

        public BlobSettingsElement BlobSettings { get; set; }

        public DatabaseSettingsElement DatabaseSettings { get; set; }

        public ImageSettingsElement ImageSettings { get; set; }
    }
}
