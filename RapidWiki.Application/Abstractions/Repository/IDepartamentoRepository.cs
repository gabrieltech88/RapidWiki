using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Interfaces;

public interface IDepartamentoRepository : IRepository<Departamento>
{
    Task<IEnumerable<Departamento>> GetByUserAsync(Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default);
}