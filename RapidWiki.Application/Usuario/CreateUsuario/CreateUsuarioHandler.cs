using MediatR;
using RapidWiki.Application.Interfaces;


namespace RapidWiki.Application.CreateUsuario;
public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioRequest, CreateUsuarioResult>
{
    private readonly IIdentityService _identityService;

    public CreateUsuarioHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<CreateUsuarioResult> Handle(CreateUsuarioRequest request, CancellationToken cancellationToken)
    {
        if(request is null) throw new ArgumentNullException(nameof(request));

        var userId = await _identityService.CreateUsuarioAsync(request);

        if(userId == Guid.Empty) throw new InvalidOperationException("Failed to create user.");
        
        return new CreateUsuarioResult { Id = userId };
    }
}
