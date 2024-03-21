using DataLayer.EfClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

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
            await SetDefaultRoleAsync(context);
            UpdateConnectionStringAsync(context, dbContext);
            await _next(context);
        }

        private async Task SetDefaultRoleAsync(HttpContext context)
        {
            var role = context.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                context.Session.SetString("Role", "guest");
                await context.Session.CommitAsync();
            }
        }

        private void UpdateConnectionStringAsync(HttpContext context, DataContext dbContext)
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
        }
    }
}
