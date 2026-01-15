/** 資料庫-原子-列舉  */
export enum DbAtomMenuEnum {
  /** 未定義 */
  Undefined = 0,
  // ------------------------------------------------
  /** ERP 5000 企業資源規劃 (Enterprise Resource Planning) */
  // Erp = 5000,

  /** ERP-發票結案 */
  ErpBill = 5100,
  // ------------------------------------------------
  /** Work 6000 工作 */
  // Work = 6000,

  /** 工作-專案管理 */
  WorkProject = 6100,

  /** 工作-工項管理 */
  WorkJob = 6200,
  // ------------------------------------------------
  /** CRM 7000 客戶關係管理 (Customer Relationship Management) */
  // Crm = 7000,

  /** CRM-商機管理 */
  CrmPipeline = 7100,

  /** CRM-Booking */
  CrmBooking = 7200,

  /** CRM-電銷管理 */
  CrmPhone = 7300,

  /** CRM-電銷客戶管理 */
  CrmPhoneSales = 7350,

  /** CRM-活動管理 */
  CrmActivity = 7400,
  // ------------------------------------------------
  /** Report 報表 8000 */
  // Report = 8000,

  /** 報表-產品同比 */
  ReportProductPeriod = 8100,

  /** 報表-產品趨勢 */
  ReportProductTrend = 8200,

  /** 報表-產品分析 */
  ReportProductAnalysis = 8300,

  /** 報表-窗口分析 */
  ReportContacterAnalysis = 8400,

  /** 報表-客戶分析 */
  ReportCompanyAnalysis = 8500,

  /** 報表-活動分析 */
  ReportActivityAnalysis = 8600,
  // ------------------------------------------------
  /** System 系統設定 9000 */
  // System = 9000,

  /** 系統設定-窗口 */
  SystemContacter = 9100,

  /** 系統設定-客戶 */
  SystemCompany = 9200,

  /** 系統設定-產品 */
  SystemProduct = 9300,

  /** 系統設定-員工 */
  SystemEmployee = 9400,

  /** 系統設定-角色 */
  SystemRole = 9500,

  /** 系統設定-部門 */
  SystemDepartment = 9600,

  /** 系統設定-地區 */
  SystemRegion = 9700,
}
