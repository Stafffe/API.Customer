using API.Customer.Business.Exceptions;
using API.Customer.Business.Interfaces;
using API.Customer.Data.DataObjects;
using System;
using System.Linq;
using System.Net.Mail;

namespace API.Customer.Business.Validation
{
  public class CustomerInformationValidatior : ICustomerInformationValidatior
  {
    private readonly string[] ValidCountries = new string[] { "sweden", "denmark", "norway", "finland" };

    //I'm making an assumtion here that the customer needs to have all fields set. But could in some cases be otherwise
    public void ValidateCustomerInformation(CustomerInformation customerInfo, bool allowNullValues)
    {
      if (customerInfo == null)                                                       //Should add test for this
        throw new ValidationException("Recieved null CustomerInformation in validation class.");

      if (!allowNullValues)
        ValidateForNulls(customerInfo);

      ValidateEmail(customerInfo.Email);

      ValidateCountry(customerInfo.Adress?.Country);

      ValidateZipCode(customerInfo.Adress?.ZipCode);

      ValidatePhoneNumber(customerInfo.PhoneNumber, customerInfo.Adress?.Country);
    }

    private void ValidatePhoneNumber(string phoneNumber, string country)
    {
      if (string.IsNullOrWhiteSpace(phoneNumber))
        return;

      if (string.IsNullOrWhiteSpace(country))
        throw new ValidationException("Must provide a country in message to change phonenumber.");

      string phonNumberWithoutCountryNumber;
      if (country.Equals("sweden", StringComparison.InvariantCultureIgnoreCase))
      {
        if (phoneNumber.StartsWith("0"))
          phonNumberWithoutCountryNumber = phoneNumber.Remove(0, 1);
        else if (phoneNumber.StartsWith("+46"))
          phonNumberWithoutCountryNumber = phoneNumber.Remove(0, 3);
        else
          throw new ValidationException("Invalid phone number.");
      }
      else if (country.Equals("denmark", StringComparison.InvariantCultureIgnoreCase))
      {
        if (phoneNumber.StartsWith("+45"))
          phonNumberWithoutCountryNumber = phoneNumber.Remove(0, 3);
        else
          throw new ValidationException("Invalid phone number.");
      }
      else if (country.Equals("norway", StringComparison.InvariantCultureIgnoreCase))
      {
        if (phoneNumber.StartsWith("+47"))
          phonNumberWithoutCountryNumber = phoneNumber.Remove(0, 3);
        else
          throw new ValidationException("Invalid phone number.");
      }
      else if (country.Equals("finland", StringComparison.InvariantCultureIgnoreCase))
      {
        if (phoneNumber.StartsWith("+358"))
          phonNumberWithoutCountryNumber = phoneNumber.Remove(0, 4);
        else
          throw new ValidationException("Invalid phone number.");
      }
      else 
          throw new ValidationException("Invalid phone number.");

      if(phonNumberWithoutCountryNumber.Length > 15)
        throw new ValidationException("To long phone number.");
      if (phonNumberWithoutCountryNumber.Length < 8)
        throw new ValidationException("To short phone number.");
    }

    private void ValidateZipCode(string zipCode)
    {
      if (string.IsNullOrWhiteSpace(zipCode))
        return;

      if (!zipCode.All(char.IsDigit))
        throw new ValidationException("Invalid zipcode.");
      if (zipCode.Length < 4)
        throw new ValidationException("Invalid zipcode.");
      if (zipCode.Length > 5)
        throw new ValidationException("Invalid zipcode.");
    }

    private void ValidateCountry(string country)
    {
      if (string.IsNullOrWhiteSpace(country))
        return;
      if (!ValidCountries.Contains(country.ToLower()))
        throw new ValidationException("Invalid country.");
    }

    private void ValidateForNulls(CustomerInformation customerInfo)
    {
      if (string.IsNullOrWhiteSpace(customerInfo.Adress?.Country))
        throw new ValidationException("Recieved null country.");
      if (string.IsNullOrWhiteSpace(customerInfo.Adress?.ZipCode))
        throw new ValidationException("Recieved null zipcode.");
      if (string.IsNullOrWhiteSpace(customerInfo.Email))
        throw new ValidationException("Recieved null emial.");
      if (string.IsNullOrWhiteSpace(customerInfo.PhoneNumber))
        throw new ValidationException("Recieved null phonenumber.");
      if (string.IsNullOrWhiteSpace(customerInfo.OfficialId))
        throw new ValidationException("Recieved null officialId.");
    }

    private void ValidateEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return;

      try
      {
        new MailAddress(email);
      }
      catch
      {
        throw new ValidationException("Invalid email.");
      }
    }
  }
}
