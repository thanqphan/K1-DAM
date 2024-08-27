using DAM.DAM.Api.DTOs.Requests.File;
using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.Api.DTOs.Responses.File;
using static DAM.DAM.BLL.Extensions.PagingExtension;

namespace DAM.DAM.BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileResponse> AddFileAsync(FileRequest request);
        Task<FileResponse> UpdateFileAsync(FileRequest request);
        Task DeleteFileAsync(FolderDeleteRequest request);
        Task<FileResponse> GetFileByIdAsync(string id);
        Task<PagedResult<FileResponse>> GetAllFilesAsync(FolderGetAllRequest request);
    }
}
