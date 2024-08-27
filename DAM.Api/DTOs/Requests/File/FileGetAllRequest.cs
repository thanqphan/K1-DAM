namespace DAM.DAM.Api.DTOs.Requests.File
{
    public class FileGetAllRequest
    {
        public string SearchQuery { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
