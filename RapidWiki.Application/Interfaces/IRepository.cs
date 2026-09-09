namespace RapidWiki.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}