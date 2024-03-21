using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute(string roles)
        : base(typeof(CustomAuthorizeFilter))
    {
        Arguments = [roles];
    }
}

public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _roles;

    public CustomAuthorizeFilter(IHttpContextAccessor httpContextAccessor, string roles)
    {
        _httpContextAccessor = httpContextAccessor;
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        // Получить роль пользователя
        var role = httpContext.Session.GetString("Role")!;
        

        // Проверить, имеет ли пользователь одну из нужных ролей
        if (_roles!.Split(",").Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            // Пользователь имеет нужную роль, разрешить доступ к действию
            return;
        }
        if (role == "guest")
        {
            context.Result = new ChallengeResult();
            return;
        }
        // Пользователь не имеет доступа к данному действию
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        return;
    }
}
