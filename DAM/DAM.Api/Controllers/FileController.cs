using DAM.DAM.Api.DTOs.Requests.File;
using DAM.DAM.Api.DTOs.Requests.Folder;
using DAM.DAM.BLL.Interfaces;
using DAM.DAM.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAM.DAM.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService FileService)
        {
            _fileService = FileService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFile([FromBody] FileRequest request)
        {
            var response = await _fileService.AddFileAsync(request);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateFile([FromBody] FileRequest request)
        {
            var response = await _fileService.UpdateFileAsync(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] string id, [FromQuery] string userId)
        {
            await _fileService.DeleteFileAsync(id, userId);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(string id)
        {
            try
            {
                var response = await _fileService.GetFileByIdAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFiles([FromQuery] FolderGetAllRequest request)
        {
            var folders = await _fileService.GetAllFilesAsync(request);
            return Ok(folders);
        }
    }
}
