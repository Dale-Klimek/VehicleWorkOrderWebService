namespace VehicleWorkOrder.MobileAppService.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Interfaces;
    using Microsoft.Extensions.Options;

    public class BlobStorageService : IBlobStorageService
    {
        private BlobServiceClient _client;
        private const string containerName = "Pictures";


        public BlobStorageService(IOptions<ConfigurationService> options)
        {
            _client = new BlobServiceClient(options.Value.BlobService);
        }
        public async Task Initialize()
        {
            // Can't call this repeatedly because it throws an error if it exists
            var container = await _client.CreateBlobContainerAsync(containerName);
        }

        public async Task<Stream> Download(string name)
        {
            var container = _client.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(name);
            return (await blob.DownloadAsync()).Value.Content;
        }

        public async Task<Guid> Upload(Stream stream)
        {
            var name = Guid.NewGuid();
            var container = _client.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(name.ToString());
            await blob.UploadAsync(stream);
            return name;
        }
    }
}
