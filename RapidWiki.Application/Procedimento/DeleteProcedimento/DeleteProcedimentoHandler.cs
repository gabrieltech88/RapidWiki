using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.DeleteProcedimento;

public class DeleteProcedimentoHandler : IRequestHandler<DeleteProcedimentoRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly IProcedimentoRascunhoRepository _procedimentoRascunhoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProcedimentoHandler(ICurrentUser currentUser, IProcedimentoRepository procedimentoRepository, IProcedimentoRascunhoRepository procedimentoRascunhoRepository, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _procedimentoRepository = procedimentoRepository;
        _procedimentoRascunhoRepository = procedimentoRascunhoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(DeleteProcedimentoRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var isAdmin = _currentUser.IsInRole("Admin");
        var isEditor = _currentUser.IsInRole("Editor");

        if (!isAdmin && !isEditor)
        {
            throw new UnauthorizedAccessException("Você não possui permissão para excluir procedimentos.");
        }

        var procedimento = await _procedimentoRepository.GetByIdForUpdateAsync(request.Id, usuarioId, isAdmin, cancellationToken);

        if (procedimento is null)
        {
            throw new KeyNotFoundException("Procedimento não encontrado.");
        }

        var rascunho = await _procedimentoRascunhoRepository.GetByProcedimentoIdAsync(request.Id, cancellationToken);

        if (rascunho is not null)
        {
            _procedimentoRascunhoRepository.Delete(rascunho);
        }

        await _procedimentoRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return request.Id;
    }
}
