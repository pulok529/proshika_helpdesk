using System.Data;
using Microsoft.Data.SqlClient;
using Helpdesk.Contracts.Auth;
using Helpdesk.Infrastructure.Legacy;
using Helpdesk.Infrastructure.Legacy.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helpdesk.Infrastructure.Services;

public class AuthService
{
    private readonly LegacyDataAccess _dataAccess;
    private readonly IConfiguration _configuration;

    public AuthService(LegacyDataAccess dataAccess, IConfiguration configuration)
    {
        _dataAccess = dataAccess;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginInternalAsync(string empId, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter> { new("@EmpId", empId) };
        var table = await _dataAccess.ExecuteDataTableAsync("sp_Login_User", parameters, cancellationToken);
        if (table.Rows.Count == 0) return null;

        var row = table.Rows[0];
        var roleId = row.Field<int>("RoleId");
        var name = row.Field<string>("Employee_Name") ?? empId;
        return IssueToken(empId, "internal", roleId.ToString(), name);
    }

    public async Task<LoginResponse?> LoginVendorAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@UserName", username),
            new("@Password", password)
        };
        var table = await _dataAccess.ExecuteDataTableAsync("sp_External_Login_User", parameters, cancellationToken);
        if (table.Rows.Count == 0) return null;

        var row = table.Rows[0];
        var roleId = row.Field<int>("RoleId");
        var empId = row.Field<int>("EmpId").ToString();
        var name = $"{row.Field<string>("SPFName")} {row.Field<string>("SPLName")}".Trim();
        return IssueToken(empId, "vendor", roleId.ToString(), name);
    }

    private LoginResponse IssueToken(string subject, string role, string roleId, string displayName)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SigningKey"]!));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(ClaimTypes.Role, role),
            new("roleId", roleId),
            new("name", displayName)
        };

        var expires = DateTime.UtcNow.AddHours(8);
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials);

        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResponse(encoded, expires);
    }
}
