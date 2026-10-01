using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetArquivos;

public class GetArquivosHandler : IRequestHandler<GetArquivosRequest, GetArquivosResult>
{
    private readonly ICurrentUser _currentUser;
    private readonly IArquivoRepository _arquivoRepository;

    public GetArquivosHandler(ICurrentUser currentUser, IArquivoRepository arquivoRepository)
    {
        _currentUser = currentUser;
        _arquivoRepository = arquivoRepository;
    }

    public async Task<GetArquivosResult> Handle(GetArquivosRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;

        var isAdmin = _currentUser.IsInRole("Admin");

        return await _arquivoRepository.GetPagedByUserAsync(
                usuarioId,
                isAdmin,
                request.Page,
                request.Search,
                request.DepartamentoId,
                cancellationToken
            );
    }
}