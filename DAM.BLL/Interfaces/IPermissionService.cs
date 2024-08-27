using DAM.DAM.Api.DTOs.Permission;

namespace DAM.DAM.BLL.Interfaces
{
    public interface IPermissionService
    {
        Task GrantPermissionAsync(PermissionGrantRequest request);
        Task<bool> HasPermissionAsync(PermissionRequest request);
    }
}
