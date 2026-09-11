using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.CreateProcedimento;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class ProcedimentoRepository : IRepository<Procedimento>
{
    private readonly RapidWikiDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public ProcedimentoRepository(RapidWikiDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;

    }

    public async Task<Procedimento> CreateAsync(Procedimento entity)
    {
        var result = await _context.Procedimentos.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return result.Entity;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Procedimento>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Procedimento> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Procedimento entity)
    {
        throw new NotImplementedException();
    }
}
