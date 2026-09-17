using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.UpdateProcedimento;

public class UpdateProcedimentoHandler : IRequestHandler<UpdateProcedimentoRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly IProcedimentoRascunhoRepository _procedimentoRascunhoRepository;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProcedimentoHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository, IProcedimentoRascunhoRepository procedimentoRascunhoRepository, IDepartamentoRepository departamentoRepository, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _procedimentoRepository = procedimentoRepository;
        _procedimentoRascunhoRepository = procedimentoRascunhoRepository;
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

        var departamentosIds = request.DepartamentosIds.Distinct().ToList();

        if (departamentosIds.Count == 0)
        {
            throw new ArgumentException("Selecione pelo menos um departamento.");
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

        if (procedimento.Status == StatusProcedimento.Publicado && request.Status == StatusProcedimento.Rascunho)
        {
            var rascunho = await _procedimentoRascunhoRepository.GetByProcedimentoIdAsync(procedimento.Id, cancellationToken);

            if (rascunho is null)
            {
                rascunho = new ProcedimentoRascunho(
                    procedimento,
                    request.Titulo,
                    request.Descricao,
                    request.Conteudo,
                    departamentos,
                    usuarioId
                );

                await _procedimentoRascunhoRepository.CreateAsync(rascunho, cancellationToken);
            }
            else
            {
                rascunho.Atualizar(
                    request.Titulo,
                    request.Descricao,
                    request.Conteudo,
                    departamentos,
                    usuarioId
                );
            }

            await _unitOfWork.SaveChangesAsync();

            return procedimento.Id;
        }

        if (procedimento.Status == StatusProcedimento.Rascunho && request.Status == StatusProcedimento.Rascunho)
        {
            procedimento.Atualizar(
                request.Titulo,
                request.Descricao,
                request.Conteudo,
                StatusProcedimento.Rascunho,
                departamentos,
                usuarioId
            );

            await _unitOfWork.SaveChangesAsync();

            return procedimento.Id;
        }

        if (request.Status == StatusProcedimento.Publicado)
        {
            procedimento.Atualizar(
                request.Titulo,
                request.Descricao,
                request.Conteudo,
                StatusProcedimento.Publicado,
                departamentos,
                usuarioId
            );

            var rascunho = await _procedimentoRascunhoRepository.GetByProcedimentoIdAsync(procedimento.Id, cancellationToken);

            if (rascunho is not null)
            {
                _procedimentoRascunhoRepository.Delete(rascunho);
            }

            await _unitOfWork.SaveChangesAsync();

            return procedimento.Id;
        }

        throw new ArgumentException("Status do procedimento inválido.");
    }
}