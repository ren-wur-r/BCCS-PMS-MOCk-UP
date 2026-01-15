USE [ProjectManagerMain]
GO

-- ====================================================================
-- 電銷管理系統資料表建立腳本
-- 建立日期: 2026-01-08
-- 說明: 建立電銷客戶管理所需的資料表
-- ====================================================================

-- 1. 建立電銷客戶狀態 Atom 表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AtomPhoneSalesCustomerStatus]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AtomPhoneSalesCustomerStatus](
        [ID] [smallint] NOT NULL,
        [Name] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [CreateTime] [datetimeoffset](3) NOT NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL,
        CONSTRAINT [PK_AtomPhoneSalesCustomerStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
    )

    INSERT INTO [dbo].[AtomPhoneSalesCustomerStatus] ([ID], [Name], [CreateTime], [UpdateTime])
    VALUES
    (1, N'待聯繫', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (2, N'已聯繫', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (3, N'已約訪', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (4, N'已派送', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (5, N'不再聯繫', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())
END
GO

-- 2. 建立客戶價值等級 Atom 表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AtomPhoneSalesCustomerValue]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AtomPhoneSalesCustomerValue](
        [ID] [smallint] NOT NULL,
        [Name] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [CreateTime] [datetimeoffset](3) NOT NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL,
        CONSTRAINT [PK_AtomPhoneSalesCustomerValue] PRIMARY KEY CLUSTERED ([ID] ASC)
    )

    INSERT INTO [dbo].[AtomPhoneSalesCustomerValue] ([ID], [Name], [CreateTime], [UpdateTime])
    VALUES
    (1, N'高價值', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (2, N'中價值', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (3, N'低價值', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (4, N'待評估', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())
END
GO

-- 3. 建立資料來源 Atom 表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AtomPhoneSalesDataSource]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AtomPhoneSalesDataSource](
        [ID] [smallint] NOT NULL,
        [Name] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [CreateTime] [datetimeoffset](3) NOT NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL,
        CONSTRAINT [PK_AtomPhoneSalesDataSource] PRIMARY KEY CLUSTERED ([ID] ASC)
    )

    INSERT INTO [dbo].[AtomPhoneSalesDataSource] ([ID], [Name], [CreateTime], [UpdateTime])
    VALUES
    (1, N'活動管理', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (2, N'電銷自建', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()),
    (3, N'批次匯入', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET())
END
GO

-- 4. 建立電銷客戶表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneSalesCustomer]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhoneSalesCustomer](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [OwnerEmployeeID] [int] NOT NULL,
        [AtomPhoneSalesDataSourceID] [smallint] NOT NULL,
        [ManagerActivityID] [int] NULL,
        [CompanyName] [nvarchar](300) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [UnifiedNumber] [varchar](10) COLLATE Latin1_General_100_CS NULL,
        [Industry] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [CompanyScale] [nvarchar](50) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [CompanyPhone] [varchar](50) COLLATE Latin1_General_100_CS NULL,
        [CompanyAddress] [nvarchar](500) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [CompanyWebsite] [varchar](500) COLLATE Latin1_General_100_CS NULL,
        [AtomPhoneSalesCustomerStatusID] [smallint] NOT NULL DEFAULT 1,
        [AtomPhoneSalesCustomerValueID] [smallint] NOT NULL DEFAULT 4,
        [ValueScore] [decimal](10, 2) NULL DEFAULT 0,
        [IsExistingCustomer] [bit] NOT NULL DEFAULT 0,
        [ExistingManagerCompanyID] [int] NULL,
        [ComparisonMethod] [nvarchar](50) COLLATE Latin1_General_100_CS NULL,
        [LastComparisonTime] [datetimeoffset](3) NULL,
        [ContactCount] [int] NOT NULL DEFAULT 0,
        [LastContactTime] [datetimeoffset](3) NULL,
        [LastContactEmployeeID] [int] NULL,
        [NextFollowUpTime] [datetimeoffset](3) NULL,
        [CustomOrder] [int] NOT NULL DEFAULT 0,
        [Tags] [nvarchar](500) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [Remark] [nvarchar](1000) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT 0,
        [CreateEmployeeID] [int] NOT NULL,
        [CreateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [UpdateEmployeeID] [int] NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT [PK_PhoneSalesCustomer] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [FK_PhoneSalesCustomer_OwnerEmployee] FOREIGN KEY([OwnerEmployeeID]) REFERENCES [dbo].[Employee] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_AtomDataSource] FOREIGN KEY([AtomPhoneSalesDataSourceID]) REFERENCES [dbo].[AtomPhoneSalesDataSource] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_AtomStatus] FOREIGN KEY([AtomPhoneSalesCustomerStatusID]) REFERENCES [dbo].[AtomPhoneSalesCustomerStatus] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_AtomValue] FOREIGN KEY([AtomPhoneSalesCustomerValueID]) REFERENCES [dbo].[AtomPhoneSalesCustomerValue] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_ExistingCompany] FOREIGN KEY([ExistingManagerCompanyID]) REFERENCES [dbo].[ManagerCompany] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_CreateEmployee] FOREIGN KEY([CreateEmployeeID]) REFERENCES [dbo].[Employee] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomer_UpdateEmployee] FOREIGN KEY([UpdateEmployeeID]) REFERENCES [dbo].[Employee] ([ID])
    )

    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomer_OwnerEmployee] ON [dbo].[PhoneSalesCustomer]([OwnerEmployeeID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomer_Status] ON [dbo].[PhoneSalesCustomer]([AtomPhoneSalesCustomerStatusID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomer_CompanyName] ON [dbo].[PhoneSalesCustomer]([CompanyName])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomer_UnifiedNumber] ON [dbo].[PhoneSalesCustomer]([UnifiedNumber])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomer_Industry] ON [dbo].[PhoneSalesCustomer]([Industry])
