using Microsoft.AspNetCore.Http;

namespace CommonLibrary.CmnFolderFile.Format;

public class CmnFolderFileSaveFileReqMdl
{
    /// <summary>基底路徑</summary>
    public string BasePath { get; set; }

    /// <summary>相對路徑</summary>
    public string RelativeFilePath { get; set; }

    /// <summary>上傳的檔案</summary>
    public IFormFile FormFile { get; set; }
}
