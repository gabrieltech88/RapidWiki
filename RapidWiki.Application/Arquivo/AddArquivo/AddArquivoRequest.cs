using MediatR;

namespace RapidWiki.Application.AddArquivo;
public record AddArquivoRequest(
    string Nome,
    string Tipo,
    long TamanhoBytes,
    Stream Conteudo,
    IReadOnlyCollection<Guid> DepartamentoIds
) : IRequest<Guid>;

