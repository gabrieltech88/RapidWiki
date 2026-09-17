using AutoMapper;
using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.CreateUsuario;

public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioRequest,CreateUsuarioResult>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUsuarioHandler(IUsuarioRepository usuarioRepository,
        IDepartamentoRepository departamentoRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _departamentoRepository = departamentoRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateUsuarioResult> Handle(CreateUsuarioRequest request, CancellationToken cancellationToken)
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
                    throw new InvalidOperationException($"Departamento {departamentoId} não encontrado.");
                }

                usuario.Departamentos.Add(departamento);
            }

            await _usuarioRepository.CreateAsync(usuario);

            await _identityService.CreateUsuarioAsync(usuario.Id, request.Email, request.Password, request.Role);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return new CreateUsuarioResult
            {
                Id = usuario.Id
            };
        }
        catch
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }
}