using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Model;

namespace ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Service;

public interface IMbsPhsMemoryService
{
    List<PhoneSalesCustomerMemoryMdl> GetAllCustomers();
    PhoneSalesCustomerMemoryMdl GetCustomerById(int id);
    int AddCustomer(PhoneSalesCustomerMemoryMdl customer);
    void UpdateCustomer(PhoneSalesCustomerMemoryMdl customer);
    void DeleteCustomer(int id);
    void UpdateCustomerOrder(int id, int customOrder);
    List<PhoneSalesContacterMemoryMdl> GetContactersByCustomerId(int customerId);
    void ClearAllData();
}
