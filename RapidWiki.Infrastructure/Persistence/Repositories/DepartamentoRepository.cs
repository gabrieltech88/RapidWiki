using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class DepartamentoRepository : IRepository<Departamento>
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
        return await _context.Departamentos.ToListAsync();
    }

    public async Task<Departamento> GetByIdAsync(Guid id)
    {
        return await _context.Departamentos.FindAsync(id);
    }

    public Task UpdateAsync(Departamento entity)
    {
        throw new NotImplementedException();
    }
}
