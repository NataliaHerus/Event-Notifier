using EventNotifier.Services.Interfaces;
using Azure.Storage.Blobs;

namespace EventNotifier.Services
{
    public class AzureService : IAzureService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _client;
        public AzureService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _client = _blobServiceClient.GetBlobContainerClient("eventnotifiercontainer");
        }

        public async Task UploadBlobAsync(string blobName, IFormFile file)
        {
            try
            {
                var blobClient = _client.GetBlobClient(blobName);

                if (await blobClient.ExistsAsync())
                    throw new InvalidOperationException($"Blob {blobName} already exists. Container: {_client}.");

                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> ExistsBlob(string blobName)
        {
            var blobClient = _client.GetBlobClient(blobName);

            return await blobClient.ExistsAsync();
        }

        public async Task<byte[]?> DownloadBlobAsync(string blobName)
        {
            try
            {
                var blobClient = _client.GetBlobClient(blobName);

                if ((await blobClient.ExistsAsync()))
                {

                    await using var stream = new MemoryStream();
                    await blobClient.DownloadToAsync(stream);
                    return stream.ToArray();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    return;

                var blobs = _client.GetBlobsByHierarchy(prefix: path);
                if (!blobs.Any()) return;

                foreach (var blob in blobs)
                {
                    if (blob.IsBlob)
                    {
                        await _client.DeleteBlobAsync(blob.Blob.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public bool ExistsFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var blobs = _client.GetBlobsByHierarchy(prefix: path);
            return blobs.Any();
        }
    }
}
