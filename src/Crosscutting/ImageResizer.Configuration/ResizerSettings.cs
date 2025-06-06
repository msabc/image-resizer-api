﻿using ImageResizer.Configuration.Models;

namespace ImageResizer.Configuration
{
    public class ResizerSettings
    {
        public JWTSettingsElement JWTSettings { get; set; }

        public BlobSettingsElement BlobSettings { get; set; }

        public QueueSettingsElement QueueSettings { get; set; }

        public ImageSettingsElement ImageSettings { get; set; }

        public FeatureSettingsElement FeatureSettings { get; set; }
    }
}
