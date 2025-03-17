namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IImageProcessorService
    {
        Task<(int height, int width)> LoadAsync(Stream stream);

        Task ResizeAsync(Stream input, Stream output, string extension, int height);
    }
}
