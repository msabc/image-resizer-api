using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Models;
using ImageResizer.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ImageResizer.Infrastructure.Services
{
    public class ImageBlobService : IImageBlobService
    {
        private readonly BlobContainerClient _imagesBlobContainerClient;
        private readonly DateTimeOffset _sasTokenValidityPeriod;
        private readonly string _imagesContainerAccountKey;

        public ImageBlobService(
            IOptions<ConnectionStrings> connectionStringsOptions,
            IOptions<ResizerSettings> resizerOptions)
        {
            if (string.IsNullOrWhiteSpace(connectionStringsOptions.Value.BlobStorageConnectionString))
                throw new ArgumentException($"Missing configuration setting: {nameof(ConnectionStrings.BlobStorageConnectionString)}.");

            if (string.IsNullOrWhiteSpace(resizerOptions.Value.BlobSettings.ImagesContainerName))
                throw new ArgumentException($"Missing configuration setting {nameof(BlobSettingsElement.ImagesContainerName)}.");

            _imagesBlobContainerClient = new BlobContainerClient(
                connectionStringsOptions.Value.BlobStorageConnectionString,
                resizerOptions.Value.BlobSettings.ImagesContainerName
            );

            _imagesBlobContainerClient.CreateIfNotExists(PublicAccessType.Blob);

            _imagesContainerAccountKey = resizerOptions.Value.BlobSettings.ImagesContainerAccountKey;
            _sasTokenValidityPeriod = DateTimeOffset.UtcNow.AddMinutes(10);
        }

        public async Task<string> UploadAsync(Stream fileStream, string blobName)
        {
            var blobClient = _imagesBlobContainerClient.GetBlobClient(blobName);

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
            var blobClient = _imagesBlobContainerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        public string GenerateSasToken(string blobName, string originalFileName)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _imagesBlobContainerClient.Name,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = _sasTokenValidityPeriod
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            sasBuilder.ContentDisposition = $"attachment; filename={originalFileName}";

            return sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(
                _imagesBlobContainerClient.AccountName,
                _imagesContainerAccountKey)
            ).ToString();
        }
    }
}
