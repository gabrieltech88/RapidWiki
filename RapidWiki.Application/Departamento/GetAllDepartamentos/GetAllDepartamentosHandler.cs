using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;


namespace RapidWiki.Application.GetAllDepartamentos;
public class GetAllDepartamentosHandler : IRequestHandler<GetAllDepartamentosRequest, IEnumerable<GetAllDepartamentosResult>>
{
    private readonly IRepository<Departamento> _departamentoRepository;
    private readonly IMapper _mapper;

    public GetAllDepartamentosHandler(IRepository<Departamento> departamentoRepository, IMapper mapper)
    {
        _departamentoRepository = departamentoRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<GetAllDepartamentosResult>> Handle(GetAllDepartamentosRequest request, CancellationToken cancellationToken)
    {
        var departamentos = await _departamentoRepository.GetAllAsync();

        if(departamentos == null || !departamentos.Any()) throw new NullReferenceException("Não foi encontrado nenhum departamento.");

        return departamentos.Select(d => _mapper.Map<GetAllDepartamentosResult>(d));
    }
}
