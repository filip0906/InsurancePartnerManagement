using Dapper;
using InsurancePartnerManagement.Models;
using System.Data.SqlClient;

namespace InsurancePartnerManagement.Repositories;

public class PolicyRepository
{
    private readonly string _connectionString;

    public PolicyRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // Dohvati sve police za određenog partnera
    public async Task<IEnumerable<Policy>> GetPoliciesByPartnerIdAsync(int partnerId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Policies WHERE PartnerId = @PartnerId";
        return await connection.QueryAsync<Policy>(sql, new { PartnerId = partnerId });
    }

    public async Task AddPolicyAsync(Policy policy)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"INSERT INTO Policies (PolicyNumber, PolicyAmount, PartnerId)
                    VALUES (@PolicyNumber, @PolicyAmount, @PartnerId)";
        await connection.ExecuteAsync(sql, policy);
    }
}