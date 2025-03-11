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
        private readonly BlobContainerClient _blobContainerClient;

        public BlobService(IOptions<ResizerSettings> resizerOptions)
        {
            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ConnectionString))
                throw new ArgumentException($"Missing configuration setting: {nameof(BlobSettingsElement.ConnectionString)}.");

            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ImagesContainerName))
                throw new ArgumentException($"Missing configuration setting {nameof(BlobSettingsElement.ImagesContainerName)}.");

            _blobContainerClient = new BlobContainerClient(
                resizerOptions.Value.BlobSettings.ConnectionString, 
                resizerOptions.Value.BlobSettings.ImagesContainerName
            );

            _blobContainerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            var download = await blobClient.DownloadAsync();

            return download.Value.Content;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
