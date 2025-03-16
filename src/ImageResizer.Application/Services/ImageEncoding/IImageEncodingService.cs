using SixLabors.ImageSharp.Formats;

namespace ImageResizer.Application.Services.ImageEncoding
{
    public interface IImageEncodingService
    {
        public IImageEncoder GetImageEncoder(string extension);
    }
}
