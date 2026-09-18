using MediatR;

namespace RapidWiki.Application.ChangePassword;

public record ChangePasswordRequest(
    string SenhaAtual,
    string NovaSenha
) : IRequest;