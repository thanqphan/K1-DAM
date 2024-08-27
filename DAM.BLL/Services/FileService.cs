using AutoMapper;
using DAM.DAM.Api.DTOs.File;
using DAM.DAM.Api.DTOs.Permission;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
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
            if (!await _permissionService.HasPermissionAsync(new PermissionRequest
            {
                UserId = request.UserId,
                EntityId = request.FolderId ?? request.DriveId,
                Role = PermissionRoleEnum.Contributor
            }))
            {
                throw new UnauthorizedAccessException("You do not have permission to add files.");
            }

            var file = _mapper.Map<File>(request);
            await _filesRepository.AddAsync(file);

            return _mapper.Map<FileResponse>(file);
        }

        public async Task<FileResponse> UpdateFileAsync(FileRequest request)
        {
            var existingFile = await _filesRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("File not found.");

            if (!await _permissionService.HasPermissionAsync(new PermissionRequest
            {
                UserId = request.UserId,
                EntityId = existingFile.Id,
                Role = PermissionRoleEnum.Contributor
            }))
            {
                throw new UnauthorizedAccessException("You do not have permission to update this file.");
            }

            _mapper.Map(request, existingFile);
            await _filesRepository.UpdateAsync(existingFile);

            return _mapper.Map<FileResponse>(existingFile);
        }

        public async Task DeleteFileAsync(FileRequest request)
        {
            var file = await _filesRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("File not found.");

            if (!await _permissionService.HasPermissionAsync(new PermissionRequest
            {
                UserId = request.UserId,
                EntityId = file.Id,
                Role = PermissionRoleEnum.Contributor
            }))
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this file.");
            }

            await _filesRepository.DeleteAsync(file);
        }
    }
}
