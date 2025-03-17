using ImageResizer.Domain.Interfaces.Services;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer.Infrastructure.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        public async Task<(int height, int width)> LoadAsync(Stream stream)
        {
            var image = await SixLabors.ImageSharp.Image.LoadAsync(stream);

            return (image.Height, image.Width);
        }

        public async Task ResizeAsync(Stream input, Stream output, string extension, int height)
        {
            using var image = await SixLabors.ImageSharp.Image.LoadAsync(input);

            float aspectRatio = (float)image.Width / image.Height;
            int newWidth = (int)(height * aspectRatio);

            image.Mutate(x => x.Resize(newWidth, height));

            await image.SaveAsync(output, GetImageEncoder(extension));
        }

        private static IImageEncoder GetImageEncoder(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension));

            if (!extension.StartsWith('.'))
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
