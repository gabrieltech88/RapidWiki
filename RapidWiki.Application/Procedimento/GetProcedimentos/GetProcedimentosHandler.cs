using MediatR;
using RapidWiki.Application.GetProcedimentosRequest;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
using RapidWiki.Domain.Enums;

public class GetProcedimentosHandler : IRequestHandler<GetProcedimentosRequest, GetProcedimentosResult>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRepository _procedimentoRepository;

    public GetProcedimentosHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository)
    {
        _currentUser = currentUser;
        _procedimentoRepository = procedimentoRepository;
    }

    public async Task<GetProcedimentosResult> Handle(GetProcedimentosRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.Id;
        var isAdmin = _currentUser.IsInRole("Admin");
        var isEditor = _currentUser.IsInRole("Editor");

        StatusProcedimento? status = request.Status;

        if (!isAdmin && !isEditor)
        {
            if (status == StatusProcedimento.Rascunho)
            {
                throw new UnauthorizedAccessException(
                    "Você não tem permissão para visualizar rascunhos."
                );
            }

            status = StatusProcedimento.Publicado;
        }

        return await _procedimentoRepository
            .GetPagedByUserAsync(
                userId,
                isAdmin,
                request.Page,
                request.Search,
                request.DepartamentoId,
                status,
                cancellationToken
            );
    }
}