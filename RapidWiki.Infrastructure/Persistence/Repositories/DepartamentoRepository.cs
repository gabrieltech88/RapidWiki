using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly RapidWikiDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DepartamentoRepository(RapidWikiDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;

    }

    public async Task<Departamento> CreateAsync(Departamento entity)
    {        
        var result = await _context.Departamentos.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return result.Entity;
    }


    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Departamento>> GetAllAsync()
    {
        return await _context.Departamentos.AsNoTracking().ToListAsync();
    }

    public async Task<Departamento> GetByIdAsync(Guid id)
    {
        return await _context.Departamentos.FindAsync(id);
    }

    public async Task<IEnumerable<Departamento>> GetByUserAsync(Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default)
    {
        var query = _context.Departamentos.AsNoTracking().AsQueryable();

        if (!hasGlobalAccess)
        {
            query = query.Where(
                departamento =>
                    departamento.Usuarios.Any(
                        usuario =>
                            usuario.Id == usuarioId
                    )
            );
        }

        return await query.OrderBy(departamento => departamento.Nome).ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(Departamento entity)
    {
        throw new NotImplementedException();
    }
}
