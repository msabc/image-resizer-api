using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Models;
using ImageResizer.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ImageResizer.Infrastructure.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _imagesBlobContainerClient;

        public BlobService(IOptions<ResizerSettings> resizerOptions)
        {
            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ConnectionString))
                throw new ArgumentException($"Missing configuration setting: {nameof(BlobSettingsElement.ConnectionString)}.");

            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ImagesContainerName))
                throw new ArgumentException($"Missing configuration setting {nameof(BlobSettingsElement.ImagesContainerName)}.");

            _imagesBlobContainerClient = new BlobContainerClient(
                resizerOptions.Value.BlobSettings.ConnectionString, 
                resizerOptions.Value.BlobSettings.ImagesContainerName
            );

            _imagesBlobContainerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadAsync(Stream fileStream, string blobName)
        {
            var blobClient = _imagesBlobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadAsync(string fileName)
        {
            var blobClient = _imagesBlobContainerClient.GetBlobClient(fileName);

            var downloadInfo = await blobClient.DownloadAsync();

            return downloadInfo.Value.Content;
        }

        public async Task<Stream> DownloadAsync(Uri blobUri)
        {
            var blobClient = new BlobClient(blobUri);

            var downloadInfo = await blobClient.DownloadAsync();

            return downloadInfo.Value.Content;
        }

        public async Task DeleteAsync(string blobName)
        {
            var blobClient = _imagesBlobContainerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
