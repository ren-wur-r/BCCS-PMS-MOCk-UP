using Microsoft.AspNetCore.Http;

namespace CommonLibrary.CmnExcel.Format;

/// <summary>通用-Excel-讀取Xlsx依IFormFile-請求模型</summary>
public class CmnExcelReadXlsxByIFormFilReqMdl
{
    /// <summary>FormFile</summary>
    public IFormFile FormFile { get; set; }
}
