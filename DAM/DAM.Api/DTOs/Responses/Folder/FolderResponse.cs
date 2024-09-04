namespace DAM.DAM.Api.DTOs.Responses.Folder
{
    public class FolderResponse
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ParentId { get; set; } = string.Empty;
        public string DriveId { get; set; } = string.Empty;
    }
}
