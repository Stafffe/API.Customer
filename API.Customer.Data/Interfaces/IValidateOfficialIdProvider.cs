using System.Threading.Tasks;

namespace API.Customer.Data.Interfaces
{
  public interface IValidateOfficialIdProvider
  {
    Task ValidateOfficialId(string officialId);
  }
}