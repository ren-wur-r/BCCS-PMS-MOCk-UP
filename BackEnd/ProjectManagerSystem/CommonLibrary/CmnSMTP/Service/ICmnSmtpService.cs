using System.Threading.Tasks;
using CommonLibrary.CmnSMTP.Format;

namespace CommonLibrary.CmnSMTP.Service;

/// <summary>通用-SMTP-服務</summary>
public interface ICmnSmtpService
{
    /// <summary>通用-SMTP-發送</summary>      
    public Task<CmnSmtpSendRspMdl> SendAsync(CmnSmtpSendReqMdl reqObj);
}