using MediatR;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.Usuario.CreateUsuario
{
    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioRequest, CreateUsuarioResult>
    {
       
        public async Task<CreateUsuarioResult> Handle(CreateUsuarioRequest request, CancellationToken cancellationToken)
        {
            
        }
    }
}