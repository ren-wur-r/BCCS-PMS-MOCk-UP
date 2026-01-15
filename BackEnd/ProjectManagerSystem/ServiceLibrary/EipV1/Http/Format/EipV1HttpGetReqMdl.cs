using System.Net.Http;

namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-取得-請求模型</summary>
public class EipV1HttpGetReqMdl
{
    /// <summary>相對路徑</summary>
    public string RelativeUrl { get; set; }

    ///// <summary>HTTP方法</summary>
    //public HttpMethod Method { get; set; }

    ///// <summary>要求資料</summary>
    //public object ReqData { get; set; }
}