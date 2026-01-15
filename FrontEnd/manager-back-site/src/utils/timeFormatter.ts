/**
 * 將 ISO 或其他日期字串轉為 datetime-local 可用格式 (YYYY-MM-DDTHH:mm)
 * 用於 input[type="datetime-local"]
 * @param dateTimeString 日期時間字串 (例如 "2025-10-31T20:30:00+08:00")
 * @returns 格式化後字串 (例如 "2025-10-31T20:30")
 */
export const formatToDatetimeLocal = (dateTimeString: string | null | undefined): string => {
  if (!dateTimeString) return "";

  try {
    const date = new Date(dateTimeString);

    // 無效日期檢查
    if (isNaN(date.getTime())) {
      return dateTimeString;
    }

    // 調整為本地時間
    const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);

    const isoString = localDate.toISOString();
    return isoString.slice(0, 16); // 取 YYYY-MM-DDTHH:mm
  } catch (error) {
    console.warn("formatToDatetimeLocal 格式化錯誤:", error);
    return dateTimeString;
  }
};

/**
 * 將 datetime-local 格式 (YYYY-MM-DDTHH:mm) 轉回 ISO 格式
 * 特性：使用在時間是【使用者選擇】上
 * 用於送回後端 (例如 2025-10-31T12:30:00.000+08:00)
 * @param localDateTime 本地時間字串
 * @returns ISO 格式時間字串
 */
export const formatToServerDatetime = (localDateTime: string | null | undefined): string => {
  if (!localDateTime) return "";

  try {
    const date = new Date(localDateTime);
    if (isNaN(date.getTime())) return localDateTime;

    // 抓出日期部分（YYYY-MM-DDTHH:mm）
    const yyyy = localDateTime.slice(0, 4);
    const mm = localDateTime.slice(5, 7);
    const dd = localDateTime.slice(8, 10);
    const hh = localDateTime.slice(11, 13);
    const mi = localDateTime.slice(14, 16);

    return `${yyyy}-${mm}-${dd}T${hh}:${mi}:00+08:00`;
  } catch (error) {
    console.warn("formatToServerDatetimeISO8 格式化錯誤:", error);
    return localDateTime ?? "";
  }
};

/**
 * 將 Date 物件或 YYYY-MM-DD 字串格式化成含台灣時區 (UTC+8) 的
 * 「當日開始時間」ISO-like 字串。
 *
 * 格式（統一）：
 *   YYYY-MM-DDT00:00:00.000+08:00
 *
 * 特性：
 * - 支援 Date 或 YYYY-MM-DD 字串
 * - 自動解析年月日
 * - 字串會做基本格式驗證（不合法時回傳原字串）
 * - 固定強制為當日 00:00:00.000 +08:00，不做 UTC 轉換
 * - 使用在【日期】為開始【時間固定送0:00:00】上
 * 用途：
 * - 查詢區間開始時間
 * - 傳給後端的固定時間格式
 *
 * @param local Date | string | null | undefined
 * @returns string 格式化日期，若無效則回傳空字串
 */
export const formatToServerDateStartISO8 = (local: Date | string | null | undefined): string => {
  if (!local) return "";

  let y: string, m: string, d: string;

  try {
    // Case 1：Date
    if (local instanceof Date) {
      if (isNaN(local.getTime())) return "";
      y = String(local.getFullYear());
      m = String(local.getMonth() + 1).padStart(2, "0");
      d = String(local.getDate()).padStart(2, "0");
    }
    // Case 2：字串 YYYY-MM-DD
    else if (typeof local === "string") {
      const match = local.match(/^(\d{4})-(\d{2})-(\d{2})$/);
      if (!match) return local; // 格式不對 → 直接回傳原字串

      [y, m, d] = match.slice(1);
    } else {
      return "";
    }

    return `${y}-${m}-${d}T00:00:00.000+08:00`;
  } catch (err) {
    console.warn("formatToServerDateStartISO8 格式化錯誤:", err);
    return "";
  }
};

/**
 * 將 Date 物件或 YYYY-MM-DD 字串格式化為 UTC+8 的當日結束時間。
 *
 * 格式（統一）：
 *   YYYY-MM-DDT23:59:59.999+08:00
 *
 * 特性：
 * - 支援 Date 或 字串輸入
 * - 字串需為 YYYY-MM-DD 格式（會套用基本驗證）
 * - 若格式不合法，直接回傳原字串避免中斷
 * - 強制使用台灣時區 UTC+8，不做 UTC 轉換
 * - 使用在【日期】為結束【時間固定送23:59:59】上
 * 用途：
 * - 查詢條件的結束日期
 * - 回傳該日的最後一毫秒（23:59:59.999）
 *
 * @param local Date | string | null | undefined
 * @returns string 轉換後的 ISO-like 時間字串；若無效則回傳空字串。
 */
export const formatToServerDateEndISO8 = (local: Date | string | null | undefined): string => {
  if (!local) return "";

  let y: string, m: string, d: string;

  try {
    // --- Case 1: Date 物件 ---
    if (local instanceof Date) {
      if (isNaN(local.getTime())) return "";
      y = String(local.getFullYear());
      m = String(local.getMonth() + 1).padStart(2, "0");
      d = String(local.getDate()).padStart(2, "0");
    }
    // --- Case 2: 字串 YYYY-MM-DD ---
    else if (typeof local === "string") {
      // 基本檢查：是否符合 YYYY-MM-DD
      const match = local.match(/^(\d{4})-(\d{2})-(\d{2})$/);
      if (!match) return local; // 不合法 → 直接回傳原字串

      [y, m, d] = match.slice(1);
    } else {
      return "";
    }

    return `${y}-${m}-${d}T23:59:59.999+08:00`;
  } catch (err) {
    console.warn("formatToServerDateEndISO8 格式化錯誤:", err);
    return "";
  }
};

/**
 * 格式化日期時間為 YYYY-MM-DD HH:mm 格式
 * @param dateTimeString 日期時間字符串
 * @returns 格式化後的時間字符串
 */
export const formatDateTime = (dateTimeString: string | null | undefined): string => {
  if (!dateTimeString) return "";

  try {
    const date = new Date(dateTimeString);

    // 檢查是否為有效日期
    if (isNaN(date.getTime())) {
      return dateTimeString;
    }

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");

    return `${year}-${month}-${day} ${hours}:${minutes}`;
  } catch (error) {
    console.warn("日期格式化錯誤:", error);
    return dateTimeString;
  }
};

/**
 * 格式化日期為 YYYY-MM-DD 格式
 * @param dateString 日期字符串
 * @returns 格式化後的日期字符串
 */
export const formatDate = (dateString: string | null | undefined): string => {
  if (!dateString) return "";

  try {
    const date = new Date(dateString);

    if (isNaN(date.getTime())) {
      return dateString;
    }

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
  } catch (error) {
    console.warn("日期格式化錯誤:", error);
    return dateString;
  }
};

/**
 * 格式化時間為 HH:mm
 */
export const formatTime = (dateTimeString: string | null | undefined): string => {
  if (!dateTimeString) return "";

  try {
    const date = new Date(dateTimeString);
    if (isNaN(date.getTime())) return dateTimeString;

    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");

    return `${hours}:${minutes}`;
  } catch {
    return dateTimeString ?? "";
  }
};
