using System;
using System.Text.Json;
using CommonLibrary.CmnCryptography.CryptoMD5.Format;
using CommonLibrary.CmnCryptography.CryptoMD5.Service;
using CommonLibrary.CmnNumberBase.Extension;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.Logical.Format;

namespace ServiceLibrary.Core.Employee.Logical.Service;

/// <summary>核心-員工-邏輯服務</summary>
public class CoEmployeeLogicalService : ICoEmployeeLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<CoEmployeeLogicalService> _logger;

    /// <summary>加解密MD5服務</summary>
    private readonly ICmnMd5Service _md5;

    /// <summary>建構式</summary>
    public CoEmployeeLogicalService(
        ILogger<CoEmployeeLogicalService> logger,
        ICmnMd5Service md5)
    {
        this._logger = logger;
        this._md5 = md5;
    }

    /// <summary>核心-員工-邏輯-取得密文密碼</summary>
    public CoEmpLgcGetCypherPasswordRspMdl GetCypherPassword(CoEmpLgcGetCypherPasswordReqMdl reqObj)
    {
        // logical, 加密, 取得加密密碼
        var md5EncryptReqObj = new CmnMd5EncryptReqMdl()
        {
            PlainText = reqObj.EmployeePlainPassword.Trim(),
        };
        var md5EncryptRspObj = this._md5.Encrypt(md5EncryptReqObj);
        if (md5EncryptRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        var rspObj = new CoEmpLgcGetCypherPasswordRspMdl()
        {
            EmployeeCypherPassword = md5EncryptRspObj.CipherText,
        };
        return rspObj;
    }

    /// <summary>核心-員工-邏輯-取得登入令牌</summary>
    public CoEmpLgcGetLoginTokenRspMdl GetLoginToken(CoEmpLgcGetLoginTokenReqMdl reqObj)
    {
        // 10進位, {ID}{現在時間}
        var token10base =
            $"{DateTimeOffset.UtcNow:fff}" + // 3位
            $"{(reqObj.EmployeeID < 0 ? "9" : "8")}" + // 1位
            $"{Math.Abs(reqObj.EmployeeID)}" + // 至少1位
            $"{DateTimeOffset.UtcNow:yyMMddHHmmss}"; // 12位

        // 62進位
        var token62base = token10base.Base10ToBase62();
        if (string.IsNullOrWhiteSpace(token62base))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        var rspObj = new CoEmpLgcGetLoginTokenRspMdl()
        {
            LoginToken = token62base,
        };
        return rspObj;
    }

    /// <summary>核心-員工-邏輯-拆解登入令牌</summary>
    public CoEmpLgcSplitLoginTokenRspMdl SplitLoginToken(CoEmpLgcSplitLoginTokenReqMdl reqObj)
    {
        // 登入令牌，10進位
        var token10base = reqObj.LoginToken.Base62ToBase10();
        if (string.IsNullOrWhiteSpace(token10base))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 判斷長度是否小於17
        if (token10base.Length < 17)
        {
            // 小於，錯誤
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 取得ID正負文字
        var idPnText = token10base[3..4];

        // 取得ID文字
        var idText = token10base[4..(token10base.Length - 12)];
        if (string.IsNullOrWhiteSpace(idText))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        // 轉換ID
        if (int.TryParse(idText, out var id) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        var rspObj = new CoEmpLgcSplitLoginTokenRspMdl
        {
            EmployeeID = idPnText == "9" ? -id : id,
        };
        return rspObj;
    }

}
