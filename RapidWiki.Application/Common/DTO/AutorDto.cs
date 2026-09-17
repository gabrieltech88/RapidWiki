namespace RapidWiki.Application.Common.Dto;

public record AutorDto
{
    required public Guid Id { get; init; }
    required public string Nome { get; init; }
}