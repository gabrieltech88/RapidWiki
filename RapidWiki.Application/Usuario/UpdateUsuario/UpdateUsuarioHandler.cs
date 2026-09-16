using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.UpdateUsuario;

public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartamentoRepository _departamentoRepository;

    public UpdateUsuarioHandler(IDepartamentoRepository departamentoRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser, IUsuarioRepository usuarioRepository, IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _usuarioRepository = usuarioRepository;
        _identityService = identityService;
        _departamentoRepository = departamentoRepository;
    }

    public async Task<Guid> Handle(UpdateUsuarioRequest request, CancellationToken cancellationToken)
    {
        if(request.DepartamentosIds.Count == 0)
        {
            throw new ArgumentException("O usuário deve possuir pelo menos um departamento");
        }

        var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
        if(usuario is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        var departamentosIds = request.DepartamentosIds.Distinct().ToList();
        var departamentos = new List<Departamento>();

        foreach(var departamentoId in departamentosIds)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);

            if (departamento is null)
            {
                throw new KeyNotFoundException($"Departamento com ID {departamentoId} não encontrado.");
            }

            departamentos.Add(departamento);
        }

         await _unitOfWork
            .BeginTransactionAsync();

        try
        {
            usuario.Atualizar(request.Nome, departamentos);
            
            await _identityService.UpdateUsuarioAsync(request.Id, request.Email, request.Role);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return usuario.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}