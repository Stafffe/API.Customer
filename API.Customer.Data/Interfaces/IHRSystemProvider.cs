using API.Customer.Data.DataObjects;
using System.Threading.Tasks;

namespace API.Customer.Data.Interfaces
{
  public interface IHRSystemProvider
  {
    Task NotifyAboutChange(string officialId, ChangeType changeType);
    Task ValidateOfficialId(string officialId);
  }
}