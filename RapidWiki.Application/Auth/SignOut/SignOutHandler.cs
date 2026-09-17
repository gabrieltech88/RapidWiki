using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.SignOut;

public class SignOutHandler
    : IRequestHandler<SignOutRequest>
{
    private readonly IAuthService _authService;


    public SignOutHandler(
        IAuthService authService)
    {
        _authService = authService;
    }


    public async Task Handle(
        SignOutRequest request,
        CancellationToken cancellationToken)
    {
        await _authService.SignOutAsync();
    }
}