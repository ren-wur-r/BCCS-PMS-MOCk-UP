namespace ServiceLibrary.EipV1.Http.Format;

/// <summary>EipV1-Http-取得-回應模型</summary>
public class EipV1HttpGetRspMdl<T>
{
    /// <summary>回應資料</summary>
    public T RspData { get; set; }
}