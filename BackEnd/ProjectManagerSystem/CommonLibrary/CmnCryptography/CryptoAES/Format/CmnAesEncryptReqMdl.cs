namespace CommonLibrary.CmnCryptography.CryptoAES.Format;

public class CmnAesEncryptReqMdl
{
    /// <summary>明文</summary>
    public string PlainText { get; set; }

    /// <summary>金鑰</summary>
    public string Key { get; set; }

    /// <summary>IV</summary>
    public string IV { get; set; }
}
