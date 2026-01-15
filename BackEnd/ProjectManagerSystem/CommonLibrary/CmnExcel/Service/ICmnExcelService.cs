using CommonLibrary.CmnExcel.Format;

namespace CommonLibrary.CmnExcel.Service;

/// <summary>通用-Excel-服務</summary>
public interface ICmnExcelService
{
    #region xlsx

    /// <summary>通用-Excel-建立Xlsx</summary>
    public CmnExcelCreateXlsxRspMdl CreateXlsx(CmnExcelCreateXlsxReqMdl reqObj);

    /// <summary>通用-Excel-附加Xlsx</summary>
    public CmnExcelAppendXlsxRspMdl AppendXlsx(CmnExcelAppendXlsxReqMdl reqObj);

    /// <summary>通用-Excel-讀取Xlsx依檔案</summary>
    public CmnExcelReadXlsxByFileRspMdl ReadXlsxByFile(CmnExcelReadXlsxByFileReqMdl reqObj);

    /// <summary>通用-Excel-讀取Xlsx依IFormFile</summary>
    public CmnExcelReadXlsxByIFormFilRspMdl ReadXlsxByIFormFile(CmnExcelReadXlsxByIFormFilReqMdl reqObj);

    #endregion

    #region xls

    /// <summary>通用-Excel-建立Xls</summary>
    public CmnExcelCreateXlsRspMdl CreateXls(CmnExcelCreateXlsReqMdl reqObj);

    /// <summary>通用-Excel-附加Xls</summary>
    public CmnExcelAppendXlsRspMdl AppendXls(CmnExcelAppendXlsReqMdl reqObj);

    /// <summary>通用-Excel-讀取Xls依檔案</summary>
    public CmnExcelReadXlsByFileRspMdl ReadXlsByFile(CmnExcelReadXlsByFileReqMdl reqObj);

    /// <summary>通用-Excel-讀取Xls依IFormFile</summary>
    public CmnExcelReadXlsByIFormFilRspMdl ReadXlsByIFormFile(CmnExcelReadXlsByIFormFilReqMdl reqObj);

    #endregion

}
