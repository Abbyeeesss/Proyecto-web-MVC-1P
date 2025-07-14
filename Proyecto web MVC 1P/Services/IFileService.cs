namespace Proyecto_web_MVC_1P.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task<bool> DeleteFileAsync(string filePath);
        bool IsValidImageFile(IFormFile file);
        string GetFileUrl(string fileName, string folder);
    }
}