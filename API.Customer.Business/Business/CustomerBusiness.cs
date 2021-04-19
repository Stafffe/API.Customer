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
    private readonly IHRSystemProvider _hrSystemProvider;

    public CustomerBusiness(IDatabaseProvider databaseProvider, ICustomerInformationValidatior customerInformationValidatior, IHRSystemProvider hrSystemProvider)
    {
      _databaseProvider = databaseProvider;
      _customerInformationValidatior = customerInformationValidatior;
      _hrSystemProvider = hrSystemProvider;
    }

    public async Task<CustomerInformation> GetCustomerInformation(string officialId)
    {
      officialId = FormatOfficialId(officialId);
      await _hrSystemProvider.ValidateOfficialId(officialId);
      var customerInformation = await _databaseProvider.GetCustomerInformation(officialId);

      return customerInformation;
    }

    public async Task<CustomerInformation> CreateCustomer(CustomerInformation customerInformation)
    {
      FormatData(ref customerInformation);
      await _hrSystemProvider.ValidateOfficialId(customerInformation.OfficialId);
      _customerInformationValidatior.ValidateCustomerInformation(customerInformation, false);

      await _databaseProvider.CreateCustomer(customerInformation);
      await _hrSystemProvider.NotifyAboutChange(customerInformation.OfficialId, ChangeType.Create);

      //There is at the moment no point in returning the actually created object here, but this could be usefull in other cases for example when an internal id of the object is created
      return await _databaseProvider.GetCustomerInformation(customerInformation.OfficialId);
    }

    public async Task<CustomerInformation> UpdateCustomer(CustomerInformation customerInformation)
    {
      FormatData(ref customerInformation);
      await _hrSystemProvider.ValidateOfficialId(customerInformation.OfficialId);
      _customerInformationValidatior.ValidateCustomerInformation(customerInformation, true);

      await _databaseProvider.UpdateCustomer(customerInformation);
      await _hrSystemProvider.NotifyAboutChange(customerInformation.OfficialId, ChangeType.Update);

      //Same here, no point of this return atm
      return await _databaseProvider.GetCustomerInformation(customerInformation.OfficialId);
    }

    public async Task DeleteCustomer(string officialId)
    {
      officialId = FormatOfficialId(officialId);
      await _hrSystemProvider.ValidateOfficialId(officialId);

      await _databaseProvider.DeleteCustomer(officialId);
      await _hrSystemProvider.NotifyAboutChange(officialId, ChangeType.Delete);
    }

    private void FormatData(ref CustomerInformation customerInfo) {     //This could me made nicer with an external class
      customerInfo.Adress.ZipCode = customerInfo.Adress?.ZipCode?.Replace(" ", "").Replace("-", "");
      customerInfo.PhoneNumber = customerInfo.PhoneNumber?.Replace(" ", "").Replace("-", "");
      customerInfo.OfficialId = FormatOfficialId(customerInfo.OfficialId);
    }

    private string FormatOfficialId(string officialId) {                        
      return officialId?.Replace("-", "");
    }
  }
}
