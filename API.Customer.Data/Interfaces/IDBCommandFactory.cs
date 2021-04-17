using System.Data.Common;

namespace API.Customer.Data.Interfaces
{
  public interface IDBCommandFactory
  {
    DbCommand CreateCommand();
  }
}
