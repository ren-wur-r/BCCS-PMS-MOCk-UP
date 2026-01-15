using CommonLibrary.CmnCryptography.CryptoAES.Format;

namespace CommonLibrary.CmnCryptography.CryptoAES.Service;

/// <summary>通用-AES服務</summary>
public interface ICmnAesService
{
    /// <summary>通用-AES-加密</summary>
    public CmnAesEncryptRspMdl Encrypt(CmnAesEncryptReqMdl reqObj);

    /// <summary>通用-AES-解密</summary>
    public CmnAesDecryptRspMdl Decrypt(CmnAesDecryptReqMdl reqObj);
}
