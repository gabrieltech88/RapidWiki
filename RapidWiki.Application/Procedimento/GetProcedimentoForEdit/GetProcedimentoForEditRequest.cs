using MediatR;
using RapidWiki.Application.GetProcedimentoById;

namespace RapidWiki.Application.GetProcedimentoForEdit;

public record GetProcedimentoForEditRequest(
    Guid Id
) : IRequest<GetProcedimentoByIdResult>;