using DAM.DAM.Api.DTOs.File;
using DAM.DAM.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAM.DAM.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _FileService;

        public FileController(IFileService FileService)
        {
            _FileService = FileService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFile([FromBody] FileRequest request)
        {
            var response = await _FileService.AddFileAsync(request);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateFile([FromBody] FileRequest request)
        {
            var response = await _FileService.UpdateFileAsync(request);

            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFile([FromQuery] FileRequest request)
        {
            await _FileService.DeleteFileAsync(request);

            return NoContent();
        }
    }
}
