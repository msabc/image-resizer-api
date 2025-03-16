using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

namespace ImageResizer.Application.Services.ImageEncoding
{
    public class ImageEncodingService : IImageEncodingService
    {
        public IImageEncoder GetImageEncoder(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException("Unable to determine the file extension.");

            if (!extension.StartsWith("."))
                throw new ArgumentException("The extension should is in an invalid format.");

            return extension switch
            {
                ".jpg" or ".jpeg" => new JpegEncoder(),
                ".png" => new PngEncoder(),
                ".bmp" => new BmpEncoder(),
                ".tiff" => new TiffEncoder(),
                ".webp" => new WebpEncoder(),
                ".gif" => new GifEncoder(),
                _ => throw new NotSupportedException($"Unsupported extension {extension} provided."),
            };
        }
    }
}
