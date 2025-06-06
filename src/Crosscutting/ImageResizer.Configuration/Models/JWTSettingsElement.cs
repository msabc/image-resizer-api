﻿namespace ImageResizer.Configuration.Models
{
    public record JWTSettingsElement
    {
        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required string IssuerSigningKey { get; set; }
    }
}