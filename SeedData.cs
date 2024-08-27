using DAM.DAM.DAL.Entities;

namespace DAM
{
    using global::DAM.DAM.DAL.Contexts;
    using global::DAM.DAM.DAL.Enums;
    using System;
    using File = DAM.DAL.Entities.File;

    public static class SeedData
    {
        public static void Initialize(DAMContext context)
        {
            if (context.Users.Any() || context.Drives.Any() || context.Folders.Any() || context.Files.Any() || context.Permissions.Any())
            {
                return; // DB has been seeded
            }
            var user1 = new User { Id = Guid.NewGuid().ToString(), Username = "admin_user" };
            var user2 = new User { Id = Guid.NewGuid().ToString(), Username = "contributor1" };
            var user3 = new User { Id = Guid.NewGuid().ToString(), Username = "reader1" };

            var driveA = new Drive { Id = Guid.NewGuid().ToString(), Name = "DriveA", OwnerId = user1.Id };
            var driveB = new Drive { Id = Guid.NewGuid().ToString(), Name = "DriveB", OwnerId = user1.Id };
            var driveC = new Drive { Id = Guid.NewGuid().ToString(), Name = "DriveC", OwnerId = user2.Id };
            var driveD = new Drive { Id = Guid.NewGuid().ToString(), Name = "DriveD", OwnerId = user3.Id };

            // Folders and Subfolders
            var root1 = new Folder { Id = Guid.NewGuid().ToString(), Name = "Root1", DriveId = driveA.Id };
            var subFolder1 = new Folder { Id = Guid.NewGuid().ToString(), Name = "SubFolder1", ParentId = root1.Id };
            var subFolder2 = new Folder { Id = Guid.NewGuid().ToString(), Name = "SubFolder2", ParentId = root1.Id };
            var subSubFolder1 = new Folder { Id = Guid.NewGuid().ToString(), Name = "SubSubFolder1", ParentId = subFolder2.Id };

            var root2 = new Folder { Id = Guid.NewGuid().ToString(), Name = "Root2", DriveId = driveA.Id };

            var projectFiles = new Folder { Id = Guid.NewGuid().ToString(), Name = "ProjectFiles", DriveId = driveB.Id };
            var specifications = new Folder { Id = Guid.NewGuid().ToString(), Name = "Specifications", ParentId = projectFiles.Id };

            var documents = new Folder { Id = Guid.NewGuid().ToString(), Name = "Documents", DriveId = driveC.Id };
            var images = new Folder { Id = Guid.NewGuid().ToString(), Name = "Images", DriveId = driveC.Id };

            var publicFolder = new Folder { Id = Guid.NewGuid().ToString(), Name = "Public", DriveId = driveD.Id };
            var archives = new Folder { Id = Guid.NewGuid().ToString(), Name = "Archives", DriveId = driveD.Id };

            var file1 = new File { Id = Guid.NewGuid().ToString(), Name = "File1.txt", FolderId = subFolder1.Id, Type = FileTypeEnum.Text };
            var file2 = new File { Id = Guid.NewGuid().ToString(), Name = "File2.txt", FolderId = subFolder1.Id, Type = FileTypeEnum.Text };
            var file3 = new File { Id = Guid.NewGuid().ToString(), Name = "File3.docx", FolderId = subSubFolder1.Id, Type = FileTypeEnum.Document };
            var designPdf = new File { Id = Guid.NewGuid().ToString(), Name = "Design.pdf", FolderId = projectFiles.Id, Type = FileTypeEnum.PDF };
            var planDocx = new File { Id = Guid.NewGuid().ToString(), Name = "Plan.docx", FolderId = projectFiles.Id, Type = FileTypeEnum.Document };
            var spec1Pdf = new File { Id = Guid.NewGuid().ToString(), Name = "Spec1.pdf", FolderId = specifications.Id, Type = FileTypeEnum.PDF };
            var spec2Pdf = new File { Id = Guid.NewGuid().ToString(), Name = "Spec2.pdf", FolderId = specifications.Id, Type = FileTypeEnum.PDF };

            var permission1 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user1.Id, EntityId = root1.Id, Role = PermissionRoleEnum.Admin };
            var permission2 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user2.Id, EntityId = subFolder1.Id, Role = PermissionRoleEnum.Contributor };
            var permission3 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user3.Id, EntityId = subSubFolder1.Id, Role = PermissionRoleEnum.Reader };
            var permission4 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user1.Id, EntityId = projectFiles.Id, Role = PermissionRoleEnum.Admin };
            var permission5 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user2.Id, EntityId = specifications.Id, Role = PermissionRoleEnum.Contributor };
            var permission6 = new Permission { Id = Guid.NewGuid().ToString(), UserId = user3.Id, EntityId = file3.Id, Role = PermissionRoleEnum.Reader };

            context.Users.AddRange(user1, user2, user3);
            context.Drives.AddRange(driveA, driveB, driveC, driveD);
            context.Folders.AddRange(root1, root2, subFolder1, subFolder2, subSubFolder1, projectFiles, specifications, documents, images, publicFolder, archives);
            context.Files.AddRange(file1, file2, file3, designPdf, planDocx, spec1Pdf, spec2Pdf);
            context.Permissions.AddRange(permission1, permission2, permission3, permission4, permission5, permission6);

            context.SaveChanges();
        }
    }

}
