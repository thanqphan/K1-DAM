using DAM.DAM.Api.DTOs.Folder;
using DAM.DAM.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAM.DAM.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFolder([FromBody] FolderRequest request)
        {
            var response = await _folderService.AddFolderAsync(request);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateFolder([FromBody] FolderRequest request)
        {
            var response = await _folderService.UpdateFolderAsync(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder([FromQuery] FolderDeleteRequest request)
        {
            await _folderService.DeleteFolderAsync(request);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolderById(string id)
        {
            try
            {
                var folderDto = await _folderService.GetFolderByIdAsync(id);
                return Ok(folderDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFolders([FromQuery] FolderGetAllRequest request)
        {
            var folders = await _folderService.GetAllFoldersAsync(request);
            return Ok(folders);
        }
    }
}
