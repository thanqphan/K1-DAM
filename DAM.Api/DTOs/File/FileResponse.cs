using DAM.DAM.DAL.Enums;

namespace DAM.DAM.Api.DTOs.File
{
    public class FileResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FileTypeEnum Type { get; set; }
        public string FolderId { get; set; }
        public string DriveId { get; set; }
    }
}
