using DAM.DAM.Api.DTOs.Responses.Folder;

namespace DAM.DAM.Api.DTOs.Requests.Folder
{
    public class FolderRequest : FolderResponse
    {
        public string UserId { get; set; }
    }
}
