using API.Customer.Data.DataObjects;
using System.Threading.Tasks;

namespace API.Customer.Business.Interfaces
{
  public interface ICustomerBusiness
  {
    Task<CustomerInformation> CreateCustomer(CustomerInformation customerInformation);
    Task DeleteCustomer(string officialId);
    Task<CustomerInformation> GetCustomerInformation(string officialId);
    Task<CustomerInformation> UpdateCustomer(CustomerInformation customerInformation);
  }
}