/**
 * 格式化金額為貨幣顯示格式
 * @param amount 金額數字
 * @param currency 貨幣符號，預設為 '$'
 * @returns 格式化後的金額字符串，例如：$1,234,567
 */
export const formatCurrency = (amount: number | null | undefined, currency: string = '$'): string => {
  if (amount === null || amount === undefined) return `${currency}0`;
  
  // 處理負數
  const isNegative = amount < 0;
  const absoluteAmount = Math.abs(amount);
  
  // 格式化數字，加入千分位逗號
  const formattedNumber = absoluteAmount.toLocaleString('en-US');
  
  // 組合結果
  const result = `${currency}${formattedNumber}`;
  
  return isNegative ? `-${result}` : result;
};

/**
 * 格式化金額為美金顯示格式
 * @param amount 金額數字
 * @returns 格式化後的美金金額字符串，例如：$1,234,567
 */
export const formatUSDCurrency = (amount: number | null | undefined): string => {
  return formatCurrency(amount, '$');
};