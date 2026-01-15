namespace CommonLibrary.CmnFolderFile.Format;

/// <summary>通用-資料夾檔案-儲存檔案 from Byte list</summary>
public class CmnFolderFileSaveFileFromByteListReqMdl
{
    /// <summary>基底路徑</summary>
    public string BasePath { get; set; }

    /// <summary>相對路徑</summary>
    public string RelativeFilePath { get; set; }

    /// <summary>檔案內容</summary>
    public byte[] FileByteList { get; set; }
}