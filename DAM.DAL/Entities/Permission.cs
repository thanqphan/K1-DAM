using DAM.DAM.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAM.DAM.DAL.Entities
{
    public class Permission
    {
        [MaxLength(36)]
        public string Id { get; set; }
        [MaxLength(36)]
        public string UserId { get; set; }
        public User User { get; set; }
        [MaxLength(36)]
        public string EntityId { get; set; }
        public PermissionRoleEnum Role { get; set; }
    }
}
