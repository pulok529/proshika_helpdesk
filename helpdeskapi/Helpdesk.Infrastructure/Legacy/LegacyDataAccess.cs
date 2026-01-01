using System.Data;
using System.Data.Common;
using Helpdesk.Infrastructure.Legacy.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Helpdesk.Infrastructure.Legacy;

/// <summary>
/// Thin helper to execute legacy stored procedures via the scaffolded LegacyDbContext.
/// </summary>
public class LegacyDataAccess
{
    private readonly LegacyDbContext _dbContext;

    public LegacyDataAccess(LegacyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataTable> ExecuteDataTableAsync(string storedProc, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection, cancellationToken);

        await using var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = storedProc;
        foreach (var p in parameters)
        {
            cmd.Parameters.Add(p);
        }

        var dt = new DataTable();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        dt.Load(reader);
        return dt;
    }

    public async Task<int> ExecuteScalarIntAsync(string storedProc, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        await EnsureOpenAsync(connection, cancellationToken);

        await using var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = storedProc;
        foreach (var p in parameters)
        {
            cmd.Parameters.Add(p);
        }

        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    private static async Task EnsureOpenAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }
    }
}
