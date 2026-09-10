using MediatR;

namespace RapidWiki.Application.SignIn;

public record SignInRequest : IRequest<SignInResult>
{
    required public string Email { get; init; }
    required public string Password { get; init; }
}