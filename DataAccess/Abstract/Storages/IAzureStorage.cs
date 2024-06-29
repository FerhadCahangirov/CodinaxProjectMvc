using Azure.Storage.Blobs.Models;

namespace CodinaxProjectMvc.DataAccess.Abstract.Storages;

public interface IAzureStorage
{
    Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files);
    Task<(string fileName, string pathOrContainerName)> UploadAsync(string containerName, IFormFile file);
    Task DeleteAsync(string pathOrContainerName, string fileName);

    List<string> GetFiles(string pathOrContainerName);

    BlobItem GetFile(string pathOrContainerName, string fileName);

    Task BitrateAsync(string fileName, string containerName);

    bool HasFile(string pathOrContainerName, string fileName);
}