END
GO

-- 5. 建立電銷窗口表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneSalesContacter]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhoneSalesContacter](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [PhoneSalesCustomerID] [int] NOT NULL,
        [ContacterName] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [ContacterJobTitle] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [ContacterDepartment] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [ContacterPhone] [varchar](50) COLLATE Latin1_General_100_CS NULL,
        [ContacterMobile] [varchar](50) COLLATE Latin1_General_100_CS NULL,
        [ContacterEmail] [varchar](200) COLLATE Latin1_General_100_CS NOT NULL,
        [IsPrimary] [bit] NOT NULL DEFAULT 0,
        [CreateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [UpdateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT [PK_PhoneSalesContacter] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [FK_PhoneSalesContacter_Customer] FOREIGN KEY([PhoneSalesCustomerID]) REFERENCES [dbo].[PhoneSalesCustomer] ([ID])
    )

    CREATE NONCLUSTERED INDEX [IX_PhoneSalesContacter_Customer] ON [dbo].[PhoneSalesContacter]([PhoneSalesCustomerID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesContacter_Email] ON [dbo].[PhoneSalesContacter]([ContacterEmail])
END
GO

-- 6. 建立電銷記錄表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneSalesRecord]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhoneSalesRecord](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [PhoneSalesCustomerID] [int] NOT NULL,
        [PhoneSalesContacterID] [int] NULL,
        [ContactEmployeeID] [int] NOT NULL,
        [ContactTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [ContactContent] [nvarchar](2000) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [NegotiationItems] [nvarchar](1000) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [EstimatedAmount] [decimal](18, 2) NULL,
        [NextFollowUpTime] [datetimeoffset](3) NULL,
        [CreateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT [PK_PhoneSalesRecord] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [FK_PhoneSalesRecord_Customer] FOREIGN KEY([PhoneSalesCustomerID]) REFERENCES [dbo].[PhoneSalesCustomer] ([ID]),
        CONSTRAINT [FK_PhoneSalesRecord_Contacter] FOREIGN KEY([PhoneSalesContacterID]) REFERENCES [dbo].[PhoneSalesContacter] ([ID]),
        CONSTRAINT [FK_PhoneSalesRecord_Employee] FOREIGN KEY([ContactEmployeeID]) REFERENCES [dbo].[Employee] ([ID])
    )

    CREATE NONCLUSTERED INDEX [IX_PhoneSalesRecord_Customer] ON [dbo].[PhoneSalesRecord]([PhoneSalesCustomerID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesRecord_ContactTime] ON [dbo].[PhoneSalesRecord]([ContactTime] DESC)
END
GO

-- 7. 建立客戶資料修改歷程表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneSalesCustomerHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhoneSalesCustomerHistory](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [PhoneSalesCustomerID] [int] NOT NULL,
        [FieldName] [varchar](100) COLLATE Latin1_General_100_CS NOT NULL,
        [FieldDisplayName] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [OldValue] [nvarchar](max) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [NewValue] [nvarchar](max) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [UpdateEmployeeID] [int] NOT NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT [PK_PhoneSalesCustomerHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [FK_PhoneSalesCustomerHistory_Customer] FOREIGN KEY([PhoneSalesCustomerID]) REFERENCES [dbo].[PhoneSalesCustomer] ([ID]),
        CONSTRAINT [FK_PhoneSalesCustomerHistory_Employee] FOREIGN KEY([UpdateEmployeeID]) REFERENCES [dbo].[Employee] ([ID])
    )

    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomerHistory_Customer] ON [dbo].[PhoneSalesCustomerHistory]([PhoneSalesCustomerID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesCustomerHistory_UpdateTime] ON [dbo].[PhoneSalesCustomerHistory]([UpdateTime] DESC)
END
GO

-- 8. 建立窗口資料修改歷程表
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneSalesContacterHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PhoneSalesContacterHistory](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [PhoneSalesContacterID] [int] NOT NULL,
        [FieldName] [varchar](100) COLLATE Latin1_General_100_CS NOT NULL,
        [FieldDisplayName] [nvarchar](100) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NOT NULL,
        [OldValue] [nvarchar](max) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [NewValue] [nvarchar](max) COLLATE Latin1_General_100_CS_AS_KS_WS_SC_UTF8 NULL,
        [UpdateEmployeeID] [int] NOT NULL,
        [UpdateTime] [datetimeoffset](3) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT [PK_PhoneSalesContacterHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
        CONSTRAINT [FK_PhoneSalesContacterHistory_Contacter] FOREIGN KEY([PhoneSalesContacterID]) REFERENCES [dbo].[PhoneSalesContacter] ([ID]),
        CONSTRAINT [FK_PhoneSalesContacterHistory_Employee] FOREIGN KEY([UpdateEmployeeID]) REFERENCES [dbo].[Employee] ([ID])
    )

    CREATE NONCLUSTERED INDEX [IX_PhoneSalesContacterHistory_Contacter] ON [dbo].[PhoneSalesContacterHistory]([PhoneSalesContacterID])
    CREATE NONCLUSTERED INDEX [IX_PhoneSalesContacterHistory_UpdateTime] ON [dbo].[PhoneSalesContacterHistory]([UpdateTime] DESC)
END
GO

PRINT '電銷管理系統資料表建立完成'
GO
