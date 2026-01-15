using Xunit.Abstractions;

namespace CommonLibrary.CmnXunit;

/// <summary>通用Xunit測試資料</summary>
/// <typeparam name="TInput">輸入資料</typeparam>
/// <typeparam name="TExpected">期望資料</typeparam>
public class CmnXunitTestData<TInput, TExpected> : IXunitSerializable
{
    /// <summary>描述</summary>
    public string Description { get; set; }

    /// <summary>輸入資料</summary>
    public TInput InputObj { get; set; }

    /// <summary>期望資料</summary>
    public TExpected ExpectedObj { get; set; }

    ///// <summary>輸入資料文字</summary>
    //private readonly string _inputJsonText;

    ///// <summary>期望資料文字</summary>
    //private readonly string _expectedJsonText;

    /// <summary>建構式</summary>
    public CmnXunitTestData()
    {
        //this._inputJsonText = null;
        //this._expectedJsonText = null;
    }

    /// <summary>序列化</summary>
    public void Deserialize(IXunitSerializationInfo info)
    {
        this.Description = info.GetValue<string>(nameof(this.Description));
        //this.InputObj = JsonSerializer.Deserialize<TInput>(info.GetValue<string>(nameof(this._inputJsonText)));
        //this.ExpectedObj = JsonSerializer.Deserialize<TExpected>(info.GetValue<string>(nameof(this._expectedJsonText)));
    }

    /// <summary>反序列化</summary>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.Description), this.Description);
        //info.AddValue(nameof(this._inputJsonText), JsonSerializer.Serialize(this.InputObj));
        //info.AddValue(nameof(this._expectedJsonText), JsonSerializer.Serialize(this.ExpectedObj));
    }

    public override string ToString()
    {
        return this.Description;
    }
}
