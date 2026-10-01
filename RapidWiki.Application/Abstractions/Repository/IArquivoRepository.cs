using RapidWiki.Application.GetArquivos;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Interfaces;

public interface IArquivoRepository : IRepository<Arquivo>
{
    Task<GetArquivosResult> GetPagedByUserAsync(Guid usuarioId, bool hasGlobalAccess, int page, string? search, Guid? departamentoId, CancellationToken cancellationToken = default);
    Task<Arquivo?> GetAccessibleByIdAsync(Guid arquivoId, Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default);
}