using System.Threading.Tasks;
using CommonLibrary.CmnFolderFile.Format;

namespace CommonLibrary.CmnFolderFile.Service;

/// <summary>通用-資料夾檔案-邏輯服務</summary>
public interface ICmnFolderFileService
{
    /// <summary>取得檔案類型</summary>
    public string GetContentType(string fileName);

    /// <summary>通用-資料夾檔案-合併路徑</summary>
    public CmnFolderFileCombinePathRspMdl CombinePath(CmnFolderFileCombinePathReqMdl reqObj);

    /// <summary>通用-資料夾檔案-建立檔案</summary>
    public Task<CmnFolderFileCreateFileRspMdl> CreateFileAsync(CmnFolderFileCreateFileReqMdl reqObj);

    /// <summary>通用-資料夾檔案-儲存檔案</summary>
    public Task<CmnFolderFileSaveFileRspMdl> SaveFileAsync(CmnFolderFileSaveFileReqMdl reqObj);

    /// <summary>通用-資料夾檔案-刪除檔案</summary>
    public CmnFolderFileDeleteFileRspMdl DeleteFile(CmnFolderFileDeleteFileReqMdl reqObj);

    /// <summary>通用-資料夾檔案-讀取檔案</summary>
    public CmnFolderFileReadFileRspMdl ReadFile(CmnFolderFileReadFileReqMdl reqObj);

    /// <summary>通用-資料夾檔案-驗證檔案</summary>
    public CmnFolderFileValidateFileRspMdl ValidateFile(CmnFolderFileValidateFileReqMdl reqObj);

    /// <summary>通用-資料夾檔案-刪除資料夾</summary>
    public CmnFolderFileDeleteFolderRspMdl DeleteFolder(CmnFolderFileDeleteFolderReqMdl reqObj);

    /// <summary>通用-資料夾檔案-轉換Base64格式</summary>
    public Task<CmnFolderFileTryParseBase64ImageRspMdl> TryParseBase64ImageAsync(CmnFolderFileTryParseBase64ImageReqMdl reqObj);

    /// <summary>通用-資料夾檔案-儲存檔案 from Byte list</summary>
    public Task<CmnFolderFileSaveFileFromByteListRspMdl> SaveFileFromByteListAsync(CmnFolderFileSaveFileFromByteListReqMdl reqObj);
}
