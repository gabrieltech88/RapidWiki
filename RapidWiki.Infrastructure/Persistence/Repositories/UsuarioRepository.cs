using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly RapidWikiDbContext _context;

    public UsuarioRepository(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> CreateAsync(Usuario entity)
    {
        await _context.Usuarios.AddAsync(entity);

        return entity;
    }
    public async Task<(IReadOnlyCollection<Usuario> Items, int TotalItems)> GetPagedAsync(int page, string? search, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 1)
        {
            page = 1;
        }

        var query = _context.Usuarios.AsNoTracking().Include(usuario =>
                usuario.Departamentos
            )
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(usuario =>usuario.Nome.Contains(term));
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var usuarios =await query.OrderBy(usuario =>usuario.Nome)
                .Skip(
                    (page - 1) *
                    pageSize
                )
                .Take(
                    pageSize
                )
                .ToListAsync(
                    cancellationToken
                );

        return (
            usuarios,
            totalItems
        );
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Usuario>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await _context.Usuarios.Include(usuario => usuario.Departamentos)
            .FirstOrDefaultAsync(
                usuario => usuario.Id == id
            );
    }

    public Task UpdateAsync(Usuario entity)
    {
        throw new NotImplementedException();
    }
}
