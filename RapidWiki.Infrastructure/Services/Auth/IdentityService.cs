using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Usuario.CreateUsuario;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IRepository<Departamento> _departamentoRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<IdentityUser<Guid>> userManager, IRepository<Departamento> departamentoRepository, IRepository<Usuario> usuarioRepository, IMapper mapper)
    {
        _userManager = userManager;
        _departamentoRepository = departamentoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateUserAsync(CreateUsuarioRequest request)
    {
        var usuario = _mapper.Map<Usuario>(request);
        var claims = new List<Claim>();

        foreach (var departamentoId in request.DepartamentoIds)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);
            if (departamento == null)
            {
                throw new NullReferenceException($"Departamento with ID {departamentoId} not found.");
            }

            claims.Add(new Claim("Departamento", departamento.Id.ToString()));
            usuario.Departamentos.Add(departamento);
        }

        var createdUsuario = await _usuarioRepository.CreateAsync(usuario);

        var identityUser = new IdentityUser<Guid>
        {   
            Id = createdUsuario.Id,
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await _userManager.AddToRolesAsync(identityUser, request.Roles);
        await _userManager.AddClaimsAsync(identityUser, claims);

        return createdUsuario.Id;
    }

    
}