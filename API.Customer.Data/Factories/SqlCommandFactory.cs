using API.Customer.Data.Interfaces;
using System.Data.Common;
using System.Data.SqlClient;

namespace API.Customer.Data.Factories
{
  public class SqlCommandFactory : IDBCommandFactory
  {
    private string _connectionString;

    public SqlCommandFactory(string connectionString)
    {
      _connectionString = connectionString;
    }

    public DbCommand CreateCommand()
    {
      var connection = new SqlConnection(_connectionString);
      connection.Open();

      return connection.CreateCommand();
    }
  }
}
