namespace RapidWiki.Application.Commmon.Dto;

public record RascunhoDto
{
    public Guid Id { get; init; }
    public required string Titulo { get; init; }
    public required string Descricao { get; init; }
    public Guid AutorId { get; init; }
    public required string AutorNome { get; init; }
    public Guid EditorId { get; init; }
    public required string EditorNome { get; init; }
    public DateTime AtualizadoEm { get; init; }
    public bool PossuiVersaoPublicada { get; init; }
}