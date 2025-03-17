namespace ImageResizer.Domain.Interfaces.Services
{
    public interface IThumbnailBlobService
    {
        public Task<string> UploadAsync(Stream fileStream, string blobName);

        public Task<Stream> DownloadAsync(Uri blobUri);

        public Task DeleteAsync(string blobName);
    }
}
