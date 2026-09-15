
namespace RapidWiki.Application.Interfaces;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task SaveChangesAsync();
    Task RollbackAsync();
}
