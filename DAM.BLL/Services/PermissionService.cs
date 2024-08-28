using DAM.DAM.Api.DTOs.Requests.Permission;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Entities;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using File = DAM.DAM.DAL.Entities.File;

namespace DAM.DAM.BLL.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IBaseRepository<Permission> _permissionsRepository;
        private readonly IBaseRepository<Folder> _foldersRepository;
        private readonly IBaseRepository<File> _filesRepository;
        public PermissionService(IBaseRepository<Permission> permissionRepository,
            IBaseRepository<Folder> folderRepository,
            IBaseRepository<File> fileRepository
            )
        {
            _foldersRepository = folderRepository;
            _permissionsRepository = permissionRepository;
            _filesRepository = fileRepository;
        }
        public async Task GrantPermissionAsync(PermissionGrantRequest request)
        {
            var entity = (object)(await _foldersRepository.GetByIdAsync(request.EntityId))
                 ?? await _filesRepository.GetByIdAsync(request.EntityId);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found.");
            }

            string parentId = entity switch
            {
                Folder folderEntity => folderEntity.ParentId,
                File fileEntity => fileEntity.FolderId,
                _ => throw new InvalidOperationException("Unknown entity type."),
            };

            var hasAdminPermission = await HasPermissionAsync(new PermissionRequest
            {
                UserId = request.GrantingUserId,
                EntityId = parentId,
                Role = PermissionRoleEnum.Admin,
            });

            if (!hasAdminPermission)
            {
                throw new UnauthorizedAccessException("You must have Admin role to share permissions.");
            }

            var hasPermission = await HasPermissionAsync(new PermissionRequest
            {
                UserId = request.TargetUserId,
                EntityId = request.EntityId,
                Role = request.Role
            });

            if (hasPermission)
            {
                throw new InvalidOperationException("This user already has the same permission for this entity.");
            }

            var existingPermission = (await _permissionsRepository.GetAllAsync())
                .FirstOrDefault(p => p.UserId == request.TargetUserId && p.EntityId == request.EntityId);

            if (existingPermission != null)
            {
                existingPermission.Role = request.Role;
                await _permissionsRepository.UpdateAsync(existingPermission);
            }
            else
            {
                var permission = new Permission
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.TargetUserId,
                    EntityId = request.EntityId,
                    Role = request.Role
                };
                await _permissionsRepository.AddAsync(permission);
            }
        }

        public async Task<bool> HasPermissionAsync(PermissionRequest request)
        {
            var permission = (await _permissionsRepository.GetAllAsync()).FirstOrDefault(p => p.UserId == request.UserId
                && p.EntityId == request.EntityId && p.Role == request.Role);
            if (permission == null)
            {
                return false;
            }

            return true;
        }
    }
}
