using MediatR;

namespace RapidWiki.Application.GetProcedimentoById; 

public record GetProcedimentoByIdRequest(
    Guid Id
) : IRequest<GetProcedimentoByIdResult>;