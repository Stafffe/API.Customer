using API.Customer.Data.DataObjects;
using API.Customer.Data.Interfaces;
using System.Threading.Tasks;

namespace API.Customer.Data.Providers
{
  public class DatabaseProvider : IDatabaseProvider
  {
    public DatabaseProvider()
    {

    }

    public async Task<CustomerInformation> GetCustomerInformation(string officialId) {
      return null;
    }

    public async Task CreateCustomer(CustomerInformation customerInformation) {

    }

    public async Task UpdateCustomer(CustomerInformation customerInformation)
    {

    }

    public async Task DeleteCustomer(string officialId) { 
    
    }
  }
}
