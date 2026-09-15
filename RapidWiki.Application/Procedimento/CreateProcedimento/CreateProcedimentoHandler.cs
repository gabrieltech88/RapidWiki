using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.CreateProcedimento;

public class CreateProcedimentoHandler : IRequestHandler<CreateProcedimentoRequest, Guid>
{
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateProcedimentoHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository, IUnitOfWork unitOfWork,IMapper mapper, IDepartamentoRepository departamentoRepository)
    {
        _procedimentoRepository = procedimentoRepository;
        _mapper = mapper;
        _departamentoRepository = departamentoRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateProcedimentoRequest request, CancellationToken cancellationToken)
    {
        if(request == null) throw new ArgumentNullException(nameof(request));
        
        var procedimento = _mapper.Map<Procedimento>(request);

        procedimento.AutorId = _currentUser.Id;

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
        await _unitOfWork.SaveChangesAsync();

        return result.Id;

        
    }
}