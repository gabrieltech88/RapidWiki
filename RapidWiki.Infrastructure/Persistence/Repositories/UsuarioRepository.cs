using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IRepository<Usuario>
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

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Usuario>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Usuario> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Usuario entity)
    {
        throw new NotImplementedException();
    }
}
