using MediatR;

namespace RapidWiki.Application.GetProcedimentosRequest;

public record GetProcedimentosRequest(

) : IRequest<GetProcedimentosResult>;