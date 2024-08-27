using System.ComponentModel.DataAnnotations;

namespace DAM.DAM.DAL.Entities
{
    public class Drive
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public string Name { get; set; }
        [MaxLength(36)]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
