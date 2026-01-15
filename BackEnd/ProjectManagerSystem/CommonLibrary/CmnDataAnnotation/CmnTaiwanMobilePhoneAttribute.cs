using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CommonLibrary.CmnDataAnnotation;

/// <summary>通用-台灣手機號碼驗證-屬性</summary>
public class CmnTaiwanMobilePhoneAttribute : ValidationAttribute
{
    /// <summary>
    /// 台灣手機號碼正則表達式：
    /// - 格式: 09XX-XXX-XXX
    /// - 開頭為09
    /// - 第三碼為0-9
    /// - 總長度10碼（不含連字號）
    /// - 範例: 0912-043-111
    /// </summary>
    private const string TAIWAN_MOBILE_PHONE = @"^09\d{1}\d{1}-\d{3}-\d{3}$";

    public CmnTaiwanMobilePhoneAttribute()
    {
    }

    /// <summary>是否合法</summary>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var phone = value as string;
        if (string.IsNullOrWhiteSpace(phone))
        {
            return ValidationResult.Success;
        }

        if (Regex.IsMatch(phone, TAIWAN_MOBILE_PHONE) == false)
        {
            return new ValidationResult("手機號碼格式不正確，正確格式為: 09XX-XXX-XXX (例如: 0912-043-111)");
        }

        return ValidationResult.Success;
    }
}
