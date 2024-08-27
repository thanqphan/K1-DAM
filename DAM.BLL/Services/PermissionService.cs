using DAM.DAM.Api.DTOs.Permission;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Entities;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security;
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
            var enityAsFolder = await _foldersRepository.GetByIdAsync(request.EntityId);
            var entity = (object)enityAsFolder ?? await _filesRepository.GetByIdAsync(request.EntityId);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found.");
            }

            string parentId;
            if (entity is Folder folderEntity)
            {
                parentId = folderEntity.ParentId;
            }
            else if (entity is File fileEntity)
            {
                parentId = fileEntity.FolderId;
            }
            else
            {
                throw new InvalidOperationException("Unknown entity type.");
            }

            var hasAdminPermission = await HasPermissionAsync(new PermissionRequest
            {
                UserId = request.GrantingUserId,
                EntityId = request.EntityId,
                Role = PermissionRoleEnum.Admin,
            });

            if (!hasAdminPermission)
            {
                throw new UnauthorizedAccessException("You must have Admin role to share permissions.");
            }

            // Kiểm tra nếu người dùng đã có quyền với vai trò cụ thể
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

            // Tìm kiếm quyền hiện có nếu vai trò khác
            var existingPermission = (await _permissionsRepository.GetAllAsync())
                .FirstOrDefault(p => p.UserId == request.TargetUserId && p.EntityId == request.EntityId);

            if (existingPermission != null)
            {
                // Cập nhật quyền nếu vai trò khác
                existingPermission.Role = request.Role;
                await _permissionsRepository.UpdateAsync(existingPermission);
            }
            else
            {
                // Tạo quyền mới nếu chưa tồn tại
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
