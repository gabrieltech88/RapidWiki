using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;

namespace RapidWiki.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;
    private readonly IRepository<Departamento> _departamentoRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityService(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<IdentityUser<Guid>> signInManager, IRepository<Departamento> departamentoRepository, IRepository<Usuario> usuarioRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _departamentoRepository = departamentoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentException("Role name cannot be null or whitespace.", nameof(roleName));

        var role = new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = roleName };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        return role.Id;
    }

    public async Task<Guid> CreateUsuarioAsync(CreateUsuarioRequest request)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var usuario = _mapper.Map<Usuario>(request);

            foreach (var departamentoId in request.DepartamentoIds)
            {
                var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);
                if (departamento == null)
                {
                    throw new NullReferenceException($"Departamento with ID {departamentoId} not found.");
                }

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

            var roleResult = await _userManager.AddToRoleAsync(identityUser, request.Role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", roleResult.Errors.Select(x => x.Description)));
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return createdUsuario.Id;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new InvalidOperationException($"Error creating user: {ex.Message}", ex);
        }
    }

    public async Task<Application.SignIn.SignInResult> SignInAsync(SignInRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new UnauthorizedAccessException("Email ou senha inválidos.");

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!result.Succeeded) throw new UnauthorizedAccessException("Email ou senha inválidos.");
        var roles = await _userManager.GetRolesAsync(user);

        return new Application.SignIn.SignInResult
        {
            Id = user.Id,
            Role = roles.SingleOrDefault()
                ?? throw new InvalidOperationException("Usuário não possui uma role.")
        };
    }
}