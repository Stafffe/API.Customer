using API.Customer.Web.Interfaces;
using DataObjects = API.Customer.Data.DataObjects;

namespace API.Customer.Web.Mapping
{
  public class CustomerMapper : ICustomerMapper
  {
    public DTOs.CustomerInformation Map(DataObjects.CustomerInformation customerInfo)
    {
      return new DTOs.CustomerInformation
      {
        Adress = new DTOs.Address { Country = customerInfo.Adress.Country, ZipCode = customerInfo.Adress.ZipCode },
        Email = customerInfo.Email,
        OfficialId = customerInfo.OfficialId,
        PhoneNumber = customerInfo.PhoneNumber
      };
    }

    public DataObjects.CustomerInformation Map(DTOs.CustomerInformation customerInfo)
    {
      return new DataObjects.CustomerInformation
      {
        Adress = new DataObjects.Address { Country = customerInfo.Adress.Country, ZipCode = customerInfo.Adress.ZipCode },
        Email = customerInfo.Email,
        OfficialId = customerInfo.OfficialId,
        PhoneNumber = customerInfo.PhoneNumber
      };
    }
  }
}
