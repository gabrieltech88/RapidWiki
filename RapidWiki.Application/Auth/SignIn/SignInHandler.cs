using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.SignIn;

public class SignInHandler : IRequestHandler<SignInRequest, SignInResult>
{
    private readonly IAuthService _authService;

    public SignInHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<SignInResult> Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        if(request is null) throw new ArgumentNullException(nameof(request));
        
        var result = await _authService.SignInAsync(request);

        return result;
    }
}