using Npgsql;
using Staffinity.Personal.Domain.Modules.Auth.Model;
using Staffinity.Personal.Domain.Modules.Auth.Ports.Out;

namespace Staffinity.Personal.Infrastructure.Persistence.Auth;

public sealed class PgAuthRepository : IAuthRepository
{
    private const string Query = """
        SELECT e.employee_id, e.email, e.password_hash, e.access_level_id, al.name AS access_level_name
        FROM public.employees e
        JOIN public.access_levels al ON al.access_level_id = e.access_level_id
        WHERE e.email = @email AND COALESCE(e.is_deleted,false)=false
        LIMIT 1;
        """;

    private readonly NpgsqlDataSource _dataSource;

    public PgAuthRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }

    public async Task<(AuthUser? User, string? PasswordHash)> FindByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        await using var command = _dataSource.CreateCommand(Query);
        command.Parameters.AddWithValue("email", email);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return (null, null);
        }

        var user = new AuthUser
        {
            Id = reader.GetGuid(reader.GetOrdinal("employee_id")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            AccessLevelId = reader.GetGuid(reader.GetOrdinal("access_level_id")),
            AccessLevelName = reader.GetString(reader.GetOrdinal("access_level_name"))
        };

        var passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));
        return (user, passwordHash);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
