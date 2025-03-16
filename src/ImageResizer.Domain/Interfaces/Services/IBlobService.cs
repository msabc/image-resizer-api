namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IBlobService
    {
        public Task<string> UploadAsync(Stream fileStream, string blobName);

        public Task<Stream> DownloadAsync(string blobName);

        public Task<Stream> DownloadAsync(Uri blobUri);

        public Task DeleteAsync(string blobName);
    }
}
