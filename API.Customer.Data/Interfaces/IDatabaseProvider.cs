using API.Customer.Data.DataObjects;
using System.Threading.Tasks;

namespace API.Customer.Data.Interfaces
{
  public interface IDatabaseProvider
  {
    Task<CustomerInformation> GetCustomerInformation(string officialId);
    Task CreateCustomer(CustomerInformation customerInformation);
    Task UpdateCustomer(CustomerInformation customerInformation);
    Task DeleteCustomer(string officialId);
  }
}