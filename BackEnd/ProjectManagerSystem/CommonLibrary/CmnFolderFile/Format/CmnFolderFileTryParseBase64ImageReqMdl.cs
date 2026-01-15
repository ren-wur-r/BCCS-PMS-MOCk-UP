using Microsoft.AspNetCore.Http;

namespace CommonLibrary.CmnFolderFile.Format;
public class CmnFolderFileTryParseBase64ImageReqMdl
{
    /// <summary>上傳檔案</summary>
    public IFormFile FormFile { get; set; }
}
