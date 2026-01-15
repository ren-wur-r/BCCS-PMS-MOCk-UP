using CommonLibrary.CmnCryptography.CryptoMD5.Format;

namespace CommonLibrary.CmnCryptography.CryptoMD5.Service;

/// <summary>通用-MD5服務</summary>
public interface ICmnMd5Service
{
    /// <summary>通用-MD5-加密</summary>
    public CmnMd5EncryptRspMdl Encrypt(CmnMd5EncryptReqMdl reqObj);
}
