using API.Customer.Data.Interfaces;
using API.Customer.Data.Options;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Data.SqlClient;

namespace API.Customer.Data.Factories
{
  public class SqlCommandFactory : IDBCommandFactory
  {
    private string _connectionString;

    public SqlCommandFactory(IOptions<CustomerOptions> databaseOptions)
    {
      _connectionString = databaseOptions.Value.ConnectionString;
    }

    public DbCommand CreateCommand()
    {
      var connection = new SqlConnection(_connectionString);
      connection.Open();

      return connection.CreateCommand();
    }
  }
}
