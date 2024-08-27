using DAM.DAM.Api.DTOs.Folder;
using DAM.DAM.BLL.Extensions;
using static DAM.DAM.BLL.Extensions.PagingExtension;

namespace DAM.DAM.BLL.Interfaces
{
    public interface IFolderService
    {
        Task<FolderResponse> AddFolderAsync(FolderRequest request);
        Task<FolderResponse> UpdateFolderAsync(FolderRequest request);
        Task DeleteFolderAsync(FolderDeleteRequest request);
        Task<FolderDetailsResponse> GetFolderByIdAsync(string id);
        Task<PagedResult<FolderResponse>> GetAllFoldersAsync(FolderGetAllRequest request);
    }
}
