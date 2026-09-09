using MediatR;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Usuario.CreateUsuario
{
    public class CreateUsuarioInIdentity
    {
        required public string Username { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
        required public Guid Id { get; set; }
        public IEnumerable<Guid> DepartamentoIds { get; set; } = [];
        public IEnumerable<string> Roles { get; set; } = [];

    }
}