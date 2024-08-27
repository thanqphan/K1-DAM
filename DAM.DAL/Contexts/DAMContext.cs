using DAM.DAM.DAL.Entities;
using DAM.DAM.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using File = DAM.DAM.DAL.Entities.File;

namespace DAM.DAM.DAL.Contexts
{
    public class DAMContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Drive> Drives { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DAMContext(DbContextOptions<DAMContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Drives)
                .WithOne(d => d.Owner)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Drive>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Drive>()
                .HasMany(d => d.Folders)
                .WithOne(f => f.Drive)
                .HasForeignKey(f => f.DriveId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Drive>()
                .HasMany(d => d.Files)
                .WithOne(f => f.Drive)
                .HasForeignKey(f => f.DriveId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Folder>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.SubFolders)
                .WithOne(sf => sf.ParentFolder)
                .HasForeignKey(sf => sf.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Files)
                .WithOne(file => file.Folder)
                .HasForeignKey(file => file.FolderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<File>()
                .HasKey(file => file.Id);

            modelBuilder.Entity<File>()
                .HasOne(file => file.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(file => file.FolderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<File>()
                .HasOne(file => file.Drive)
                .WithMany(d => d.Files)
                .HasForeignKey(file => file.DriveId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<File>()
                .Property(p => p.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (FileTypeEnum)Enum.Parse(typeof(FileTypeEnum), v));

            modelBuilder.Entity<Permission>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Permission>()
                .Property(p => p.Role)
                .HasConversion(
                    v => v.ToString(), 
                    v => (PermissionRoleEnum)Enum.Parse(typeof(PermissionRoleEnum), v));

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.EntityId);
        }
    }
}
