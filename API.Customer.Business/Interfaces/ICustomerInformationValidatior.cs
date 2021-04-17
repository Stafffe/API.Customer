using API.Customer.Data.DataObjects;

namespace API.Customer.Business.Interfaces
{
  public interface ICustomerInformationValidatior
  {
    void ValidateOfficialId(string officialId);
    void ValidateCustomerInformation(CustomerInformation customerInfo);
  }
}