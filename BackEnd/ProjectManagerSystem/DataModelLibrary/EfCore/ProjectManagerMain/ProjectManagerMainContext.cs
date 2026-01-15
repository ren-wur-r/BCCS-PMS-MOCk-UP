using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

public partial class ProjectManagerMainContext : DbContext
{
    public ProjectManagerMainContext(DbContextOptions<ProjectManagerMainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AtomMenu> AtomMenu { get; set; }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<EmployeePermission> EmployeePermission { get; set; }

    public virtual DbSet<EmployeePipeline> EmployeePipeline { get; set; }

    public virtual DbSet<EmployeePipelineBill> EmployeePipelineBill { get; set; }

    public virtual DbSet<EmployeePipelineBooking> EmployeePipelineBooking { get; set; }

    public virtual DbSet<EmployeePipelineContacter> EmployeePipelineContacter { get; set; }

    public virtual DbSet<EmployeePipelineDue> EmployeePipelineDue { get; set; }

    public virtual DbSet<EmployeePipelineOriginal> EmployeePipelineOriginal { get; set; }

    public virtual DbSet<EmployeePipelinePhone> EmployeePipelinePhone { get; set; }

    public virtual DbSet<EmployeePipelineProduct> EmployeePipelineProduct { get; set; }

    public virtual DbSet<EmployeePipelineSaler> EmployeePipelineSaler { get; set; }

    public virtual DbSet<EmployeePipelineSalerTracking> EmployeePipelineSalerTracking { get; set; }

    public virtual DbSet<EmployeeProject> EmployeeProject { get; set; }

    public virtual DbSet<EmployeeProjectContract> EmployeeProjectContract { get; set; }

    public virtual DbSet<EmployeeProjectExpense> EmployeeProjectExpense { get; set; }

    public virtual DbSet<EmployeeProjectMember> EmployeeProjectMember { get; set; }

    public virtual DbSet<EmployeeProjectStone> EmployeeProjectStone { get; set; }

    public virtual DbSet<EmployeeProjectStoneJob> EmployeeProjectStoneJob { get; set; }

    public virtual DbSet<EmployeeProjectStoneJobBucket> EmployeeProjectStoneJobBucket { get; set; }

    public virtual DbSet<EmployeeProjectStoneJobExecutor> EmployeeProjectStoneJobExecutor { get; set; }

    public virtual DbSet<EmployeeProjectStoneJobWork> EmployeeProjectStoneJobWork { get; set; }

    public virtual DbSet<EmployeeProjectStoneJobWorkFile> EmployeeProjectStoneJobWorkFile { get; set; }

    public virtual DbSet<EmployeeProjectWork> EmployeeProjectWork { get; set; }

    public virtual DbSet<ManagerActivity> ManagerActivity { get; set; }

    public virtual DbSet<ManagerActivityExpense> ManagerActivityExpense { get; set; }

    public virtual DbSet<ManagerActivityProduct> ManagerActivityProduct { get; set; }

    public virtual DbSet<ManagerActivitySponsor> ManagerActivitySponsor { get; set; }

    public virtual DbSet<ManagerActivitySurveyAnswerer> ManagerActivitySurveyAnswerer { get; set; }

    public virtual DbSet<ManagerActivitySurveyAnswererItem> ManagerActivitySurveyAnswererItem { get; set; }

    public virtual DbSet<ManagerActivitySurveyQuestion> ManagerActivitySurveyQuestion { get; set; }

    public virtual DbSet<ManagerActivitySurveyQuestionItem> ManagerActivitySurveyQuestionItem { get; set; }

    public virtual DbSet<ManagerCompany> ManagerCompany { get; set; }

    public virtual DbSet<ManagerCompanyAffiliate> ManagerCompanyAffiliate { get; set; }

    public virtual DbSet<ManagerCompanyLocation> ManagerCompanyLocation { get; set; }

    public virtual DbSet<ManagerCompanyMainKind> ManagerCompanyMainKind { get; set; }

    public virtual DbSet<ManagerCompanySubKind> ManagerCompanySubKind { get; set; }

    public virtual DbSet<ManagerContacter> ManagerContacter { get; set; }

    public virtual DbSet<ManagerContacterRating> ManagerContacterRating { get; set; }

    public virtual DbSet<ManagerContacterRatingReason> ManagerContacterRatingReason { get; set; }

    public virtual DbSet<ManagerDepartment> ManagerDepartment { get; set; }

    public virtual DbSet<ManagerProduct> ManagerProduct { get; set; }

    public virtual DbSet<ManagerProductMainKind> ManagerProductMainKind { get; set; }

    public virtual DbSet<ManagerProductSpecification> ManagerProductSpecification { get; set; }

    public virtual DbSet<ManagerProductSubKind> ManagerProductSubKind { get; set; }

    public virtual DbSet<ManagerRegion> ManagerRegion { get; set; }

    public virtual DbSet<ManagerRole> ManagerRole { get; set; }

    public virtual DbSet<ManagerRolePermission> ManagerRolePermission { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_BIN2");

        modelBuilder.Entity<AtomMenu>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("原子目錄"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("ID");
            entity.Property(e => e.Description)
                .IsFixedLength()
                .HasComment("目錄說明")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.Account)
                .IsFixedLength()
                .HasComment("帳號");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.Email)
                .IsFixedLength()
                .HasComment("電子信箱");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.ManagerRoleID).HasComment("管理者角色ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Password)
                .IsFixedLength()
                .HasComment("密碼");
            entity.Property(e => e.Remark)
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePermission>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工權限"));

            entity.Property(e => e.EmployeeID).HasComment("員工ID");
            entity.Property(e => e.AtomMenuID).HasComment("原子目錄ID");
            entity.Property(e => e.AtomPermissionKindIdCreate).HasComment("權限類型ID新增");
            entity.Property(e => e.AtomPermissionKindIdDelete).HasComment("權限類型ID刪除");
            entity.Property(e => e.AtomPermissionKindIdEdit).HasComment("權限類型ID編輯");
            entity.Property(e => e.AtomPermissionKindIdView).HasComment("權限類型ID檢視");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerRegionID).HasComment("管理者區域ID");
        });

        modelBuilder.Entity<EmployeePipeline>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機"));

            entity.Property(e => e.ID).HasComment("商機ID");
            entity.Property(e => e.AtomPipelineStatusID).HasComment("商機狀態");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.ManagerCompanyID).HasComment("客戶公司ID");
            entity.Property(e => e.ManagerCompanyLocationID).HasComment("客戶公司營業地點ID");
            entity.Property(e => e.SalerEmployeeID).HasComment("業務員工ID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineBill>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EmployeePipelineBillTracking");

            entity.ToTable(tb => tb.HasComment("商機發票紀錄"));

            entity.Property(e => e.ID).HasComment("商機發票紀錄ID");
            entity.Property(e => e.BillNumber)
                .IsFixedLength()
                .HasComment("發票號碼");
            entity.Property(e => e.BillTime).HasComment("發票日期");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.NoTaxAmount).HasComment("未稅發票金額");
            entity.Property(e => e.PeriodNumber).HasComment("發票期數");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineBooking>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機Booking"));

            entity.Property(e => e.ID).HasComment("商機BookingID");
            entity.Property(e => e.Content).HasComment("需求內容");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.ManagerProductID).HasComment("管理者產品ID");
            entity.Property(e => e.ManagerProductSpecificationID).HasComment("管理者產品規格ID");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineContacter>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機窗口"));

            entity.Property(e => e.ID).HasComment("商機窗口ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.IsPrimary)
                .HasDefaultValue(true)
                .HasComment("是否為主要商機窗口");
            entity.Property(e => e.ManagerContacterID).HasComment("窗口ID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineDue>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機履約期限"));

            entity.Property(e => e.ID).HasComment("商機履約通知ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.DueTime).HasComment("履約日期");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .HasComment("備注")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineOriginal>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機原始資料"));

            entity.Property(e => e.EmployeePipelineID)
                .ValueGeneratedNever()
                .HasComment("商機ID");
            entity.Property(e => e.AtomCityID).HasComment("縣市ID");
            entity.Property(e => e.AtomCustomerGradeID).HasComment("客戶分級ID");
            entity.Property(e => e.AtomEmployeeRangeID).HasComment("人員規模ID");
            entity.Property(e => e.AtomRatingKindID).HasComment("窗口開發評等ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerCompanyLocationAddress)
                .IsFixedLength()
                .HasComment("公司營業地點地址")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerCompanyLocationStatus).HasComment("公司營業地點狀態");
            entity.Property(e => e.ManagerCompanyLocationTelephone)
                .IsFixedLength()
                .HasComment("公司營業地點電話");
            entity.Property(e => e.ManagerCompanyMainKindName)
                .IsFixedLength()
                .HasComment("公司主類別名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerCompanyName)
                .IsFixedLength()
                .HasComment("客戶公司名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerCompanySubKindName)
                .IsFixedLength()
                .HasComment("公司子類別")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerCompanyUnifiedNumber)
                .IsFixedLength()
                .HasComment("公司統一編號");
            entity.Property(e => e.ManagerContacterDepartment)
                .IsFixedLength()
                .HasComment("窗口部門")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerContacterEmail).IsFixedLength();
            entity.Property(e => e.ManagerContacterIsConsent).HasComment("窗口是否個資同意");
            entity.Property(e => e.ManagerContacterJobTitle)
                .IsFixedLength()
                .HasComment("窗口職稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerContacterName)
                .IsFixedLength()
                .HasComment("窗口姓名")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerContacterPhone)
                .IsFixedLength()
                .HasComment("窗口電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerContacterStatus).HasComment("窗口在職狀態");
            entity.Property(e => e.ManagerContacterTelephone)
                .IsFixedLength()
                .HasComment("窗口電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.TeamsFirstJoinTime).HasComment("Teams首次加入時間");
            entity.Property(e => e.TeamsLastLeaveTime).HasComment("Teams上次離開時間");
            entity.Property(e => e.TeamsMeetingDuration)
                .IsFixedLength()
                .HasComment("Teams會議持續時間")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.TeamsRegistrationStatus)
                .IsFixedLength()
                .HasComment("Teams註冊狀態")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.TeamsRegistrationTime).HasComment("Teams註冊時間");
            entity.Property(e => e.TeamsRole)
                .IsFixedLength()
                .HasComment("TeamsRole")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelinePhone>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機電銷紀錄"));

            entity.Property(e => e.CreateEmployeeID).HasComment("電銷員工ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.ManagerContacterID).HasComment("窗口ID");
            entity.Property(e => e.RecordTime).HasComment("紀錄時間");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .HasComment("備注")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Title)
                .IsFixedLength()
                .HasComment("電銷主題")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineProduct>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機產品"));

            entity.Property(e => e.ID).HasComment("商機產品ID");
            entity.Property(e => e.ClosingPrice).HasComment("成交價");
            entity.Property(e => e.ContractKindID).HasComment("採購方式");
            entity.Property(e => e.ContractText)
                .IsFixedLength()
                .HasComment("採購文字");
            entity.Property(e => e.CostPrice).HasComment("成本");
            entity.Property(e => e.Count).HasComment("數量");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.ManagerCompanyID).HasComment("管理者公司ID");
            entity.Property(e => e.ManagerProductID).HasComment("管理者產品ID");
            entity.Property(e => e.ManagerProductSpecificationID).HasComment("管理者產品規格ID");
            entity.Property(e => e.PurchaseKindID).HasComment("新購/續約");
            entity.Property(e => e.SellPrice).HasComment("售價");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineSaler>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機業務"));

            entity.Property(e => e.ID).HasComment("商機BookingID");
            entity.Property(e => e.CreateEmployeeID).HasComment("指派員工ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .HasComment("備注")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.SalerEmployeeID).HasComment("業務員工ID");
            entity.Property(e => e.SalerReplyTime).HasComment("業務回覆時間");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeePipelineSalerTracking>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("商機業務開發紀錄"));

            entity.Property(e => e.ID).HasComment("商機業務開發紀錄ID");
            entity.Property(e => e.CreateEmployeeID).HasComment("業務員工ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("商機ID");
            entity.Property(e => e.ManagerContacterID).HasComment("窗口ID");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .HasComment("備注")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.TrackingTime).HasComment("開發時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProject>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案"));

            entity.Property(e => e.AtomEmployeeProjectStatusID).HasComment("原子-員工專案狀態");
            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineID).HasComment("員工商機ID");
            entity.Property(e => e.EndTime).HasComment("迄止時間");
            entity.Property(e => e.ManagerCompanyID).HasComment("管理者公司ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.StartTime).HasComment("起始時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectContract>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案契約"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.IsNewest).HasComment("是否最新");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Url)
                .IsFixedLength()
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
        });

        modelBuilder.Entity<EmployeeProjectExpense>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案收支"));

            entity.Property(e => e.ID).HasComment("");
            entity.Property(e => e.ActualAmount).HasComment("實際金額");
            entity.Property(e => e.BillNumber)
                .IsFixedLength()
                .HasComment("發票號碼");
            entity.Property(e => e.BillTime).HasComment("發票日期");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱");
            entity.Property(e => e.PredictAmount).HasComment("預估金額");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectMember>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案人員"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeID).HasComment("員工ID");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectStone>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案里程碑"));

            entity.Property(e => e.AtomEmployeeProjectStatusID).HasComment("原子-員工專案狀態");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EndTime).HasComment("結束時間");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.PreNotifyDay).HasComment("前N日通知");
            entity.Property(e => e.StartTime).HasComment("開始時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectStoneJob>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EmployeeProjectPlanTask");

            entity.ToTable(tb => tb.HasComment("員工專案里程碑工項"));

            entity.Property(e => e.AtomEmployeeProjectStatusID).HasComment("原子-員工專案狀態");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EmployeeProjectStoneID).HasComment("員工專案里程碑ID");
            entity.Property(e => e.EndTime).HasComment("結束時間");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.StartTime).HasComment("開始時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.WorkHour).HasComment("工時");
        });

        modelBuilder.Entity<EmployeeProjectStoneJobBucket>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EmployeeProjectPlanTaskBucket");

            entity.ToTable(tb => tb.HasComment("員工專案里程碑工項清單"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EmployeeProjectStoneID).HasComment("員工專案里程碑ID");
            entity.Property(e => e.EmployeeProjectStoneJobID).HasComment("員工專案里程碑工項ID");
            entity.Property(e => e.IsFinish).HasComment("是否完成");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectStoneJobExecutor>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeProjectID, e.EmployeeProjectStoneID, e.EmployeeProjectStoneJobID, e.EmployeeID }).HasName("PK_EmployeeProjectStoneTaskExecutor");

            entity.ToTable(tb => tb.HasComment("員工專案里程碑工項執行者"));

            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EmployeeProjectStoneID).HasComment("員工專案里程碑ID");
            entity.Property(e => e.EmployeeProjectStoneJobID).HasComment("員工專案里程碑工項ID");
            entity.Property(e => e.EmployeeID).HasComment("員工ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectStoneJobWork>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案里程碑工項工作"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeID).HasComment("員工ID");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EmployeeProjectStoneID).HasComment("員工專案里程碑ID");
            entity.Property(e => e.EmployeeProjectStoneJobID).HasComment("員工專案里程碑工項ID");
            entity.Property(e => e.EndTime).HasComment("迄止時間");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.StartTime).HasComment("開始時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<EmployeeProjectStoneJobWorkFile>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案里程碑工項工作檔案"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.EmployeeProjectStoneID).HasComment("員工專案里程碑ID");
            entity.Property(e => e.EmployeeProjectStoneJobID).HasComment("員工專案里程碑工項ID");
            entity.Property(e => e.EmployeeProjectStoneJobWorkID).HasComment("員工專案里程碑工項工作ID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Url)
                .IsFixedLength()
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
        });

        modelBuilder.Entity<EmployeeProjectWork>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("員工專案工作計畫書"));

            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeeProjectID).HasComment("員工專案ID");
            entity.Property(e => e.IsNewest).HasComment("是否最新");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Url)
                .IsFixedLength()
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
        });

        modelBuilder.Entity<ManagerActivity>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ManagerAcitivty");

            entity.ToTable(tb => tb.HasComment("活動"));

            entity.Property(e => e.ID).HasComment("活動ID");
            entity.Property(e => e.AtomManagerActivityKindID).HasComment("活動類型");
            entity.Property(e => e.Content)
                .IsFixedLength()
                .HasComment("內容")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.EmployeePipelineCount).HasComment("商機數");
            entity.Property(e => e.EndTime).HasComment("結束時間");
            entity.Property(e => e.ExpectedLeadCount).HasComment("期望名單數");
            entity.Property(e => e.Location)
                .IsFixedLength()
                .HasComment("地點")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("活動名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.StartTime).HasComment("開始時間");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivityExpense>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("活動支出"));

            entity.Property(e => e.ID).HasComment("贊助ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("建立時間");
            entity.Property(e => e.ExpenseItem)
                .IsFixedLength()
                .HasComment("支出項目")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.Quantity).HasComment("數量");
            entity.Property(e => e.TotalAmount).HasComment("總金額");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivityProduct>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ManagerAcitivtyProduct");

            entity.Property(e => e.ID).HasComment("活動產品ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("建立時間");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.ManagerProductID).HasComment("產品ID");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivitySponsor>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者活動贊助商"));

            entity.Property(e => e.ID).HasComment("贊助ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("建立時間");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.SponsorAmount).HasComment("贊助金額");
            entity.Property(e => e.SponsorName)
                .IsFixedLength()
                .HasComment("贊助廠商名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivitySurveyAnswerer>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者活動問卷回答者"));

            entity.Property(e => e.ID).HasComment("管理者活動問卷ID");
            entity.Property(e => e.CompanyAddress)
                .IsFixedLength()
                .HasComment("公司地址")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CompanyBudgetID).HasComment("公司預算ID");
            entity.Property(e => e.CompanyName)
                .IsFixedLength()
                .HasComment("公司名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CompanyPhone)
                .IsFixedLength()
                .HasComment("公司電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CompanyPurchaseID).HasComment("公司採購ID");
            entity.Property(e => e.CompanyScaleID).HasComment("公司規模ID");
            entity.Property(e => e.ContacterDepartment)
                .IsFixedLength()
                .HasComment("窗口部門")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ContacterEmail)
                .IsFixedLength()
                .HasComment("窗口信箱");
            entity.Property(e => e.ContacterJobTitle)
                .IsFixedLength()
                .HasComment("窗口職稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ContacterName)
                .IsFixedLength()
                .HasComment("窗口名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ContacterPhone)
                .IsFixedLength()
                .HasComment("窗口手機")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ContacterTelephone)
                .IsFixedLength()
                .HasComment("窗口電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.FeedbackScheduleID).HasComment("回饋時程");
            entity.Property(e => e.FeedbackTargetID).HasComment("回饋目標");
            entity.Property(e => e.IsConsent).HasComment("是否個資同意");
            entity.Property(e => e.ManagerActivityID).HasComment("管理者活動ID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivitySurveyAnswererItem>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者活動問卷回答項目"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("管理者活動問卷回答項目ID");
            entity.Property(e => e.Content)
                .IsFixedLength()
                .HasComment("回答內容");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsChecked).HasComment("是否勾選");
            entity.Property(e => e.ManagerActivitySurveyAnswererID).HasComment("管理者活動問卷回答者ID");
            entity.Property(e => e.ManagerActivitySurveyQuestionID).HasComment("管理者活動問卷問題ID");
            entity.Property(e => e.ManagerActivitySurveyQuestionItemID).HasComment("管理者活動問卷問題項目ID");
            entity.Property(e => e.RatingValue).HasComment("星等評分");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivitySurveyQuestion>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者活動問卷問題"));

            entity.Property(e => e.ID).HasComment("活動問卷問題ID");
            entity.Property(e => e.Content)
                .IsFixedLength()
                .HasComment("問題說明")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsOther).HasComment("新增其他選項");
            entity.Property(e => e.KindID).HasComment("問題類型ID");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.Sort).HasComment("排序");
            entity.Property(e => e.Title)
                .IsFixedLength()
                .HasComment("問題標題")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerActivitySurveyQuestionItem>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者活動問卷問題項目"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerActivityID).HasComment("活動ID");
            entity.Property(e => e.ManagerActivitySurveyQuestionID).HasComment("活動問卷問題ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("項目名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Sort).HasComment("排序");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerCompany>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("客戶公司"));

            entity.Property(e => e.ID).HasComment("客戶公司ID");
            entity.Property(e => e.AtomCustomerGradeID)
                .HasDefaultValue((short)-1)
                .HasComment("客戶分級ID");
            entity.Property(e => e.AtomEmployeeRangeID)
                .HasDefaultValue((short)-1)
                .HasComment("人員規模ID");
            entity.Property(e => e.AtomSecurityGradeID)
                .HasDefaultValue((short)-1)
                .HasComment("資安責任等級ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerCompanyMainKindID).HasComment("客戶公司主分類ID");
            entity.Property(e => e.ManagerCompanySubKindID).HasComment("客戶公司子分類ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("客戶公司名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.UnifiedNumber)
                .IsFixedLength()
                .HasComment("統一編號");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerCompanyAffiliate>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ManagerAffiliateCompany");

            entity.ToTable(tb => tb.HasComment("關係客戶公司"));

            entity.Property(e => e.ID).HasComment("關係客戶公司ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsDeleted).HasComment("是否刪除");
            entity.Property(e => e.ManagerCompanyID).HasComment("客戶公司ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("關係客戶公司名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UnifiedNumber)
                .IsFixedLength()
                .HasComment("統一編號");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerCompanyLocation>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ManagerSubCompany");

            entity.ToTable(tb => tb.HasComment("客戶公司營業地點"));

            entity.Property(e => e.ID).HasComment("客戶公司營業地點ID");
            entity.Property(e => e.Address)
                .IsFixedLength()
                .HasComment("地址")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.AtomCityID).HasComment("縣市ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsDeleted).HasComment("是否刪除");
            entity.Property(e => e.ManagerCompanyID).HasComment("客戶公司ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("公司營業地點名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Status)
                .HasDefaultValue((short)-1)
                .HasComment("狀態");
            entity.Property(e => e.Telephone)
                .IsFixedLength()
                .HasComment("電話");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerCompanyMainKind>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("客戶公司主分類"));

            entity.Property(e => e.ID).HasComment("客戶公司主分類ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("客戶公司主分類名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerCompanySubKind>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("客戶公司子分類"));

            entity.Property(e => e.ID).HasComment("客戶公司子分類ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.ManagerCompanyMainKindID).HasComment("客戶公司主分類ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("客戶公司子分類名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerContacter>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("窗口"));

            entity.Property(e => e.ID).HasComment("窗口ID");
            entity.Property(e => e.AtomRatingKindID).HasComment("開發評等ID");
            entity.Property(e => e.CreateEmployeeID).HasComment("建立者員工ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.Department)
                .IsFixedLength()
                .HasComment("部門")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Email).IsFixedLength();
            entity.Property(e => e.IsConsent).HasComment("是否個資同意");
            entity.Property(e => e.JobTitle)
                .IsFixedLength()
                .HasComment("職稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.ManagerCompanyID).HasComment("公司ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("窗口姓名")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Phone)
                .IsFixedLength()
                .HasComment("電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.Telephone)
                .IsFixedLength()
                .HasComment("電話")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerContacterRating>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("窗口開發評等"));

            entity.Property(e => e.ID).HasComment("窗口開發評等ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerContacterID).HasComment("窗口ID");
            entity.Property(e => e.ManagerContacterRatingReasonID).HasComment("窗口開發評等原因ID");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerContacterRatingReason>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("窗口開發評等原因"));

            entity.Property(e => e.ID).HasComment("窗口開發評等原因ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerDepartment>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者部門"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.ParentDepartmentID).HasComment("上層部門ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("管理者部門名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerProduct>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者產品"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.IsKey).HasComment("是否為主力產品");
            entity.Property(e => e.KindID).HasComment("產品類型");
            entity.Property(e => e.ManagerProductMainKindID).HasComment("產品主分類ID");
            entity.Property(e => e.ManagerProductSubKindID).HasComment("產品子分類ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("產品名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Remark)
                .IsFixedLength()
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerProductMainKind>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者產品主分類"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CommissionRate).HasComment("業務毛利率");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("產品主分類名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerProductSpecification>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者產品規格"));

            entity.Property(e => e.ID)
                .ValueGeneratedOnAdd()
                .HasComment("ID");
            entity.Property(e => e.ManagerProductID).HasComment("管理者產品ID");
            entity.Property(e => e.CostPrice).HasComment("產品成本");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("產品規格名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.SellPrice).HasComment("產品售價");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerProductSubKind>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者產品子分類"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.ManagerProductMainKindID).HasComment("管理者產品主分類ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("產品子分類名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerRegion>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者地區"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("管理者地區名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerRole>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者角色"));

            entity.Property(e => e.ID).HasComment("ID");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.IsEnable).HasComment("是否啟用");
            entity.Property(e => e.ManagerDepartmentID).HasComment("管理者部門ID");
            entity.Property(e => e.ManagerRegionID).HasComment("管理者地區ID");
            entity.Property(e => e.Name)
                .IsFixedLength()
                .HasComment("名稱")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.Remark)
                .HasComment("備註")
                .UseCollation("Latin1_General_100_CS_AS_KS_WS_SC_UTF8");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
        });

        modelBuilder.Entity<ManagerRolePermission>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("管理者角色權限"));

            entity.Property(e => e.ManagerRoleID).HasComment("管理者角色ID");
            entity.Property(e => e.AtomMenuID).HasComment("原子目錄ID");
            entity.Property(e => e.AtomPermissionKindIdCreate).HasComment("權限類型ID新增");
            entity.Property(e => e.AtomPermissionKindIdDelete).HasComment("權限類型ID刪除");
            entity.Property(e => e.AtomPermissionKindIdEdit).HasComment("權限類型ID編輯");
            entity.Property(e => e.AtomPermissionKindIdView).HasComment("權限類型ID檢視");
            entity.Property(e => e.CreateTime).HasComment("建立時間");
            entity.Property(e => e.ManagerRegionID).HasComment("管理者區域ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
