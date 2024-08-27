using DAM.DAM.Api.DTOs.Responses.File;

namespace DAM.DAM.Api.DTOs.Responses.Folder
{
    public class FolderDetailsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string DriveId { get; set; }
        public List<FolderResponse> SubFolders { get; set; }
        public List<FileResponse> Files { get; set; }
    }
}
