using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Model;
using DataModelLibrary.Database.AtomPhoneSalesCustomerStatus;
using DataModelLibrary.Database.AtomPhoneSalesCustomerValue;
using DataModelLibrary.Database.AtomPhoneSalesDataSource;

namespace ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Service;

public class MbsPhsMemoryService : IMbsPhsMemoryService
{
    private readonly Dictionary<int, PhoneSalesCustomerMemoryMdl> _customers = new();
    private readonly Dictionary<int, PhoneSalesContacterMemoryMdl> _contacters = new();
    private int _customerIdCounter = 1;
    private int _contacterIdCounter = 1;
    private readonly object _lock = new object();

    public MbsPhsMemoryService()
    {
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        try
        {
            var csvPath = "/Users/r_r/Downloads/ProjectManagerSystem-prod/T5 iThome_20250415~0417_展攤資安展-1-Table 1.csv";

            if (!File.Exists(csvPath))
            {
                Console.WriteLine($"CSV file not found: {csvPath}");
                return;
            }

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);
            var companyDict = new Dictionary<string, PhoneSalesCustomerMemoryMdl>();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var fields = ParseCsvLine(lines[i]);

                if (fields.Count < 21)
                    continue;

                var companyName = fields[14]?.Trim();
                if (string.IsNullOrEmpty(companyName))
                    continue;

                var contacterName = fields[15]?.Trim();
                var industry = fields[6]?.Trim();
                var interestLevel = fields[9]?.Trim();
                var listStatus = fields[11]?.Trim();
                var note = fields[13]?.Trim();

                if (!companyDict.TryGetValue(companyName, out var customer))
                {
                    customer = new PhoneSalesCustomerMemoryMdl
                    {
                        ID = _customerIdCounter++,
                        OwnerEmployeeID = 1,
                        AtomPhoneSalesDataSource = DbAtomPhoneSalesDataSourceEnum.Activity,
                        CompanyName = companyName,
                        Industry = industry,
                        AtomPhoneSalesCustomerStatus = MapStatus(listStatus),
                        AtomPhoneSalesCustomerValue = MapValue(interestLevel),
                        IsExistingCustomer = note?.Contains("既有客戶") == true || note?.Contains("T5使用者") == true,
                        ContactCount = 0,
                        CustomOrder = _customerIdCounter,
                        Remark = note,
                        IsDeleted = false,
                        CreateEmployeeID = 1,
                        CreateTime = DateTimeOffset.Now.AddDays(-30),
                        UpdateTime = DateTimeOffset.Now.AddDays(-30)
                    };

                    companyDict[companyName] = customer;
                }

                if (!string.IsNullOrEmpty(contacterName))
                {
                    var contacter = new PhoneSalesContacterMemoryMdl
                    {
                        ID = _contacterIdCounter++,
                        PhoneSalesCustomerID = customer.ID,
                        ContacterName = contacterName,
                        ContacterJobTitle = fields[17]?.Trim(),
                        ContacterDepartment = fields[16]?.Trim(),
                        ContacterEmail = fields[18]?.Trim(),
                        ContacterPhone = fields[19]?.Trim(),
                        ContacterMobile = fields[20]?.Trim(),
                        IsPrimary = IsPrimaryContact(interestLevel),
                        CreateTime = DateTimeOffset.Now.AddDays(-30),
                        UpdateTime = DateTimeOffset.Now.AddDays(-30)
                    };

                    customer.ContacterList.Add(contacter);
                    _contacters[contacter.ID] = contacter;
                }
            }

            foreach (var customer in companyDict.Values)
            {
                _customers[customer.ID] = customer;
            }

