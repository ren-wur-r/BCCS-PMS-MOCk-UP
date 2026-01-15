using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLibrary.CmnSMTP.Format;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.CmnSMTP.Service;

/// <summary>通用-SMTP-服務</summary>
public class CmnSmtpService : ICmnSmtpService
{
    private readonly ILogger<CmnSmtpService> _logger;

    public CmnSmtpService(ILogger<CmnSmtpService> logger)
    {
        this._logger = logger;
    }

    /// <summary>通用-SMTP-發送</summary>
    public async Task<CmnSmtpSendRspMdl> SendAsync(CmnSmtpSendReqMdl reqObj)
    {
        MailMessage mailMessage = null;
        SmtpClient smtpClient = null;
        List<MemoryStream> memoryStreamList = null;
        if (MailAddress.TryCreate(reqObj.Config.SenderEmail, reqObj.Config.SenderName, out var fromMailAddress) == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return default;
        }

        try
        {
            mailMessage = new MailMessage
            {
                From = fromMailAddress, // 設定寄件人
                Subject = reqObj.Subject,
                Body = reqObj.Body,
                IsBodyHtml = reqObj.IsHtml,
                Priority = MailPriority.Normal
            };

            // 添加收件人
            foreach (var receiverItem in reqObj.ReceiverList)
            {
                mailMessage.To.Add(receiverItem);
            }

            // 添加附件
            if (reqObj.FileList != default && reqObj.FileList.Count > 0)
            {
                memoryStreamList = new List<MemoryStream>();
                foreach (var fileItem in reqObj.FileList)
                {
                    if (fileItem.FileByteArray != null)
                    {
                        var fileStream = new MemoryStream(fileItem.FileByteArray);
                        memoryStreamList.Add(fileStream);
                        var mailAttachment = new Attachment(fileStream, fileItem.FileName);

                        // 設定內容類型
                        if (string.IsNullOrWhiteSpace(fileItem.FileContentType) == false)
                        {
                            mailAttachment.ContentType = new System.Net.Mime.ContentType(fileItem.FileContentType);
                        }
                        mailMessage.Attachments.Add(mailAttachment);
                    }
                }
            }

            smtpClient = new SmtpClient()
            {
                Host = reqObj.Config.Host,
                Port = reqObj.Config.Port,
                EnableSsl = reqObj.Config.EnableSsl,
                UseDefaultCredentials = false,
                Timeout = 30 * 1000,
            };

            if (string.IsNullOrWhiteSpace(reqObj.Config.Username) == false
                && string.IsNullOrWhiteSpace(reqObj.Config.Password) == false)
            {
                smtpClient.Credentials = new NetworkCredential(reqObj.Config.Username, reqObj.Config.Password);
            }

            await smtpClient.SendMailAsync(mailMessage);

            var rspObj = new CmnSmtpSendRspMdl
            {
                IsSuccess = true,
            };
            return rspObj;
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {(ex.InnerException == null ? string.Empty : ex.InnerException.Message)}");
            return default;
        }
        finally
        {
            mailMessage?.Dispose();
            smtpClient?.Dispose();
            memoryStreamList?.ForEach(stream => stream.Dispose());
        }
    }

}