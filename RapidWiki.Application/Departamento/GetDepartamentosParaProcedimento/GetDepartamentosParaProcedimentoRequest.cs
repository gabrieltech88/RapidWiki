using MediatR;
using RapidWiki.Application.Common.Dto;

namespace RapidWiki.Application.GetDepartamentosParaProcedimento;

public record GetDepartamentosParaProcedimentoRequest : IRequest<IReadOnlyCollection<DepartamentoDto>>;