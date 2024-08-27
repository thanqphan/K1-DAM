using DAM.DAM.Api.DTOs.Permission;
using DAM.DAM.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAM.DAM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("grant")]
        public async Task<IActionResult> GrantPermission([FromBody] PermissionGrantRequest request)
        {
            await _permissionService.GrantPermissionAsync(request);

            return Ok();
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckPermission([FromQuery] PermissionRequest request)
        {
            var hasPermission = await _permissionService.HasPermissionAsync(request);

            return Ok(hasPermission);
        }
    }
}
