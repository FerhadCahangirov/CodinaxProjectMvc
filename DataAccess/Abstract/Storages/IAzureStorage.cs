namespace CodinaxProjectMvc.DataAccess.Abstract.Storages;

public interface IAzureStorage
{
    Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files);
    Task DeleteAsync(string pathOrContainerName, string fileName);

    List<string> GetFiles(string pathOrContainerName);
    bool HasFile(string pathOrContainerName, string fileName);
}
