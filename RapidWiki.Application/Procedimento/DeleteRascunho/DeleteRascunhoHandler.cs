using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.DeleteRascunho;

public class DeleteRascunhoHandler : IRequestHandler<DeleteRascunhoRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly IProcedimentoRascunhoRepository _procedimentoRascunhoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRascunhoHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository, IProcedimentoRascunhoRepository procedimentoRascunhoRepository, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _procedimentoRepository = procedimentoRepository;
        _procedimentoRascunhoRepository = procedimentoRascunhoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(DeleteRascunhoRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var isAdmin = _currentUser.IsInRole("Admin");
        var isEditor = _currentUser.IsInRole("Editor");

        if (!isAdmin && !isEditor)
        {
            throw new UnauthorizedAccessException("Você não possui permissão para excluir rascunhos.");
        }

        var revisao = await _procedimentoRascunhoRepository.GetByProcedimentoIdByUserAsync(request.Id, usuarioId, isAdmin, cancellationToken);

        if (revisao is not null)
        {
            _procedimentoRascunhoRepository.Delete(revisao);
            await _unitOfWork.SaveChangesAsync();

            return request.Id;
        }

        var procedimento = await _procedimentoRepository.GetByIdForUpdateAsync(request.Id, usuarioId, isAdmin, cancellationToken);

        if (procedimento is null || procedimento.Status != StatusProcedimento.Rascunho)
        {
            throw new KeyNotFoundException("Rascunho não encontrado.");
        }

        await _procedimentoRepository.DeleteAsync(procedimento.Id);
        await _unitOfWork.SaveChangesAsync();

        return request.Id;
    }
}
