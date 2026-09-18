using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(
        ICurrentUser currentUser,
        IUsuarioRepository usuarioRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _usuarioRepository = usuarioRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SenhaAtual))
        {
            throw new ArgumentException("A senha atual é obrigatória.");
        }

        if (string.IsNullOrWhiteSpace(request.NovaSenha))
        {
            throw new ArgumentException("A nova senha é obrigatória.");
        }

        var usuario = await _usuarioRepository.GetByIdAsync(_currentUser.Id);

        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _identityService.ChangePasswordAsync(
                usuario.Id,
                request.SenhaAtual,
                request.NovaSenha
            );

            usuario.ConfirmarTrocaDeSenha();

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}