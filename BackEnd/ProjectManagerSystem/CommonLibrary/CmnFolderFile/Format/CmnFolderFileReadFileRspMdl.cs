namespace CommonLibrary.CmnFolderFile.Format;

/// <summary>通用-資料夾檔案-讀取檔案-回應模型</summary>
public class CmnFolderFileReadFileRspMdl
{
    /// <summary>檔案串流</summary>
    public byte[] FileByteList { get; set; }

    /// <summary>檔案類型</summary>
    public string ContentType { get; set; }

    /// <summary>檔案名稱</summary>
    public string FileName { get; set; }
}