using System.Collections.Generic;
using CommonLibrary.CmnSMTP.Config;

namespace CommonLibrary.CmnSMTP.Format;

/// <summary>通用-SMTP-發送-請求模型</summary>
public class CmnSmtpSendReqMdl
{
    /// <summary>SMTP設定</summary>
    public CmnSmtpConfig Config { get; set; }

    /// <summary>收件者列表</summary>
    public List<string> ReceiverList { get; set; }

    /// <summary>主旨</summary>
    public string Subject { get; set; }

    /// <summary>內容</summary>
    public string Body { get; set; }

    /// <summary>是否為HTML</summary>
    public bool IsHtml { get; set; }

    /// <summary>附件列表</summary>
    public List<CmnSmtpSendReqItemFileMdl> FileList { get; set; }
}