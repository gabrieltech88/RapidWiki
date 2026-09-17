using MediatR;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.CreateUsuario
{
    public class CreateUsuarioRequest : IRequest<CreateUsuarioResult>
    {
        required public string Nome { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
        public IEnumerable<Guid> DepartamentoIds { get; set; } = [];
        required public string Role { get; set; }

    }
}