using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.UpdateProcedimento;
public class UpdateProcedimentoHandler : IRequestHandler<UpdateProcedimentoRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProcedimentoHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository, IDepartamentoRepository departamentoRepository, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _procedimentoRepository = procedimentoRepository;
        _departamentoRepository = departamentoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateProcedimentoRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var isAdmin = _currentUser.IsInRole("Admin");

        var procedimento = await _procedimentoRepository.GetByIdForUpdateAsync(request.Id, usuarioId, isAdmin, cancellationToken);

        if (procedimento is null)
        {
            throw new KeyNotFoundException("Procedimento não encontrado.");
        }

        var departamentosPermitidos = await _departamentoRepository.GetByUserAsync(usuarioId, isAdmin, cancellationToken);
        var departamentosPermitidosIds = departamentosPermitidos.Select(d => d.Id).ToHashSet();

        var departamentosIds = request.DepartamentosIds.Distinct().ToList();

        if (departamentosIds.Count == 0)
        {
            throw new ArgumentException("Selecione pelo menos um departamento.");
        }

        var possuiDepartamentoNaoPermitido = departamentosIds.Any(id => !departamentosPermitidosIds.Contains(id));

        if (possuiDepartamentoNaoPermitido)
        {
            throw new UnauthorizedAccessException("Você não possui acesso a um ou mais departamentos selecionados.");
        }

        var departamentos = new List<Departamento>();

        foreach (var departamentoId in departamentosIds)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);

            if (departamento is null)
            {
                throw new KeyNotFoundException($"Departamento {departamentoId} não encontrado.");
            }

            departamentos.Add(departamento);
        }

        procedimento.Atualizar(
            request.Titulo,
            request.Descricao,
            request.Conteudo,
            request.Status,
            departamentos
        );

        await _unitOfWork.SaveChangesAsync();
        return procedimento.Id;
    }
}