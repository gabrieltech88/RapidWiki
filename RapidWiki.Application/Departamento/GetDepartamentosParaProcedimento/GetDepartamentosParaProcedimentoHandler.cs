using MediatR;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetDepartamentosParaProcedimento;

public class GetDepartamentosParaProcedimentoHandler : IRequestHandler<GetDepartamentosParaProcedimentoRequest, IReadOnlyCollection<DepartamentoDto>>
{
    private readonly IDepartamentoRepository _departamentoRepository;

    public GetDepartamentosParaProcedimentoHandler(IDepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<IReadOnlyCollection<DepartamentoDto>> Handle(GetDepartamentosParaProcedimentoRequest request, CancellationToken cancellationToken)
    {
        var departamentos = await _departamentoRepository.GetAllAsync();

        return departamentos
            .OrderBy(d => d.Nome)
            .Select(d => new DepartamentoDto
            {
                Id = d.Id,
                Nome = d.Nome
            })
            .ToList();
    }
}