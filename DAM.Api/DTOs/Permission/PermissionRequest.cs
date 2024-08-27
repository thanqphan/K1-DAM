using DAM.DAM.DAL.Enums;

namespace DAM.DAM.Api.DTOs.Permission
{
    public class PermissionRequest
    {
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public PermissionRoleEnum Role { get; set; }
    }
}
