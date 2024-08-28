using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.Api.DTOs.Responses.Folder;
using DAM.DAM.BLL.Extensions;
using static DAM.DAM.BLL.Extensions.PagingExtension;

namespace DAM.DAM.BLL.Interfaces
{
    public interface IFolderService
    {
        Task<FolderResponse> AddFolderAsync(FolderRequest request);
        Task<FolderResponse> UpdateFolderAsync(FolderRequest request);
        Task DeleteFolderAsync(string id, string userId);
        Task<FolderDetailsResponse> GetFolderByIdAsync(string id);
        Task<PagedResult<FolderResponse>> GetAllFoldersAsync(FolderGetAllRequest request);
    }
}
