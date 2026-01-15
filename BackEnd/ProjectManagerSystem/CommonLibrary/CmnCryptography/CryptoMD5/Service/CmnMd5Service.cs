using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CommonLibrary.CmnCryptography.CryptoMD5.Format;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnCryptography.CryptoMD5.Service;

/// <summary>通用-MD5服務</summary>
public class CmnMd5Service : ICmnMd5Service
{
    /// <summary>logger</summary>
    private readonly ILogger<CmnMd5Service> _logger;

    /// <summary>建構式</summary>
    public CmnMd5Service(
        ILogger<CmnMd5Service> logger)
    {
        this._logger = logger;
    }

    /// <summary>通用-MD5-加密</summary>
    public CmnMd5EncryptRspMdl Encrypt(CmnMd5EncryptReqMdl reqObj)
    {
        try
        {
            var plainByteArray = MD5.HashData(Encoding.UTF8.GetBytes(reqObj.PlainText));

            var builder = new StringBuilder();
            for (var i = 0; i < plainByteArray.Length; i++)
            {
                builder.Append(plainByteArray[i].ToString("x2"));
            }

            var rspObj = new CmnMd5EncryptRspMdl()
            {
                CipherText = builder.ToString()
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
