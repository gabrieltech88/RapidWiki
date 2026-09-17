using RapidWiki.Application.GetRascunhos;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Interfaces;

public interface IProcedimentoRascunhoRepository
{
    Task<ProcedimentoRascunho?> GetByProcedimentoIdAsync(Guid procedimentoId, CancellationToken cancellationToken = default);
    Task<ProcedimentoRascunho?> GetByProcedimentoIdByUserAsync(Guid procedimentoId, Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default);
    Task CreateAsync(ProcedimentoRascunho rascunho, CancellationToken cancellationToken = default);
    Task<GetRascunhosResult> GetPagedAsync(Guid usuarioId, bool hasGlobalAccess, int page, string? search, CancellationToken cancellationToken = default);
    void Delete(ProcedimentoRascunho rascunho);
}
