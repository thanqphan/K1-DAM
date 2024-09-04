using DAM.DAM.Api.DTOs.Requests.Permission;
using DAM.DAM.BLL.Services;
using DAM.DAM.DAL.Entities;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using Moq;
using Xunit;
using File = DAM.DAM.DAL.Entities.File;

public class PermissionServiceTests
{
    private readonly Mock<IBaseRepository<Permission>> _permissionRepositoryMock;
    private readonly Mock<IBaseRepository<Folder>> _folderRepositoryMock;
    private readonly Mock<IBaseRepository<File>> _fileRepositoryMock;
    private readonly PermissionService _permissionService;

    public PermissionServiceTests()
    {
        _permissionRepositoryMock = new Mock<IBaseRepository<Permission>>();
        _folderRepositoryMock = new Mock<IBaseRepository<Folder>>();
        _fileRepositoryMock = new Mock<IBaseRepository<File>>();            

        _permissionService = new PermissionService(
            _permissionRepositoryMock.Object,
            _folderRepositoryMock.Object,
            _fileRepositoryMock.Object
        );
    }
    [Fact]
    public async Task GrantPermissionAsync_AdminUserSharesContributorPermissions_Success()
    {
        // Arrange
        var adminUserId = "admin_user";
        var targetUserId = "contributor1";
        var folderId = "subfolder1";
        var parentFolderId = "root1";

        var parentFolder = new Folder { Id = parentFolderId, ParentId = null };
        var subFolder = new Folder { Id = folderId, ParentId = parentFolderId };
        var adminPermission = new Permission { UserId = adminUserId, EntityId = parentFolderId, Role = PermissionRoleEnum.Admin };

        _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
            .ReturnsAsync(subFolder);

        _permissionRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Permission> { adminPermission });

        var request = new PermissionGrantRequest
        {
            GrantingUserId = adminUserId,
            TargetUserId = targetUserId,
            EntityId = folderId,
            Role = PermissionRoleEnum.Contributor
        };

        // Act
        await _permissionService.GrantPermissionAsync(request);

        // Assert
        _permissionRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Permission>(
            p => p.UserId == targetUserId && p.EntityId == folderId && p.Role == PermissionRoleEnum.Contributor)), Times.Once);
    }

}
