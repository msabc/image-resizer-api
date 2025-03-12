namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IBlobService
    {
        public Task<string> UploadAsync(Stream fileStream, string fileName);

        public Task<Stream> DownloadAsync(string fileName);

        public Task DeleteAsync(string fileName);
    }
}
