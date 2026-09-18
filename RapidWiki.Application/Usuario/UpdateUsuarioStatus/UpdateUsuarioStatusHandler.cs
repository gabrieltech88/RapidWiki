using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.UpdateUsuarioStatus;

public class UpdateUsuarioStatusHandler : IRequestHandler<UpdateUsuarioStatusRequest>
{
    private readonly IIdentityService _identityService;

    public UpdateUsuarioStatusHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(UpdateUsuarioStatusRequest request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("O Id do usuário é obrigatório.", nameof(request.Id));

        await _identityService.UpdateUsuarioStatusAsync(request.Id, request.Ativo);
    }
}