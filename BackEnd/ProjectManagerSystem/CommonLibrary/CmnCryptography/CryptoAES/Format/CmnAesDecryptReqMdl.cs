namespace CommonLibrary.CmnCryptography.CryptoAES.Format;

public class CmnAesDecryptReqMdl
{
    /// <summary>密文</summary>
    public string CipherText { get; set; }

    /// <summary>金鑰</summary>
    public string Key { get; set; }

    /// <summary>IV</summary>
    public string IV { get; set; }

}
