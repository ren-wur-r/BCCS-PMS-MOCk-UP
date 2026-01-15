using CommonLibrary.CmnCryptography.CryptoSHA1.Format;

namespace CommonLibrary.CmnCryptography.CryptoSHA1.Service;

/// <summary>通用-Sha1服務</summary>
public interface ICmnSha1Service
{
    /// <summary>通用-SHA1-加密</summary>
    public CmnSha1EncryptRspMdl Encrypt(CmnSha1EncryptReqMdl reqObj);
}
