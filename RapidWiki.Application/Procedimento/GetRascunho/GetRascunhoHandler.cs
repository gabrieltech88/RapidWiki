using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetRascunhos;

public class GetRascunhosHandler : IRequestHandler<GetRascunhosRequest, GetRascunhosResult>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRascunhoRepository _procedimentoRascunhoRepository;

    public GetRascunhosHandler(ICurrentUser currentUser, IProcedimentoRascunhoRepository procedimentoRascunhoRepository)
    {
        _currentUser = currentUser;
        _procedimentoRascunhoRepository = procedimentoRascunhoRepository;
    }

    public async Task<GetRascunhosResult> Handle(GetRascunhosRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var isAdmin = _currentUser.IsInRole("Admin");
        var isEditor = _currentUser.IsInRole("Editor");

        if (!isAdmin && !isEditor)
        {
            throw new UnauthorizedAccessException("Você não possui permissão para visualizar rascunhos.");
        }

        return await _procedimentoRascunhoRepository.GetPagedAsync(usuarioId, isAdmin, request.Page, request.Search, cancellationToken);
    }
}