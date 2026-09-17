using RapidWiki.Application.Interfaces;

namespace RapidWiki.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly RapidWikiDbContext _context;

    public UnitOfWork(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
