using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;


namespace RapidWiki.Application.CreateDepartamento;
public class CreateDepartamentoHandler : IRequestHandler<CreateDepartamentoRequest, CreateDepartamentoResult>
{
    private readonly IRepository<Departamento> _departamentoRepository;
    private readonly IMapper _mapper;

    public CreateDepartamentoHandler(IRepository<Departamento> departamentoRepository, IMapper mapper)
    {
        _departamentoRepository = departamentoRepository;
        _mapper = mapper;
    }
    public async Task<CreateDepartamentoResult> Handle(CreateDepartamentoRequest request, CancellationToken cancellationToken)
    {
        if(request is null) throw new ArgumentNullException(nameof(request));

        var departamentoMapped = _mapper.Map<Departamento>(request);

        var departamentoCreated = await _departamentoRepository.CreateAsync(departamentoMapped);

        var departmentoResultMapped = _mapper.Map<CreateDepartamentoResult>(departamentoCreated);
        
        return departmentoResultMapped;
    }
}
