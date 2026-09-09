using MediatR;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Usuario.CreateUsuario
{
    public class CreateUsuarioRequest : IRequest<CreateUsuarioResult>
    {
        required public string Username { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
        public IEnumerable<Guid> DepartamentoIds { get; set; } = [];
        public IEnumerable<string> Roles { get; set; } = [];

    }
}