            Console.WriteLine($"Loaded {_customers.Count} customers from CSV with {_contacters.Count} contacters");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CSV: {ex.Message}");
        }
    }

    private List<string> ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var inQuotes = false;
        var field = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            var ch = line[i];

            if (ch == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (ch == ',' && !inQuotes)
            {
                fields.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(ch);
            }
        }

        fields.Add(field.ToString());
        return fields;
    }

    private DbAtomPhoneSalesCustomerStatusEnum MapStatus(string listStatus)
    {
        if (string.IsNullOrEmpty(listStatus))
            return DbAtomPhoneSalesCustomerStatusEnum.PendingContact;

        if (listStatus.Contains("已轉業務"))
            return DbAtomPhoneSalesCustomerStatusEnum.Dispatched;
        if (listStatus.Contains("已完成電銷"))
            return DbAtomPhoneSalesCustomerStatusEnum.Contacted;
        if (listStatus.Contains("定期經營"))
            return DbAtomPhoneSalesCustomerStatusEnum.Contacted;
        if (listStatus.Contains("灰名單") || listStatus.Contains("黑名單"))
            return DbAtomPhoneSalesCustomerStatusEnum.NoContact;

        return DbAtomPhoneSalesCustomerStatusEnum.PendingContact;
    }

    private DbAtomPhoneSalesCustomerValueEnum MapValue(string interestLevel)
    {
        if (string.IsNullOrEmpty(interestLevel))
            return DbAtomPhoneSalesCustomerValueEnum.Pending;

        if (interestLevel.Contains("直接商機"))
            return DbAtomPhoneSalesCustomerValueEnum.High;
        if (interestLevel.Contains("潛在商機") || interestLevel.Contains("客戶深耕") || interestLevel.Contains("客戶服務"))
            return DbAtomPhoneSalesCustomerValueEnum.Medium;

        return DbAtomPhoneSalesCustomerValueEnum.Pending;
    }

    private bool IsPrimaryContact(string interestLevel)
    {
        if (string.IsNullOrEmpty(interestLevel))
            return true;

        if (interestLevel.Contains("次要窗口"))
            return false;

        return true;
    }

    public List<PhoneSalesCustomerMemoryMdl> GetAllCustomers()
    {
        lock (_lock)
        {
            return _customers.Values
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.CustomOrder)
                .ThenByDescending(c => c.UpdateTime)
                .ToList();
        }
    }

    public PhoneSalesCustomerMemoryMdl GetCustomerById(int id)
    {
        lock (_lock)
        {
            if (_customers.TryGetValue(id, out var customer) && !customer.IsDeleted)
            {
                return customer;
            }
            return null;
        }
    }

    public int AddCustomer(PhoneSalesCustomerMemoryMdl customer)
    {
        lock (_lock)
        {
            customer.ID = _customerIdCounter++;
            customer.CreateTime = DateTimeOffset.Now;
            customer.UpdateTime = DateTimeOffset.Now;
            customer.IsDeleted = false;
            customer.ContactCount = 0;
            customer.ValueScore = 0;

            if (customer.ContacterList != null && customer.ContacterList.Count > 0)
            {
                foreach (var contacter in customer.ContacterList)
                {
                    contacter.ID = _contacterIdCounter++;
                    contacter.PhoneSalesCustomerID = customer.ID;
                    contacter.CreateTime = DateTimeOffset.Now;
                    contacter.UpdateTime = DateTimeOffset.Now;
                    _contacters[contacter.ID] = contacter;
                }
            }

            _customers[customer.ID] = customer;
            return customer.ID;
        }
    }

    public void UpdateCustomer(PhoneSalesCustomerMemoryMdl customer)
    {
        lock (_lock)
        {
            if (_customers.TryGetValue(customer.ID, out var existingCustomer) && !existingCustomer.IsDeleted)
            {
                customer.UpdateTime = DateTimeOffset.Now;
                customer.CreateTime = existingCustomer.CreateTime;
                customer.CreateEmployeeID = existingCustomer.CreateEmployeeID;
                customer.OwnerEmployeeID = existingCustomer.OwnerEmployeeID;
                customer.ContactCount = existingCustomer.ContactCount;
                customer.LastContactTime = existingCustomer.LastContactTime;
                customer.LastContactEmployeeID = existingCustomer.LastContactEmployeeID;
                customer.IsDeleted = false;

                var existingContacterIds = existingCustomer.ContacterList.Select(c => c.ID).ToList();
                foreach (var contacterId in existingContacterIds)
                {
                    _contacters.Remove(contacterId);
                }

                if (customer.ContacterList != null && customer.ContacterList.Count > 0)
                {
                    foreach (var contacter in customer.ContacterList)
                    {
                        if (contacter.ID == 0)
                        {
                            contacter.ID = _contacterIdCounter++;
                        }
                        contacter.PhoneSalesCustomerID = customer.ID;
                        contacter.UpdateTime = DateTimeOffset.Now;
                        if (contacter.CreateTime == default)
                        {
                            contacter.CreateTime = DateTimeOffset.Now;
                        }
                        _contacters[contacter.ID] = contacter;
                    }
                }

                _customers[customer.ID] = customer;
            }
        }
    }

    public void DeleteCustomer(int id)
    {
        lock (_lock)
        {
            if (_customers.TryGetValue(id, out var customer))
            {
                customer.IsDeleted = true;
                customer.UpdateTime = DateTimeOffset.Now;
            }
        }
    }

    public void UpdateCustomerOrder(int id, int customOrder)
    {
        lock (_lock)
        {
            if (_customers.TryGetValue(id, out var customer) && !customer.IsDeleted)
            {
                customer.CustomOrder = customOrder;
                customer.UpdateTime = DateTimeOffset.Now;
            }
        }
    }

    public List<PhoneSalesContacterMemoryMdl> GetContactersByCustomerId(int customerId)
    {
        lock (_lock)
        {
            return _contacters.Values
                .Where(c => c.PhoneSalesCustomerID == customerId)
                .OrderByDescending(c => c.IsPrimary)
                .ThenBy(c => c.ID)
                .ToList();
        }
    }

    public void ClearAllData()
    {
        lock (_lock)
        {
            _customers.Clear();
            _contacters.Clear();
            _customerIdCounter = 1;
            _contacterIdCounter = 1;
        }
    }
}
