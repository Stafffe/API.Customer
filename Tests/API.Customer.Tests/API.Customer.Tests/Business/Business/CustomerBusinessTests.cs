using API.Customer.Business.Business;
using API.Customer.Business.Exceptions;
using API.Customer.Business.Interfaces;
using API.Customer.Data.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace API.Customer.Tests.Business.Business
{
  public class CustomerBusinessTests
  {
    private readonly CustomerBusiness _sut;
    private readonly Mock<IDatabaseProvider> _databaseProviderMock;
    private readonly Mock<ICustomerInformationValidatior> _customerValidatorMock;
    private readonly Mock<IHRSystemProvider> _officialIdValidatorProvider;

    private const string OfficialId = "19910101-1234";

    public CustomerBusinessTests()
    {
      _databaseProviderMock = new Mock<IDatabaseProvider>();
      _customerValidatorMock = new Mock<ICustomerInformationValidatior>();
      _officialIdValidatorProvider = new Mock<IHRSystemProvider>();

      _sut = new CustomerBusiness(_databaseProviderMock.Object, _customerValidatorMock.Object, _officialIdValidatorProvider.Object);
    }

    [Fact]
    public async Task DeleteCustomer_WithValidOfficialId_ShouldDeleteCustomerWithFormattedOfficialId() {
      await _sut.DeleteCustomer(OfficialId);

      _databaseProviderMock.Verify(mock => mock.DeleteCustomer(OfficialId.Replace("-", "")), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_WithThrowingValidation_ShouldThrowAndNotDeleteCustomer()
    {
      _officialIdValidatorProvider
        .Setup(mock => mock.ValidateOfficialId(OfficialId.Replace("-", "")))
        .Throws(new ValidationException(""));

      await Assert.ThrowsAsync<ValidationException>(async () => await _sut.DeleteCustomer(OfficialId));
      _databaseProviderMock.Verify(mock => mock.DeleteCustomer(It.IsAny<string>()), Times.Never);
    }
  }
}
