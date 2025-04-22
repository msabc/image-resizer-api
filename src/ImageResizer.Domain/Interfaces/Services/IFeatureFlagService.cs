using ImageResizer.Domain.Models.Enums;

namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IFeatureFlagService
    {
        bool IsFeatureEnabled(Features feature);
    }
}
