using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CommonLibrary.CmnDataAnnotation;

/// <summary>通用-台灣市內電話驗證-屬性</summary>
public class CmnTaiwanTelephoneAttribute : ValidationAttribute
{
    /// <summary>
    /// 台灣市內電話正則表達式：
    /// - 區碼: 02-09 (2-4碼)
    /// - 號碼: 3-4碼 + 3-4碼 (中間用連字號分隔)
    /// - 分機: 可選，以#開頭後接數字
    /// - 範例: 02-123-4567#分機, 02-1234-5678#分機, 04-2222-3333
    /// </summary>
    private const string TAIWAN_TELEPHONE = @"^0[2-9]{1,1}-\d{3,4}-\d{3,4}(#.*)?$";

    public CmnTaiwanTelephoneAttribute()
    {
    }

    /// <summary>是否合法</summary>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var telephone = value as string;
        if (string.IsNullOrWhiteSpace(telephone))
        {
            return ValidationResult.Success;
        }

        if (Regex.IsMatch(telephone, TAIWAN_TELEPHONE) == false)
        {
            return new ValidationResult("電話號碼格式不正確，正確格式為: 0X-XXX(X)-XXXX 或 0X-XXX(X)-XXXX#分機 (例如: 02-123-4567, 02-1234-5678#100)");
        }

        return ValidationResult.Success;
    }
}
