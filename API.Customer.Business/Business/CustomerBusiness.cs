using API.Customer.Business.Interfaces;
using API.Customer.Data.DataObjects;
using API.Customer.Data.Interfaces;
using System.Threading.Tasks;

namespace API.Customer.Business.Business
{
  public class CustomerBusiness : ICustomerBusiness
  {
    private readonly IDatabaseProvider _databaseProvider;
    private readonly ICustomerInformationValidatior _customerInformationValidatior;

    public CustomerBusiness(IDatabaseProvider databaseProvider, ICustomerInformationValidatior customerInformationValidatior)
    {
      _databaseProvider = databaseProvider;
      _customerInformationValidatior = customerInformationValidatior;
    }

    public async Task<CustomerInformation> GetCustomerInformation(string officialId)
    {
      var customerInformation = await _databaseProvider.GetCustomerInformation(officialId);

      return customerInformation;
    }

    public async Task<CustomerInformation> CreateCustomer(CustomerInformation customerInformation)
    {
      _customerInformationValidatior.ValidateCustomerInformation(customerInformation);

      await _databaseProvider.CreateCustomer(customerInformation);

      //There is at the moment no point in returning the actually created object here, but this could be usefull in other cases for example when an internal id of the object is created
      return await _databaseProvider.GetCustomerInformation(customerInformation.OfficialId);
    }

    public async Task<CustomerInformation> UpdateCustomer(CustomerInformation customerInformation)
    {
      _customerInformationValidatior.ValidateCustomerInformation(customerInformation);

      await _databaseProvider.UpdateCustomer(customerInformation);

      //Same here, no point of this return atm
      return await _databaseProvider.GetCustomerInformation(customerInformation.OfficialId);
    }

    public async Task DeleteCustomer(string officialId) {
      _customerInformationValidatior.ValidateOfficialId(officialId);
      await _databaseProvider.DeleteCustomer(officialId);
    }
  }
}
