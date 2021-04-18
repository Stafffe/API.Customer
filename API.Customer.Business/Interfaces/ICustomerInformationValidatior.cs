using API.Customer.Data.DataObjects;

namespace API.Customer.Business.Interfaces
{
  public interface ICustomerInformationValidatior
  {
    void ValidateCustomerInformation(CustomerInformation customerInfo, bool allowNullValues);
  }
}