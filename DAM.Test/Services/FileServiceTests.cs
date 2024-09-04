using AutoMapper;
using DAM.DAM.Api.DTOs.Profiles;
using DAM.DAM.Api.DTOs.Requests.File;
using DAM.DAM.Api.DTOs.Requests.Permission;
using DAM.DAM.Api.DTOs.Responses.File;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.BLL.Services;
using DAM.DAM.DAL.Enums;
using DAM.DAM.DAL.Interfaces;
using Moq;
using File = DAM.DAM.DAL.Entities.File;

public class FileServiceTests
{
    private readonly FileService _fileService;
    private readonly Mock<IBaseRepository<File>> _fileRepositoryMock;
    private readonly Mock<IPermissionService> _permissionServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public FileServiceTests()
    {
        _fileRepositoryMock = new Mock<IBaseRepository<File>>();
        _permissionServiceMock = new Mock<IPermissionService>();
        _mapperMock = new Mock<IMapper>();
        _fileService = new FileService(_fileRepositoryMock.Object, _permissionServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task AddFileAsync_WithValidContributorPermission_ShouldAddFile()
    {
        // Arrange
        var request = new FileRequest
        {
            UserId = "contributor1",
            Id = "file3",
            FolderId = "folder1",
            Name = "Spec3.pdf"
        };

        _permissionServiceMock.Setup(x => x.HasPermissionAsync(It.Is<PermissionRequest>
            (p => p.UserId == request.UserId && p.EntityId == request.Id && p.Role == PermissionRoleEnum.Contributor)))
                              .ReturnsAsync(true);

        var fileEntity = new File { Id = request.Id, Name = request.Name };
        _mapperMock.Setup(m => m.Map<File>(request)).Returns(fileEntity);
        _mapperMock.Setup(m => m.Map<FileResponse>(fileEntity)).Returns(new FileResponse { Id = request.Id, Name = request.Name });

        // Act
        var result = await _fileService.AddFileAsync(request);

        // Assert
        _fileRepositoryMock.Verify(x => x.AddAsync(It.Is<File>(f => f.Id == request.Id && f.Name == request.Name)), Times.Once);
        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task UpdateFileAsync_WithValidContributorPermission_ShouldUpdateFile()
    {
        // Arrange
        var request = new FileRequest
        {
            UserId = "contributor1",
            Id = "file1",
            Name = "Spec1_Final.pdf",
            FolderId = "folder1"
        };

        var existingFile = new File { Id = request.Id, Name = "Spec1.pdf" };

        _fileRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(existingFile);

        _permissionServiceMock.Setup(x => x.HasPermissionAsync(It.Is<PermissionRequest>
            (p => p.UserId == request.UserId && p.EntityId == request.Id && p.Role == PermissionRoleEnum.Contributor)))
                              .ReturnsAsync(true);

        // Using the actual mapper instead of mocking <- wrong setup mock mapper 
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FileProfile>();
        });
        var mapper = config.CreateMapper();
        var fileService = new FileService(_fileRepositoryMock.Object, _permissionServiceMock.Object, mapper);

        // Act
        var result = await fileService.UpdateFileAsync(request);

        // Assert
        _fileRepositoryMock.Verify(x => x.UpdateAsync(It.Is<File>(f => f.Id == request.Id && f.Name == request.Name)), Times.Once);
        Assert.Equal(request.Name, result.Name);
    }



    [Fact]
    public async Task AddFileAsync_WithInvalidPermission_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var request = new FileRequest
        {
            UserId = "contributor1",
            Id = "file3",
            FolderId = "folder1",
            Name = "Spec3.pdf"
        };

        _permissionServiceMock.Setup(x => x.HasPermissionAsync(It.Is<PermissionRequest>(p => p.UserId == request.UserId && p.EntityId == request.Id && p.Role == PermissionRoleEnum.Contributor)))
                              .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _fileService.AddFileAsync(request));
    }

    [Fact]
    public async Task UpdateFileAsync_WithInvalidPermission_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var request = new FileRequest
        {
            UserId = "contributor1",
            Id = "file1",
            Name = "Spec1_Final.pdf",
            FolderId = "folder1"
        };

        var existingFile = new File { Id = request.Id, Name = "Spec1.pdf" };

        _fileRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(existingFile);

        _permissionServiceMock.Setup(x => x.HasPermissionAsync(It.Is<PermissionRequest>
            (p => p.UserId == request.UserId && p.EntityId == request.Id && p.Role == PermissionRoleEnum.Contributor)))
                              .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _fileService.UpdateFileAsync(request));
    }

    [Fact]
    public async Task DeleteFileAsync_WithReaderPermission_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var fileId = "file3";
        var userId = "reader1";

        var file = new File { Id = fileId, Name = "File3.docx" };

        // Set up the repository to return the file when queried by Id
        _fileRepositoryMock.Setup(x => x.GetByIdAsync(fileId)).ReturnsAsync(file);

        // Set up the permission service to return false for delete permission
        _permissionServiceMock.Setup(x => x.HasPermissionAsync(It.Is<PermissionRequest>(
            p => p.UserId == userId && p.EntityId == fileId && p.Role == PermissionRoleEnum.Reader)))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _fileService.DeleteFileAsync(fileId, userId));

        Assert.Equal("You do not have permission to delete files.", exception.Message);
    }

}
