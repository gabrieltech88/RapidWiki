using System.Security.Claims;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Api.Authentication;
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor =  httpContextAccessor;
    }

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException(
            "Usuário não autenticado."
        );

    public Guid Id
    {
        get
        {
            var value =_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(value, out var userId))
            {
                throw new UnauthorizedAccessException();
            }

            return userId;
        }
    }

    public bool IsInRole(string role)
    {
        return User.IsInRole(role);
    }
}