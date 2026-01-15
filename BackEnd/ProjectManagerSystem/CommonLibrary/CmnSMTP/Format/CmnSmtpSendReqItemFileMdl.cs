namespace CommonLibrary.CmnSMTP.Format;

/// <summary>通用-SMTP-發送-請求-附件</summary>
public class CmnSmtpSendReqItemFileMdl
{
    /// <summary>檔案名稱</summary>
    public string FileName { get; set; }

    /// <summary>檔案位元陣列</summary>
    public byte[] FileByteArray { get; set; }

    /// <summary>檔案內容類型</summary>
    public string FileContentType { get; set; }
}