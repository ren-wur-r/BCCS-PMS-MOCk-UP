using System;
using System.Linq;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.CRM.PhoneSales;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Model;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Service;

namespace ServiceLibrary.ManagerBackSite.Logical.PhoneSales.Service;

public class MbsPhsLogicalService : IMbsPhsLogicalService
{
    private readonly IMbsPhsMemoryService _mbsPhsMemoryService;
    private readonly ICoEmpLoginInfoMemoryService _coEmpLoginInfoMemoryService;
    private readonly ILogger<MbsPhsLogicalService> _logger;

    public MbsPhsLogicalService(
        IMbsPhsMemoryService mbsPhsMemoryService,
        ICoEmpLoginInfoMemoryService coEmpLoginInfoMemoryService,
        ILogger<MbsPhsLogicalService> logger)
    {
        _mbsPhsMemoryService = mbsPhsMemoryService;
        _coEmpLoginInfoMemoryService = coEmpLoginInfoMemoryService;
        _logger = logger;
    }

    public async Task<MbsCrmPhsCtlGetManyCustomerRspMdl> GetManyCustomerAsync(MbsCrmPhsCtlGetManyCustomerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlGetManyCustomerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var allCustomers = _mbsPhsMemoryService.GetAllCustomers();

            var filteredCustomers = allCustomers.AsEnumerable();

            if (reqObj.AtomPhoneSalesCustomerStatus.HasValue)
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.AtomPhoneSalesCustomerStatus == reqObj.AtomPhoneSalesCustomerStatus.Value);
            }

            if (reqObj.AtomPhoneSalesCustomerValue.HasValue)
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.AtomPhoneSalesCustomerValue == reqObj.AtomPhoneSalesCustomerValue.Value);
            }

            if (!string.IsNullOrWhiteSpace(reqObj.CompanyName))
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.CompanyName.Contains(reqObj.CompanyName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(reqObj.Industry))
            {
                filteredCustomers = filteredCustomers
                    .Where(c => !string.IsNullOrWhiteSpace(c.Industry) && c.Industry.Contains(reqObj.Industry.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(reqObj.ContacterName))
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.ContacterList.Any(ct => ct.ContacterName.Contains(reqObj.ContacterName.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(reqObj.ContacterEmail))
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.ContacterList.Any(ct => ct.ContacterEmail.Contains(reqObj.ContacterEmail.Trim())));
            }

            if (reqObj.IsExistingCustomer.HasValue)
            {
                filteredCustomers = filteredCustomers
                    .Where(c => c.IsExistingCustomer == reqObj.IsExistingCustomer.Value);
            }

            var totalCount = filteredCustomers.Count();

            var pagedCustomers = filteredCustomers
                .OrderBy(c => c.CustomOrder)
                .ThenByDescending(c => c.UpdateTime)
                .Skip((reqObj.PageIndex - 1) * reqObj.PageSize)
                .Take(reqObj.PageSize)
                .ToList();

            rspObj.CustomerList = pagedCustomers.Select(c =>
            {
                var primaryContacter = c.ContacterList.FirstOrDefault(ct => ct.IsPrimary)
                                     ?? c.ContacterList.FirstOrDefault();

                return new MbsCrmPhsCtlGetManyCustomerRspItemMdl
                {
                    PhoneSalesCustomerID = c.ID,
                    CompanyName = c.CompanyName,
                    UnifiedNumber = c.UnifiedNumber,
                    Industry = c.Industry,
                    CompanyScale = c.CompanyScale,
                    AtomPhoneSalesCustomerStatus = c.AtomPhoneSalesCustomerStatus,
                    AtomPhoneSalesCustomerValue = c.AtomPhoneSalesCustomerValue,
                    AtomPhoneSalesDataSource = c.AtomPhoneSalesDataSource,
                    IsExistingCustomer = c.IsExistingCustomer,
                    ContactCount = c.ContactCount,
                    LastContactTime = c.LastContactTime,
                    PrimaryContacterName = primaryContacter?.ContacterName,
                    PrimaryContacterEmail = primaryContacter?.ContacterEmail,
                    PrimaryContacterPhone = primaryContacter?.ContacterMobile ?? primaryContacter?.ContacterPhone,
                    CustomOrder = c.CustomOrder,
                    UpdateTime = c.UpdateTime
                };
            }).ToList();

            rspObj.TotalCount = totalCount;
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetManyCustomerAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }

    public async Task<MbsCrmPhsCtlGetCustomerRspMdl> GetCustomerAsync(MbsCrmPhsCtlGetCustomerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlGetCustomerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
            var loginInfo = _coEmpLoginInfoMemoryService.Get(coEmpLiMemGetReqObj);
            if (loginInfo == default)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
                return rspObj;
            }

            var customer = _mbsPhsMemoryService.GetCustomerById(reqObj.PhoneSalesCustomerID);
            if (customer == null)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            if (customer.OwnerEmployeeID != loginInfo.EmployeeID)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
                return rspObj;
            }

            rspObj.PhoneSalesCustomerID = customer.ID;
            rspObj.CompanyName = customer.CompanyName;
            rspObj.UnifiedNumber = customer.UnifiedNumber;
            rspObj.Industry = customer.Industry;
            rspObj.CompanyScale = customer.CompanyScale;
            rspObj.CompanyPhone = customer.CompanyPhone;
            rspObj.CompanyAddress = customer.CompanyAddress;
            rspObj.CompanyWebsite = customer.CompanyWebsite;
            rspObj.AtomPhoneSalesCustomerStatus = customer.AtomPhoneSalesCustomerStatus;
            rspObj.AtomPhoneSalesCustomerValue = customer.AtomPhoneSalesCustomerValue;
            rspObj.AtomPhoneSalesDataSource = customer.AtomPhoneSalesDataSource;
            rspObj.ValueScore = customer.ValueScore;
            rspObj.IsExistingCustomer = customer.IsExistingCustomer;
            rspObj.ExistingManagerCompanyID = customer.ExistingManagerCompanyID;
            rspObj.ComparisonMethod = customer.ComparisonMethod;
            rspObj.ContactCount = customer.ContactCount;
            rspObj.LastContactTime = customer.LastContactTime;
            rspObj.NextFollowUpTime = customer.NextFollowUpTime;
            rspObj.CustomOrder = customer.CustomOrder;
            rspObj.Tags = customer.Tags;
            rspObj.Remark = customer.Remark;
            rspObj.CreateTime = customer.CreateTime;
            rspObj.UpdateTime = customer.UpdateTime;

            rspObj.ContacterList = customer.ContacterList.Select(c => new MbsCrmPhsCtlGetCustomerRspContacterItemMdl
            {
                PhoneSalesContacterID = c.ID,
                ContacterName = c.ContacterName,
                ContacterJobTitle = c.ContacterJobTitle,
                ContacterDepartment = c.ContacterDepartment,
                ContacterPhone = c.ContacterPhone,
                ContacterMobile = c.ContacterMobile,
                ContacterEmail = c.ContacterEmail,
                IsPrimary = c.IsPrimary
            }).ToList();

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetCustomerAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }

    public async Task<MbsCrmPhsCtlAddCustomerRspMdl> AddCustomerAsync(MbsCrmPhsCtlAddCustomerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlAddCustomerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
            var loginInfo = _coEmpLoginInfoMemoryService.Get(coEmpLiMemGetReqObj);
            if (loginInfo == default)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
                return rspObj;
            }

            if (reqObj.ContacterList == null || !reqObj.ContacterList.Any())
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            var primaryCount = reqObj.ContacterList.Count(c => c.IsPrimary);
            if (primaryCount != 1)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            var customer = new PhoneSalesCustomerMemoryMdl
            {
                OwnerEmployeeID = loginInfo.EmployeeID,
                AtomPhoneSalesDataSource = reqObj.AtomPhoneSalesDataSource,
                ManagerActivityID = reqObj.ManagerActivityID,
                CompanyName = reqObj.CompanyName?.Trim(),
                UnifiedNumber = reqObj.UnifiedNumber?.Trim(),
                Industry = reqObj.Industry?.Trim(),
                CompanyScale = reqObj.CompanyScale?.Trim(),
                CompanyPhone = reqObj.CompanyPhone?.Trim(),
                CompanyAddress = reqObj.CompanyAddress?.Trim(),
                CompanyWebsite = reqObj.CompanyWebsite?.Trim(),
                AtomPhoneSalesCustomerStatus = DataModelLibrary.Database.AtomPhoneSalesCustomerStatus.DbAtomPhoneSalesCustomerStatusEnum.PendingContact,
                AtomPhoneSalesCustomerValue = DataModelLibrary.Database.AtomPhoneSalesCustomerValue.DbAtomPhoneSalesCustomerValueEnum.Pending,
                Tags = reqObj.Tags?.Trim(),
                Remark = reqObj.Remark?.Trim(),
                CreateEmployeeID = loginInfo.EmployeeID,
                UpdateEmployeeID = loginInfo.EmployeeID,
                CustomOrder = 0
            };

            customer.ContacterList = reqObj.ContacterList.Select(c => new PhoneSalesContacterMemoryMdl
            {
                ContacterName = c.ContacterName?.Trim(),
                ContacterJobTitle = c.ContacterJobTitle?.Trim(),
                ContacterDepartment = c.ContacterDepartment?.Trim(),
                ContacterPhone = c.ContacterPhone?.Trim(),
                ContacterMobile = c.ContacterMobile?.Trim(),
                ContacterEmail = c.ContacterEmail?.Trim(),
                IsPrimary = c.IsPrimary
            }).ToList();

            var customerId = _mbsPhsMemoryService.AddCustomer(customer);

            rspObj.PhoneSalesCustomerID = customerId;
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddCustomerAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }

    public async Task<MbsCrmPhsCtlUpdateCustomerRspMdl> UpdateCustomerAsync(MbsCrmPhsCtlUpdateCustomerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlUpdateCustomerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
            var loginInfo = _coEmpLoginInfoMemoryService.Get(coEmpLiMemGetReqObj);
            if (loginInfo == default)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
                return rspObj;
            }

            var existingCustomer = _mbsPhsMemoryService.GetCustomerById(reqObj.PhoneSalesCustomerID);
            if (existingCustomer == null)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            if (existingCustomer.OwnerEmployeeID != loginInfo.EmployeeID)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
                return rspObj;
            }

            if (reqObj.ContacterList == null || !reqObj.ContacterList.Any())
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            var primaryCount = reqObj.ContacterList.Count(c => c.IsPrimary);
            if (primaryCount != 1)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            existingCustomer.CompanyName = reqObj.CompanyName?.Trim();
            existingCustomer.UnifiedNumber = reqObj.UnifiedNumber?.Trim();
            existingCustomer.Industry = reqObj.Industry?.Trim();
            existingCustomer.CompanyScale = reqObj.CompanyScale?.Trim();
            existingCustomer.CompanyPhone = reqObj.CompanyPhone?.Trim();
            existingCustomer.CompanyAddress = reqObj.CompanyAddress?.Trim();
            existingCustomer.CompanyWebsite = reqObj.CompanyWebsite?.Trim();
            existingCustomer.AtomPhoneSalesCustomerStatus = reqObj.AtomPhoneSalesCustomerStatus;
            existingCustomer.Tags = reqObj.Tags?.Trim();
            existingCustomer.Remark = reqObj.Remark?.Trim();
            existingCustomer.UpdateEmployeeID = loginInfo.EmployeeID;

            existingCustomer.ContacterList = reqObj.ContacterList.Select(c => new PhoneSalesContacterMemoryMdl
            {
                ContacterName = c.ContacterName?.Trim(),
                ContacterJobTitle = c.ContacterJobTitle?.Trim(),
                ContacterDepartment = c.ContacterDepartment?.Trim(),
                ContacterPhone = c.ContacterPhone?.Trim(),
                ContacterMobile = c.ContacterMobile?.Trim(),
                ContacterEmail = c.ContacterEmail?.Trim(),
                IsPrimary = c.IsPrimary
            }).ToList();

            _mbsPhsMemoryService.UpdateCustomer(existingCustomer);

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateCustomerAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }

    public async Task<MbsCrmPhsCtlDeleteCustomerRspMdl> DeleteCustomerAsync(MbsCrmPhsCtlDeleteCustomerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlDeleteCustomerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
            var loginInfo = _coEmpLoginInfoMemoryService.Get(coEmpLiMemGetReqObj);
            if (loginInfo == default)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
                return rspObj;
            }

            var customer = _mbsPhsMemoryService.GetCustomerById(reqObj.PhoneSalesCustomerID);
            if (customer == null)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            if (customer.OwnerEmployeeID != loginInfo.EmployeeID)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
                return rspObj;
            }

            _mbsPhsMemoryService.DeleteCustomer(reqObj.PhoneSalesCustomerID);

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteCustomerAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }

    public async Task<MbsCrmPhsCtlUpdateCustomerOrderRspMdl> UpdateCustomerOrderAsync(MbsCrmPhsCtlUpdateCustomerOrderReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhsCtlUpdateCustomerOrderRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        try
        {
            var coEmpLiMemGetReqObj = new CoEmpLiMemGetReqMdl { EmployeeLoginToken = reqObj.EmployeeLoginToken };
            var loginInfo = _coEmpLoginInfoMemoryService.Get(coEmpLiMemGetReqObj);
            if (loginInfo == default)
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.LoginTokenError;
                return rspObj;
            }

            if (reqObj.CustomerOrderList == null || !reqObj.CustomerOrderList.Any())
            {
                rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
                return rspObj;
            }

            foreach (var item in reqObj.CustomerOrderList)
            {
                var customer = _mbsPhsMemoryService.GetCustomerById(item.PhoneSalesCustomerID);
                if (customer == null || customer.OwnerEmployeeID != loginInfo.EmployeeID)
                {
                    continue;
                }

                _mbsPhsMemoryService.UpdateCustomerOrder(item.PhoneSalesCustomerID, item.CustomOrder);
            }

            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateCustomerOrderAsync error");
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
        }

        return await Task.FromResult(rspObj);
    }
}
