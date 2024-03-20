using DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SavourOdessa.Middleware
{
    public class ConnectionStringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ConnectionStringMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, DataContext dbContext)
        {
            var username = context.Session.GetString("Username");
            var password = context.Session.GetString("Password");
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(connectionString))
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
                connectionStringBuilder.Username = username;
                connectionStringBuilder.Password = password;

                dbContext.Database.GetDbConnection().ConnectionString = connectionStringBuilder.ConnectionString;
            }

            await _next(context);
        }
    }
}
