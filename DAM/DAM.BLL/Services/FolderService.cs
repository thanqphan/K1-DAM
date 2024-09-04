using AutoMapper;
using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.Api.DTOs.Requests.Permission;
using DAM.DAM.Api.DTOs.Responses.File;
using DAM.DAM.Api.DTOs.Responses.Folder;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Entities;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using static DAM.DAM.BLL.Extensions.PagingExtension;
using File = DAM.DAM.DAL.Entities.File;

namespace DAM.DAM.BLL.Services
{
    public class FolderService : IFolderService
    {
        private readonly IBaseRepository<Folder> _foldersRepository;
        private readonly IBaseRepository<File> _filesRepository;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public FolderService(IBaseRepository<Folder> folderRepository, IPermissionService permissionService, IMapper mapper
            , IBaseRepository<File> fileRepository)
        {
            _foldersRepository = folderRepository;
            _permissionService = permissionService;
            _mapper = mapper;
            _filesRepository = fileRepository;
        }

        public async Task<FolderResponse> AddFolderAsync(FolderRequest request)
        {
            if (!string.IsNullOrEmpty(request.ParentId) && !string.IsNullOrEmpty(request.DriveId))
            {
                throw new ArgumentException("Only one of ParentFolderId or DriveId should be provided, not both.");
            }

            await CheckPermissionAsync(request.UserId, request.ParentId ?? request.DriveId, 
                PermissionRoleEnum.Contributor, "You do not have permission to add folders.");

            var folder = _mapper.Map<Folder>(request);
            await _foldersRepository.AddAsync(folder);

            return _mapper.Map<FolderResponse>(folder);
        }

        public async Task<FolderResponse> UpdateFolderAsync(FolderRequest request)
        {
            if (!string.IsNullOrEmpty(request.ParentId) && !string.IsNullOrEmpty(request.DriveId))
            {
                throw new ArgumentException("Only one of ParentFolderId or DriveId should be provided, not both.");
            }

            var existingFolder = await _foldersRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("Folder not found.");

            await CheckPermissionAsync(request.UserId, request.Id, 
                PermissionRoleEnum.Contributor, "You do not have permission to update folders.");

            _mapper.Map(request, existingFolder);
            await _foldersRepository.UpdateAsync(existingFolder);

            return _mapper.Map<FolderResponse>(existingFolder);
        }

        public async Task DeleteFolderAsync(string id, string userId)
        {
            var folder = await _foldersRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Folder not found.");

            await CheckPermissionAsync(userId, id,
                PermissionRoleEnum.Contributor, "You do not have permission to delete folders.");

            await _foldersRepository.DeleteAsync(folder);
        }

        public async Task<FolderDetailsResponse> GetFolderByIdAsync(string id)
        {
            var folder = await _foldersRepository.GetByIdAsync(id);

            if (folder == null)
            {
                throw new KeyNotFoundException("Folder not found.");
            }

            var folderResponse = _mapper.Map<FolderDetailsResponse>(folder);

            folderResponse.SubFolders = _mapper.Map<List<FolderResponse>>(
                await _foldersRepository.GetAllAsync(f => f.ParentId == id));

            folderResponse.Files = _mapper.Map<List<FileResponse>>(
                await _filesRepository.GetAllAsync(f => f.FolderId == id));

            return folderResponse;
        }

        public async Task<PagedResult<FolderResponse>> GetAllFoldersAsync(FolderGetAllRequest request)
        {
            var folders = await _foldersRepository.GetAllAsync(f => EF.Functions.Like(f.Name, $"%{request.SearchQuery}%"));

            var pagedResult = await folders.ToPagedResult(request.PageIndex, request.PageSize);

            var mappedItems = _mapper.Map<List<FolderResponse>>(pagedResult.Items);

            return new PagedResult<FolderResponse>
            {
                Items = mappedItems,
                Total = pagedResult.Total,
                PageSize = pagedResult.PageSize,
                Skipped = pagedResult.Skipped,
            };
        }

        private async Task CheckPermissionAsync(string userId, string entityId, PermissionRoleEnum requiredRole, string errorMessage)
        {
            if (!await _permissionService.HasPermissionAsync(new PermissionRequest
            {
                UserId = userId,
                EntityId = entityId,
                Role = requiredRole
            }))
            {
                throw new UnauthorizedAccessException(errorMessage);
            }
        }

    }
}
