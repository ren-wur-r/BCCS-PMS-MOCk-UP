using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CommonLibrary.CmnCryptography.CryptoAES.Format;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnCryptography.CryptoAES.Service;

/// <summary>通用-AES服務</summary>
public class CmnAesService : ICmnAesService
{
    /// <summary>logger</summary>
    private readonly ILogger<CmnAesService> _logger;

    /// <summary>建構式</summary>
    public CmnAesService(
        ILogger<CmnAesService> logger)
    {
        this._logger = logger;
    }

    /// <summary>通用-AES-加密</summary>
    public CmnAesEncryptRspMdl Encrypt(CmnAesEncryptReqMdl reqObj)
    {
        var aes = Aes.Create();
        try
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(reqObj.Key);
            aes.IV = Encoding.UTF8.GetBytes(reqObj.IV);

            var plainByteArray = Encoding.UTF8.GetBytes(reqObj.PlainText);
            var cypherByteArray = aes.CreateEncryptor().TransformFinalBlock(plainByteArray, 0, plainByteArray.Length);

            var cypherText = Convert.ToBase64String(cypherByteArray);

            var rspObj = new CmnAesEncryptRspMdl()
            {
                CypherText = cypherText,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return default;
        }
        finally
        {
            aes.Dispose();
        }
    }

    /// <summary>通用-AES-解密</summary>
    public CmnAesDecryptRspMdl Decrypt(CmnAesDecryptReqMdl reqObj)
    {
        var aes = Aes.Create();
        try
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(reqObj.Key);
            aes.IV = Encoding.UTF8.GetBytes(reqObj.IV);

            var cipherByteArray = Convert.FromBase64String(reqObj.CipherText);
            var plainByteArray = aes.CreateDecryptor().TransformFinalBlock(cipherByteArray, 0, cipherByteArray.Length);

            var plainText = Encoding.UTF8.GetString(plainByteArray);

            var rspObj = new CmnAesDecryptRspMdl()
            {
                PlainText = plainText,
            };
            return rspObj;
        }
        catch
        {
            return default;
        }
        finally
        {
            aes.Dispose();
        }
    }

}
