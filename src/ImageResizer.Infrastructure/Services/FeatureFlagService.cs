using ImageResizer.Configuration;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace ImageResizer.Infrastructure.Services
{
    public class FeatureFlagService(IOptionsSnapshot<ResizerSettings> resizerOptionsSnapshot) : IFeatureFlagService
    {
        public bool IsFeatureEnabled(Features feature)
        {
            return feature switch
            {
                Features.AutomaticThumbnailCreationEnabled => resizerOptionsSnapshot.Value.FeatureSettings.AutomaticThumbnailCreationEnabled,
                _ => false
            };
        }
    }
}
