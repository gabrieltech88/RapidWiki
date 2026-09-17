using MediatR;

namespace RapidWiki.Application.DeleteRascunho;

public record DeleteRascunhoRequest(Guid Id) : IRequest<Guid>;
