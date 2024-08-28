using DAM.DAM.DAL.Enums;

namespace DAM.DAM.Api.DTOs.Responses.File
{
    public class FileResponse
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string FolderId { get; set; } = string.Empty;
        public string DriveId { get; set; } = string.Empty;
    }
}
