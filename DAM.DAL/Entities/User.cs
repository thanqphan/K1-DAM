using System.ComponentModel.DataAnnotations;

namespace DAM.DAM.DAL.Entities
{
    public class User
    {
        [MaxLength(36)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        public ICollection<Drive> Drives { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
