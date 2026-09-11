using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.CreateProcedimento;

public class CreateProcedimentoHandler : IRequestHandler<CreateProcedimentoRequest, Guid>
{
    private readonly IRepository<Procedimento> _procedimentoRepository;
    private readonly IRepository<Departamento> _departamentoRepository;
    private readonly IMapper _mapper;

    public CreateProcedimentoHandler(IRepository<Procedimento> procedimentoRepository, IMapper mapper, IRepository<Departamento> departamentoRepository)
    {
        _procedimentoRepository = procedimentoRepository;
        _mapper = mapper;
        _departamentoRepository = departamentoRepository;
    }

    public async Task<Guid> Handle(CreateProcedimentoRequest request, CancellationToken cancellationToken)
    {
        if(request == null) throw new ArgumentNullException(nameof(request));
        
        var procedimento = _mapper.Map<Procedimento>(request);

        foreach (var departamentoId in request.DepartamentosIds)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);
            if (departamento == null)
            {
                throw new ArgumentException($"Departamento with ID {departamentoId} not found.");
            }
            procedimento.Departamentos.Add(departamento);
        }

        var result = await _procedimentoRepository.CreateAsync(procedimento);

        return result.Id;

        
    }
}