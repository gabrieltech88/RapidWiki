using MediatR;
using RapidWiki.Application.Common;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetUsuarios;

public class GetUsuariosHandler : IRequestHandler<GetUsuariosRequest, GetUsuariosResult>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IIdentityService _identityService;

    public GetUsuariosHandler(IUsuarioRepository usuarioRepository, IIdentityService identityService)
    {
        _usuarioRepository = usuarioRepository;
        _identityService = identityService;
    }

    public async Task<GetUsuariosResult> Handle(GetUsuariosRequest request, CancellationToken cancellationToken)
    {
        const int pageSize = 8;

        var page = request.Page < 1 ? 1 : request.Page;

        var (usuarios, totalItems) = await _usuarioRepository
            .GetPagedAsync(
                page,
                request.Search,
                pageSize,
                cancellationToken
            );


        var items = new List<UsuarioDto>();

        foreach (var usuario in usuarios)
        {
            var identity = await _identityService.GetUsuarioAsync(usuario.Id);
            if (identity is null)
            {
                continue;
            }

            items.Add(new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = identity.Email,
                Ativo = identity.Ativo,
                Role = identity.Role,
                Departamentos = usuario.Departamentos.Select(departamento => new DepartamentoDto
                {
                    Id = departamento.Id,
                    Nome = departamento.Nome
                }                    
                ).ToList()
            });
        }

        return new GetUsuariosResult
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}