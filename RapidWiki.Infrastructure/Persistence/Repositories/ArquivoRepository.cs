using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class ArquivoRepository : IArquivoRepository
{
    private readonly RapidWikiDbContext _context;

    public ArquivoRepository(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task<Arquivo> CreateAsync(Arquivo entity)
    {
        var result = await _context.Arquivos.AddAsync(entity);

        return result.Entity;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Arquivo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Arquivo> GetByIdAsync(Guid id)
    {
        return await _context.Arquivos.FindAsync(id);
    }

    public Task UpdateAsync(Arquivo entity)
    {
        throw new NotImplementedException();
    }
}