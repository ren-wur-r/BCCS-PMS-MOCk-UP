using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;
using DataModelLibrary.Database.AtomPhoneSalesCustomerValue;
using DataModelLibrary.Database.AtomPhoneSalesDataSource;

namespace ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Model;

public class PhoneSalesCustomerMemoryMdl
{
    public int ID { get; set; }
    public int OwnerEmployeeID { get; set; }
    public DbAtomPhoneSalesDataSourceEnum AtomPhoneSalesDataSource { get; set; }
    public int? ManagerActivityID { get; set; }
    public string CompanyName { get; set; }
    public string UnifiedNumber { get; set; }
    public string Industry { get; set; }
    public string CompanyScale { get; set; }
    public string CompanyPhone { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyWebsite { get; set; }
    public DbAtomPhoneSalesCustomerStatusEnum AtomPhoneSalesCustomerStatus { get; set; }
    public DbAtomPhoneSalesCustomerValueEnum AtomPhoneSalesCustomerValue { get; set; }
    public decimal ValueScore { get; set; }
    public bool IsExistingCustomer { get; set; }
    public int? ExistingManagerCompanyID { get; set; }
    public string ComparisonMethod { get; set; }
    public DateTimeOffset? LastComparisonTime { get; set; }
    public int ContactCount { get; set; }
    public DateTimeOffset? LastContactTime { get; set; }
    public int? LastContactEmployeeID { get; set; }
    public DateTimeOffset? NextFollowUpTime { get; set; }
    public int CustomOrder { get; set; }
    public string Tags { get; set; }
    public string Remark { get; set; }
    public bool IsDeleted { get; set; }
    public int CreateEmployeeID { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public int? UpdateEmployeeID { get; set; }
    public DateTimeOffset UpdateTime { get; set; }

    public List<PhoneSalesContacterMemoryMdl> ContacterList { get; set; } = new List<PhoneSalesContacterMemoryMdl>();
}

public class PhoneSalesContacterMemoryMdl
{
    public int ID { get; set; }
    public int PhoneSalesCustomerID { get; set; }
    public string ContacterName { get; set; }
    public string ContacterJobTitle { get; set; }
    public string ContacterDepartment { get; set; }
    public string ContacterPhone { get; set; }
    public string ContacterMobile { get; set; }
    public string ContacterEmail { get; set; }
    public bool IsPrimary { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset UpdateTime { get; set; }
}
