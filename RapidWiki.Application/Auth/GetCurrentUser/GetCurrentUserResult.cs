namespace RapidWiki.Application.GetCurrentUser;

public record GetCurrentUserResult
{
    required public Guid Id { get; init; }
    required public string Nome { get; init; }
    required public string Role { get; init; }
}