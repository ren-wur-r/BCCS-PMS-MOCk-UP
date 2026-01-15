using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CommonLibrary.CmnCryptography.CryptoSHA1.Format;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnCryptography.CryptoSHA1.Service;

/// <summary>通用-Sha1服務</summary>
public class CmnSha1Service : ICmnSha1Service
{
    /// <summary>logger</summary>
    private readonly ILogger<CmnSha1Service> _logger;

    /// <summary>建構式</summary>
    public CmnSha1Service(
        ILogger<CmnSha1Service> logger)
    {
        this._logger = logger;
    }

    /// <summary>通用-Sha1-加密</summary>
    public CmnSha1EncryptRspMdl Encrypt(CmnSha1EncryptReqMdl reqObj)
    {
        try
        {
            var plainByteArray = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(reqObj.PlainText));
            var result = Convert.ToBase64String(plainByteArray).ToLower();

            var rspObj = new CmnSha1EncryptRspMdl()
            {
                CipherText = result
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
    }
}
