using DAM.DAM.Api.DTOs.File;

namespace DAM.DAM.BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileResponse> AddFileAsync(FileRequest request);
        Task<FileResponse> UpdateFileAsync(FileRequest request);
        Task DeleteFileAsync(FileRequest request);
    }
}
