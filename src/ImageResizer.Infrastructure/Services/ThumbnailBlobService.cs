using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Models;
using ImageResizer.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ImageResizer.Infrastructure.Services
{
    public class ThumbnailBlobService : IThumbnailBlobService
    {
        private readonly BlobContainerClient _thumbnailsBlobContainerClient;

        public ThumbnailBlobService(
            IOptions<ConnectionStrings> connectionStringsOptions, 
            IOptions<ResizerSettings> resizerOptions)
        {
            if (string.IsNullOrWhiteSpace(connectionStringsOptions.Value.BlobStorageConnectionString))
                throw new ArgumentException($"Missing configuration setting: {nameof(ConnectionStrings.BlobStorageConnectionString)}.");

            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ThumbnailsContainerName))
                throw new ArgumentException($"Missing configuration setting {nameof(BlobSettingsElement.ThumbnailsContainerName)}.");

            _thumbnailsBlobContainerClient = new BlobContainerClient(
                connectionStringsOptions.Value.BlobStorageConnectionString, 
                resizerOptions.Value.BlobSettings.ThumbnailsContainerName
            );

            _thumbnailsBlobContainerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadAsync(Stream fileStream, string blobName)
        {
            var blobClient = _thumbnailsBlobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadAsync(Uri blobUri)
        {
            var blobClient = new BlobClient(blobUri);

            var downloadInfo = await blobClient.DownloadAsync();

            return downloadInfo.Value.Content;
        }

        public async Task DeleteAsync(string blobName)
        {
            var blobClient = _thumbnailsBlobContainerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
