using DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SavourOdessa.Factory;
public interface IDataContextFactory
{
    DataContext CreateDbContext();
    DataContext CreateDbContext(string username, string password);
}

public class DataContextFactory : IDataContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public DataContextFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public DataContext CreateDbContext()
    {
        var username = _httpContextAccessor.HttpContext.Session.GetString("Username");
        var password = _httpContextAccessor.HttpContext.Session.GetString("Password");
        return CreateDbContext(username, password);
    }

    public DataContext CreateDbContext(string username, string password)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(connectionString))
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Username = username,
                Password = password
            };

            optionsBuilder.UseNpgsql(connectionStringBuilder.ConnectionString);
        }

        return new DataContext(optionsBuilder.Options);
    }
}
