using API.Customer.Web.Interfaces;

namespace API.Customer.Web.Mapping
{
  public class CustomerMapper : ICustomerMapper
  {
    public DTOs.CustomerInformation Map(Data.DataObjects.CustomerInformation result)
    {
      throw new System.NotImplementedException();
    }

    public Data.DataObjects.CustomerInformation Map(DTOs.CustomerInformation updateCustomerInfo)
    {
      throw new System.NotImplementedException();
    }
  }
}
