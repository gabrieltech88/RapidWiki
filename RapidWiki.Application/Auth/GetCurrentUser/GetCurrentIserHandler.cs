using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetCurrentUser;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResult>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUsuarioRepository _usuarioRepository;

    public GetCurrentUserHandler(ICurrentUser currentUser, IUsuarioRepository usuarioRepository)
    {
        _currentUser = currentUser;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GetCurrentUserResult> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(_currentUser.Id);

        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        return new GetCurrentUserResult
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Role = _currentUser.Role,
            DeveAlterarSenha = usuario.DeveAlterarSenha
        };
    }
}