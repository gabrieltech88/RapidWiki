using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;


namespace RapidWiki.Application.GetAllDepartamentos;
public class GetAllDepartamentosHandler : IRequestHandler<GetAllDepartamentosRequest, IEnumerable<GetAllDepartamentosResult>>
{
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllDepartamentosHandler(IDepartamentoRepository departamentoRepository, IMapper mapper, ICurrentUser currentUser)
    {
        _departamentoRepository = departamentoRepository;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<IEnumerable<GetAllDepartamentosResult>> Handle(GetAllDepartamentosRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var hasGlobalAccess = _currentUser.IsInRole("Admin");
        var departamentos = await _departamentoRepository.GetByUserAsync(usuarioId, hasGlobalAccess, cancellationToken);

        return departamentos.Select(departamento => 
                new GetAllDepartamentosResult
                {
                    Id = departamento.Id,
                    Nome = departamento.Nome
                }
            ).ToList();
    }
}
