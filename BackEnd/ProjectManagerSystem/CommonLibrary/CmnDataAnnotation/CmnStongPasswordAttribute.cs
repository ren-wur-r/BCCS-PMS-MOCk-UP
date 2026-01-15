using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CommonLibrary.CmnDataAnnotation;

/// <summary>通用-強密碼-屬性</summary>
public class CmnStongPasswordAttribute : ValidationAttribute
{

    /// <summary>
    /// 密碼正則表達式：
    /// - 至少8字元
    /// - 至少1個英文大寫字母 (A-Z)
    /// - 至少1個英文小寫字母 (a-z)  
    /// - 至少1個數字 (0-9)
    /// - 至少1個特殊字元 (!@#$%^&*()_+-=[]{}|;:,.<>?)
    /// </summary>
    private const string STRONG_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]).{8,}$";

    public CmnStongPasswordAttribute()
    {

    }

    /// <summary>是否合法</summary>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var password = value as string;
        if (string.IsNullOrWhiteSpace(password))
        {
            return new ValidationResult("密碼不能為空");
        }

        if (Regex.IsMatch(password, STRONG_PASSWORD) == false)
        {
            return new ValidationResult("密碼至少8字元，包含英文大小寫字母、數字和特殊字元");
        }

        return ValidationResult.Success;
    }
}