using System;

namespace API.Customer.Business.Exceptions
{
  public class ValidationException : Exception
  {
    public ValidationException(string errorMessage) : base(errorMessage)
    {

    }
  }
}
