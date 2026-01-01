using System.Data;
using Helpdesk.Contracts.Support;
using Helpdesk.Infrastructure.Abstractions;
using Helpdesk.Infrastructure.Legacy;
using Microsoft.Data.SqlClient;

namespace Helpdesk.Infrastructure.Services;

public class SupportUserService : ISupportUserService
{
    private readonly LegacyDataAccess _dataAccess;

    public SupportUserService(LegacyDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<IReadOnlyList<SupportUserCandidateDto>> GetUnassignedAsync(CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_UnAssignedList", Array.Empty<SqlParameter>(), cancellationToken);
        var list = new List<SupportUserCandidateDto>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add(new SupportUserCandidateDto(
                row.Field<int>("EmpId"),
                row.Field<string?>("Name_Bengoli"),
                row.Field<string?>("Desig_Name")));
        }

        return list;
    }

    public async Task<IReadOnlyList<SupportUserDto>> GetAssignedAsync(CancellationToken cancellationToken = default)
    {
        var dt = await _dataAccess.ExecuteDataTableAsync("sp_Get_AssignedList", Array.Empty<SqlParameter>(), cancellationToken);
        var list = new List<SupportUserDto>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add(new SupportUserDto(
                row.Field<int>("PermittedId"),
                row.Field<int>("EmpId"),
                row.Field<string?>("Name_Bengoli"),
                row.Field<string?>("Desig_Name"),
                row.Field<string?>("RoleName"),
                row.Field<string?>("EntryUserName"),
                row.Field<string?>("Status"),
                row.Field<DateTime?>("EntryDate")));
        }

        return list;
    }

    public async Task AssignAsync(int empId, int entryBy, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@EmpId", empId),
            new("@EntryDate", DateTime.Now),
            new("@EntryBy", entryBy)
        };
        await _dataAccess.ExecuteScalarIntAsync("sp_Save_Assign_SupportUser_INTERNAL", parameters, cancellationToken);
    }

    public async Task SetStatusAsync(int permittedId, int entryBy, bool active, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@PermittedId", permittedId),
            new("@EntryDate", DateTime.Now),
            new("@EntryBy", entryBy)
        };
        var storedProc = active ? "sp_Active_IT_User" : "sp_DeActive_IT_User";
        await _dataAccess.ExecuteScalarIntAsync(storedProc, parameters, cancellationToken);
    }
}
