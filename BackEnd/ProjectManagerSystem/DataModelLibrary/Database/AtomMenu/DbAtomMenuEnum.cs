namespace DataModelLibrary.Database.AtomMenu;

/// <summary>資料庫-原子-目錄-列舉</summary>
public enum DbAtomMenuEnum : short
{
    /// <summary>未定義</summary>
    Undefined = 0,

    #region ERP 5000 企業資源規劃 (Enterprise Resource Planning)

    ///// <summary>企業資源規劃</summary>
    //ERP = 5000,

    /// <summary>ERP-發票</summary>
    ErpBill = 5100,

    #endregion

    #region Work 6000 工作

    ///// <summary>工作</summary>
    //Work = 6000,

    /// <summary>工作-專案管理</summary>
    WorkProject = 6100,

    /// <summary>工作-工項管理</summary>
    WorkJob = 6200,

    #endregion

    #region CRM 7000 客戶關係管理 (Customer Relationship Management)

    ///// <summary>客戶關係管理</summary>
    //CRM = 7000,

    /// <summary>CRM-商機管理</summary>
    CrmPipeline = 7100,

    /// <summary>CRM-Booking</summary>
    CrmBooking = 7200,

    /// <summary>CRM-電銷管理</summary>
    CrmPhone = 7300,

    /// <summary>CRM-電銷客戶管理</summary>
    CrmPhoneSales = 7350,

    /// <summary>CRM-活動管理</summary>
    CrmActivity = 7400,

    #endregion

    #region Report 報表 8000

    ///// <summary>標準階段與類型設定</summary>
    //Standard = 8000,

    /// <summary>報表-產品同比</summary>
    ReportProductPeriod = 8100,

    /// <summary>報表-產品趨勢</summary>
    ReportProductTrend = 8200,

    /// <summary>報表-產品分析</summary>
    ReportProductAnalysis = 8300,

    /// <summary>報表-窗口分析</summary>
    ReportContacterAnalysis = 8400,

    /// <summary>報表-客戶分析</summary>
    ReportCompanyAnalysis = 8500,

    /// <summary>報表-活動分析</summary>
    ReportActivityAnalysis = 8600,

    #endregion

    #region System 系統設定 9000

    ///// <summary>系統設定</summary>
    //System = 9000,

    /// <summary>系統設定-窗口</summary>
    SystemContacter = 9100,

    /// <summary>系統設定-客戶</summary>
    SystemCompany = 9200,

    /// <summary>系統設定-產品</summary>
    SystemProduct = 9300,

    /// <summary>系統設定-員工</summary>
    SystemEmployee = 9400,

    /// <summary>系統設定-角色</summary>
    SystemRole = 9500,

    /// <summary>系統設定-部門</summary>
    SystemDepartment = 9600,

    /// <summary>系統設定-地區</summary>
    SystemRegion = 9700,

    #endregion
}