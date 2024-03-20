using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthorizeAttribute(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        // Получить роль пользователя
        var role = httpContext.Session.GetString("Role");
        // Проверка аутентификации пользователя
        if (string.IsNullOrEmpty(role))
        {
            role = "guest";
            httpContext.Session.SetString("Role", role);
            await httpContext.Session.CommitAsync();
        }
        if (role=="guest")
        {
            context.Result = new ChallengeResult();
            return;
        }

        // Проверить, имеет ли пользователь одну из нужных ролей
        if (!string.IsNullOrEmpty(role) && Roles!.Split(",").Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            // Пользователь имеет нужную роль, разрешить доступ к действию
            return;
        }

        // Пользователь не имеет доступа к данному действию
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        return;
    }
}
