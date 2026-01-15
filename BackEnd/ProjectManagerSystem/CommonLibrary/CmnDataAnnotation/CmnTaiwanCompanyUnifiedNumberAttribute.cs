using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CommonLibrary.CmnDataAnnotation;

/// <summary>通用-台灣公司統一編號驗證-屬性</summary>
public class CmnTaiwanCompanyUnifiedNumberAttribute : ValidationAttribute
{
    /// <summary>
    /// 台灣公司統一編號正則表達式：
    /// - 8位數字
    /// - 範例: 04595257
    /// </summary>
    private const string TAIWAN_UNIFIED_NUMBER = @"^\d{8}$";

    /// <summary>選轉乘數</summary>
    private readonly int[] _multipliers = { 1, 2, 1, 2, 1, 2, 4, 1 };

    public CmnTaiwanCompanyUnifiedNumberAttribute()
    {
    }

    /// <summary>是否合法</summary>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var unifiedNumber = value as string;
        if (string.IsNullOrWhiteSpace(unifiedNumber))
        {
            return ValidationResult.Success;
        }

        // 格式檢查：必須是8位數字
        if (Regex.IsMatch(unifiedNumber, TAIWAN_UNIFIED_NUMBER) == false)
        {
            return new ValidationResult("統一編號格式不正確，必須為8位數字");
        }

        // 檢查碼驗證
        if (IsValidUnifiedNumber(unifiedNumber) == false)
        {
            return new ValidationResult("統一編號檢查碼不正確");
        }

        return ValidationResult.Success;
    }

    /// <summary>驗證統一編號檢查碼</summary>
    private bool IsValidUnifiedNumber(string unifiedNumber)
    {
        var digits = new int[8];
        for (int i = 0; i < 8; i++)
        {
            digits[i] = int.Parse(unifiedNumber[i].ToString());
        }

        // 計算乘積並拆分為個位數相加
        var digitSum = 0;
        for (int i = 0; i < 8; i++)
        {
            var product = digits[i] * _multipliers[i];
            // 將乘積的十位和個位分開相加
            while (product > 0)
            {
                digitSum += product % 10;
                product /= 10;
            }
        }

        // 第7位（索引6，倒數第二位）不是7的情況
        if (digits[6] != 7)
        {
            return digitSum % 5 == 0;
        }

        // 第7位是7的情況：需要計算Z1和Z2
        // 重新計算，但最後一位的個位數需要分別取1或0

        // 計算前7位的和
        var sumExceptLast = 0;
        for (int i = 0; i < 7; i++)
        {
            var product = digits[i] * _multipliers[i];
            while (product > 0)
            {
                sumExceptLast += product % 10;
                product /= 10;
            }
        }

        // 最後一位的乘積（digits[7] * 1 = digits[7]）
        var lastProduct = digits[7] * _multipliers[7];
        var lastTens = lastProduct / 10;

        // Z1: 最後一位個位數取1
        var z1 = sumExceptLast + lastTens + 1;

        // Z2: 最後一位個位數取0
        var z2 = sumExceptLast + lastTens + 0;

        return (z1 % 5 == 0) || (z2 % 5 == 0);
    }
}

