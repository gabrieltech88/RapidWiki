using RapidWiki.Application.GetProcedimentosRequest;
using RapidWiki.Domain.Entities;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.Interfaces;

public interface IProcedimentoRepository : IRepository<Procedimento>
{
    Task<GetProcedimentosResult> GetPagedByUserAsync(Guid id, bool isAdmin, int page, string? search, Guid? departamentoId, StatusProcedimento status, CancellationToken cancellationToken = default);
    Task<Procedimento> GetByIdByUserAsync(Guid procedimentoId, Guid usuarioId, bool hasGlobalAccess);
}