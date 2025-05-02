using Dapper;
using InsurancePartnerManagement.Models;
using System.Data.SqlClient;

namespace InsurancePartnerManagement.Repositories;

public class PartnerRepository
{
    private readonly string _connectionString;

    public PartnerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Partner>("SELECT * FROM Partners ORDER BY CreatedAtUtc DESC");
    }

    public async Task<int> AddPartnerAsync(Partner partner)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"
            INSERT INTO Partners (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId, 
                                  CreatedAtUtc, CreateByUser, IsForeign, ExternalCode, Gender)
            VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId, 
                    @CreatedAtUtc, @CreateByUser, @IsForeign, @ExternalCode, @Gender);
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, partner);
    }

    public async Task<IEnumerable<Partner>> GetAllPartnersWithPoliciesAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"
            SELECT p.*, 
                   COUNT(po.Id) AS TotalPolicies, 
                   ISNULL(SUM(po.PolicyAmount), 0) AS TotalPolicyAmount
            FROM Partners p
            LEFT JOIN Policies po ON p.Id = po.PartnerId
            GROUP BY p.Id, p.FirstName, p.LastName, p.Address, p.PartnerNumber, p.CroatianPIN, 
                     p.PartnerTypeId, p.CreatedAtUtc, p.CreateByUser, p.IsForeign, p.ExternalCode, p.Gender
            ORDER BY p.CreatedAtUtc DESC";
        return await connection.QueryAsync<Partner>(sql);
    }

    public async Task<Partner> GetPartnerByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Partners WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Partner>(sql, new { Id = id });
    }
}