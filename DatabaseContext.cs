using System.Data;
using System.Data.SqlClient;

namespace InsurancePartnerManagement
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}