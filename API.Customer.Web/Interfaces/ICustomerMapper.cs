namespace API.Customer.Web.Interfaces
{
  public interface ICustomerMapper
  {
    DTOs.CustomerInformation Map(Data.DataObjects.CustomerInformation result);
    Data.DataObjects.CustomerInformation Map(DTOs.CustomerInformation updateCustomerInfo);
  }
}
