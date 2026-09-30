using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<(IReadOnlyCollection<Usuario> Items, int TotalItems)> GetPagedAsync(int page,string? search,int pageSize, CancellationToken cancellationToken = default);
}