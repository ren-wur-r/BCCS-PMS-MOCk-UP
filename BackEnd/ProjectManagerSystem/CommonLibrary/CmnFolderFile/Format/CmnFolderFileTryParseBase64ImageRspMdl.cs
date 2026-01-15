namespace CommonLibrary.CmnFolderFile.Format;

public class CmnFolderFileTryParseBase64ImageRspMdl
{
    /// <summary>Base64格式</summary>
    public string Base64Image { get; set; }

    /// <summary>含 data URI 前綴的 Base64 格式</summary>
    public string DataUriImage { get; set; }
}
