using AutoMapper;
using DAM.DAM.Api.DTOs.Requests.File;
using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.Api.DTOs.Requests.Permission;
using DAM.DAM.Api.DTOs.Responses.File;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using static DAM.DAM.BLL.Extensions.PagingExtension;
using File = DAM.DAM.DAL.Entities.File;

namespace DAM.DAM.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly IBaseRepository<File> _filesRepository;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public FileService(IBaseRepository<File> fileRepository, IPermissionService permissionService, IMapper mapper)
        {
            _filesRepository = fileRepository;
            _permissionService = permissionService;
            _mapper = mapper;
        }

        public async Task<FileResponse> AddFileAsync(FileRequest request)
        {
            if (!string.IsNullOrEmpty(request.FolderId) && !string.IsNullOrEmpty(request.DriveId))
            {
                throw new ArgumentException("Only one of ParentFolderId or DriveId should be provided, not both.");
            }

            await CheckPermissionAsync(request.UserId, request.Id,
                PermissionRoleEnum.Contributor, "You do not have permission to add folders.");

            var file = _mapper.Map<File>(request);
            await _filesRepository.AddAsync(file);

            return _mapper.Map<FileResponse>(file);
        }

        public async Task<FileResponse> UpdateFileAsync(FileRequest request)
        {
            if (!string.IsNullOrEmpty(request.FolderId) && !string.IsNullOrEmpty(request.DriveId))
            {
                throw new ArgumentException("Only one of ParentFolderId or DriveId should be provided, not both.");
            }

            var existingFile = await _filesRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("File not found.");

            await CheckPermissionAsync(request.UserId, request.Id,
                PermissionRoleEnum.Contributor, "You do not have permission to update folders.");

            _mapper.Map(request, existingFile);
            await _filesRepository.UpdateAsync(existingFile);

            return _mapper.Map<FileResponse>(existingFile);
        }

        public async Task DeleteFileAsync(string id, string userId)
        {
            var file = await _filesRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("File not found.");

            await CheckPermissionAsync(userId, id,
                PermissionRoleEnum.Contributor, "You do not have permission to delete folders.");

            await _filesRepository.DeleteAsync(file);
        }

        public async Task<FileResponse> GetFileByIdAsync(string id)
        {
            var file = await _filesRepository.GetByIdAsync(id);

            if (file == null)
            {
                throw new KeyNotFoundException("File not found.");
            }

            return _mapper.Map<FileResponse>(file); ;
        }

        public async Task<PagedResult<FileResponse>> GetAllFilesAsync(FolderGetAllRequest request)
        {
            var files = await _filesRepository.GetAllAsync(f => EF.Functions.Like(f.Name, $"%{request.SearchQuery}%"));

            var pagedResult = await files.ToPagedResult(request.PageIndex, request.PageSize);

            var mappedItems = _mapper.Map<List<FileResponse>>(pagedResult.Items);

            return new PagedResult<FileResponse>
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
