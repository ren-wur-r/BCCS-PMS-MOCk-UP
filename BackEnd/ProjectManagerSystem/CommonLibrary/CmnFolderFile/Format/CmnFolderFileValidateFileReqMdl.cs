using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.CmnFolderFile.Format;

public class CmnFolderFileValidateFileReqMdl
{
    /// <summary>上傳的檔案</summary>
    public IFormFile FormFile { get; set; }

    /// <summary>允許大小</summary>
    public long? AllowSize { get; set; }

    /// <summary>允許副檔名列表</summary>
    public IEnumerable<string> AllowExtensionList { get; set; }

}
