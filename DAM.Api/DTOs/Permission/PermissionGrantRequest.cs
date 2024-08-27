using DAM.DAM.DAL.Enums;

namespace DAM.DAM.Api.DTOs.Permission
{
    public class PermissionGrantRequest
    {
        public string GrantingUserId { get; set; }
        public string TargetUserId { get; set; }
        public string EntityId { get; set; }
        public PermissionRoleEnum Role { get; set; }
    }
}
