using DAM.DAM.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAM.DAM.DAL.Entities
{
    public class File
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public string Name { get; set; }
        public FileTypeEnum Type { get; set; }
        [MaxLength(36)]
        public string? FolderId { get; set; }
        public Folder? Folder { get; set; }
        [MaxLength(36)]
        public string? DriveId { get; set; }
        public Drive Drive { get; set; }

    }
}
