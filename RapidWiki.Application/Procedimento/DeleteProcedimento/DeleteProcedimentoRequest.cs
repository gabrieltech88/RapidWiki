using MediatR;

namespace RapidWiki.Application.DeleteProcedimento;

public record DeleteProcedimentoRequest(Guid Id) : IRequest<Guid>;
