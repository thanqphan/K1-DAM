using System.ComponentModel.DataAnnotations;

namespace DAM.DAM.DAL.Entities
{
    public class Folder
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public string Name { get; set; }
        [MaxLength(36)]
        public string? ParentId { get; set; }
        [MaxLength(36)]
        public string? DriveId { get; set; }
        public Drive? Drive { get; set; }
        public Folder? ParentFolder { get; set; }
        public ICollection<Folder> SubFolders { get; set; }
        public ICollection<File> Files { get; set; }

    }
}
