namespace RapidWiki.Application.Interfaces;

public interface IRoleService
{
    Task<Guid> CreateAsync(string roleName);
}