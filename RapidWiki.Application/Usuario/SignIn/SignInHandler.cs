using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.SignIn;

public class SignInHandler : IRequestHandler<SignInRequest, SignInResult>
{
    private readonly IIdentityService _identityService;

    public SignInHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<SignInResult> Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        if(request is null) throw new ArgumentNullException(nameof(request));
        
        var result = await _identityService.SignInAsync(request);

        return result;
    }
}