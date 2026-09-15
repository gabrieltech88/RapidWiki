namespace RapidWiki.Application.SignIn;

public record SignInResult
{
    required public Guid Id { get; init; }
    required public string Role { get; init; }
}