namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IBlobService
    {
        public Task<string> UploadFileAsync(Stream fileStream, string fileName);

        public Task<Stream> DownloadFileAsync(string fileName);

        public Task DeleteFileAsync(string fileName);
    }
}
