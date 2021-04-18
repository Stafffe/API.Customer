using API.Customer.Data.DataObjects;
using API.Customer.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Customer.Data.Providers
{
  public class DatabaseProvider : IDatabaseProvider
  {
    private readonly IDBCommandFactory _dBConnectionFactory;

    public DatabaseProvider(IDBCommandFactory dBConnectionFactory)
    {
      _dBConnectionFactory = dBConnectionFactory;
    }

    public async Task<CustomerInformation> GetCustomerInformation(string officialId)
    {
      string email;
      string zipCode;
      string country;
      string phoneNumber;

      using (var command = _dBConnectionFactory.CreateCommand())
      {
        command.Parameters.Add(new SqlParameter("@OfficialId", officialId));

        command.CommandText = $@"
        SELECT Email, ZipCode, Country, PhoneNumber
        FROM Customer 
        WHERE OfficialId = @OfficialId
";

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (reader.Read())
          {
            email = reader.GetString(0);
            zipCode = reader.GetString(1);
            country = reader.GetString(2);
            phoneNumber = reader.GetString(3);
          }
          else
            throw new Exception($"Could not find customer with id {officialId} in database.");  //May or may not log official id here depending on company policy
        }
      }

      return new CustomerInformation
      {
        OfficialId = officialId,
        Email = email,
        Adress = new Address { Country = country, ZipCode = zipCode },
        PhoneNumber = phoneNumber
      };
    }

    public Task CreateCustomer(CustomerInformation customerInformation)
    {
      using (var command = _dBConnectionFactory.CreateCommand())
      {
        var parameters = CreateParameters(customerInformation).ToArray();
        command.Parameters.AddRange(parameters);

        command.CommandText = $@"
          INSERT INTO Customer (OfficialId, Email, PhoneNumber, Country, ZipCode)
          VALUES(@OfficialId, @Email, @PhoneNumber, @Country, @ZipCode)
";

        return command.ExecuteNonQueryAsync();
      };
    }

    public Task UpdateCustomer(CustomerInformation customerInformation)
    {
      using (var command = _dBConnectionFactory.CreateCommand())
      {
        var parameters = CreateParameters(customerInformation).ToArray();
        command.Parameters.AddRange(parameters);
        string setText = GetSetTextForUpdate(customerInformation);

        command.CommandText = $@"
          UPDATE CUSTOMER
          {setText}
          WHERE OfficialId = @OfficialId
";

        return command.ExecuteNonQueryAsync();
      };
    }

    public Task DeleteCustomer(string officialId)
    {
      using (var command = _dBConnectionFactory.CreateCommand())
      {
        command.Parameters.Add(new SqlParameter("@OfficialId", officialId));

        command.CommandText = $@"
        DELETE FROM Customer 
        WHERE OfficialId = @OfficialId
";

        return command.ExecuteNonQueryAsync();
      }
    }

    private IEnumerable<SqlParameter> CreateParameters(CustomerInformation customerInformation)
    {
      if (!string.IsNullOrWhiteSpace(customerInformation.OfficialId))
        yield return new SqlParameter("@OfficialId", customerInformation.OfficialId);
      if (!string.IsNullOrWhiteSpace(customerInformation.Email))
          yield return new SqlParameter("@Email", customerInformation.Email);
      if (!string.IsNullOrWhiteSpace(customerInformation.PhoneNumber))
        yield return new SqlParameter("@PhoneNumber", customerInformation.PhoneNumber);
      if (!string.IsNullOrWhiteSpace(customerInformation.Adress?.Country))
        yield return new SqlParameter("@Country", customerInformation.Adress.Country);
      if (!string.IsNullOrWhiteSpace(customerInformation.Adress?.ZipCode))
        yield return new SqlParameter("@ZipCode", customerInformation.Adress.ZipCode);
    }

    private static string GetSetTextForUpdate(CustomerInformation customerInformation)
    {
      var setText = new StringBuilder("SET ");
      if (customerInformation.Email != null)
      {
        setText.Append("Email = @Email, ");
      }
      if (customerInformation.PhoneNumber != null)
      {
        setText.Append("PhoneNumber = @PhoneNumber, ");
      }
      if (customerInformation.Adress.Country != null)
      {
        setText.Append("Country = @Country, ");
      }
      if (customerInformation.Adress.ZipCode != null)
      {
        setText.Append("ZipCode = @ZipCode, ");
      }
      setText.Remove(setText.Length - 2, 2);
      return setText.ToString();
    }
  }
}
