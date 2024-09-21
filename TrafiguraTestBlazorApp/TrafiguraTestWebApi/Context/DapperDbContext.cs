using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPI.Context;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = "Server=(localdb)\\StudentDB;Database=StudentDB;User Id=SSTest;Password=@Stracymishra1;Trusted_Connection=True;";
            //_configuration.GetConnectionString("SqlConnection");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}