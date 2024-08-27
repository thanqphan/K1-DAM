using DAM.DAM.Api.DTOs.Responses.File;

namespace DAM.DAM.Api.DTOs.Requests.File
{
    public class FileRequest : FileResponse
    {
        public string UserId { get; set; }
    }
}
