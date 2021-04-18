using API.Customer.Business.Exceptions;
using API.Customer.Business.Interfaces;
using API.Customer.Data.DataObjects;
using System;
using System.Linq;

namespace API.Customer.Business.Validation
{
  public class CustomerInformationValidatior : ICustomerInformationValidatior
  {
    //I'm making an assumtion here that the customer needs to have all fields set. But could in some cases be otherwise
    public void ValidateCustomerInformation(CustomerInformation customerInfo, bool allowNullValues)
    {
      throw new NotImplementedException();
    }
  }
}
