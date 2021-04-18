using API.Customer.Business.Exceptions;
using API.Customer.Business.Validation;
using API.Customer.Data.DataObjects;
using Xunit;

namespace API.Customer.Tests.Business.Validation
{
  public class CustomerInformationValidatiorTests
  {
    private readonly CustomerInformationValidatior _sut;

    public CustomerInformationValidatiorTests()
    {
      _sut = new CustomerInformationValidatior();
    }

    [Fact]
    public void ValidateCustomerInformation_WithNullsAndNotAllowingNulls_ShouldThrow()
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Email = null;

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Fact]
    public void ValidateCustomerInformation_WithNullsAndShouldAllowgNulls_ShouldNotThrow()
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Email = null;

      _sut.ValidateCustomerInformation(customerInfo, true);
    }

    [Fact]
    public void ValidateCustomerInformation_WithMissingAtCharInEmail_ShouldThrow()
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Email = "dummy.se";

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Fact]
    public void ValidateCustomerInformation_WithMissingDotInEmail_ShouldThrow()
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Email = "dummy@se";

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Fact]
    public void ValidateCustomerInformation_WithNotLettersInEmail_ShouldThrow()
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Email = "@.";

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Theory]
    [InlineData("Sweden")]
    [InlineData("Denmark")]
    [InlineData("Norway")]
    [InlineData("Finland")]
    public void ValidateCustomerInformation_WithCorrectCountry_ShouldNotThrow(string country)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Adress.Country = country;

      _sut.ValidateCustomerInformation(customerInfo, false);
    }

    [Theory]
    [InlineData("Germany")]
    [InlineData("US")]
    [InlineData("Spain")]
    public void ValidateCustomerInformation_WithIncorrectCountry_ShouldThrow(string country)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Adress.Country = country;

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Theory]
    [InlineData("17A70")]
    [InlineData("1707000")]
    [InlineData("170700")]
    [InlineData("68 080")]
    [InlineData("170")]
    [InlineData("17")]
    [InlineData("7")]
    public void ValidateCustomerInformation_WithInvalidZipCode_ShouldThrow(string zipCode)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Adress.ZipCode = zipCode;

      Assert.Throws<ValidationException>(() => _sut.ValidateCustomerInformation(customerInfo, false));
    }

    [Theory]
    [InlineData("68080")]
    [InlineData("6800")]
    public void ValidateCustomerInformation_WithValidZipCode_ShouldNotThrow(string zipCode)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.Adress.ZipCode = zipCode;

      _sut.ValidateCustomerInformation(customerInfo, false);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("22345678")]
    [InlineData("32345678")]
    [InlineData("+44234567891")]
    [InlineData("+461234567")]
    [InlineData("+461234567891234567")]
    public void ValidateCustomerInformation_WithInvalidPhoneNumber_ShouldThrow(string phoneNumber)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.PhoneNumber = phoneNumber;

      _sut.ValidateCustomerInformation(customerInfo, false);
    }

    [Theory]
    [InlineData("+4612345678")]
    [InlineData("+4512345678")]
    [InlineData("+4712345678")]
    [InlineData("012345678")]
    [InlineData("+46123456789123456")]
    [InlineData("+358123456789123456")]
    public void ValidateCustomerInformation_WithValidPhoneNumber_ShouldNotThrow(string phoneNumber)
    {
      var customerInfo = GetValidCustomerInformation();
      customerInfo.PhoneNumber = phoneNumber;

      _sut.ValidateCustomerInformation(customerInfo, false);
    }

    private CustomerInformation GetValidCustomerInformation()
    {
      return new CustomerInformation
      {
        Adress = new Address { Country = "Sweden", ZipCode = "17070" },
        Email = "dummy@dummy.se",
        OfficialId = "920202-0393",
        PhoneNumber = "+46711921411"
      };
    }
  }
}
