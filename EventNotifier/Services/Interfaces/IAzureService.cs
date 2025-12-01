namespace EventNotifier.Services.Interfaces
{
    public interface IAzureService
    {
        Task<bool> ExistsBlob(string blobName);
        Task UploadBlobAsync(string blobName, IFormFile file);
        Task<byte[]?> DownloadBlobAsync(string blobName);
        Task DeleteFile(string path);
        bool ExistsFile(string path);
    }
}
