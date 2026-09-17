using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.DeleteDepartamento;

public class DeleteDepartamentoHandler : IRequestHandler<DeleteDepartamentoRequest, Guid>
{
    private readonly IDepartamentoRepository _departamentoRepository;

    public DeleteDepartamentoHandler(IDepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<Guid> Handle(DeleteDepartamentoRequest request, CancellationToken cancellationToken)
    {
        var departamento = await _departamentoRepository.GetByIdAsync(request.Id);

        if (departamento is null)
            throw new KeyNotFoundException("Departamento não encontrado.");

        await _departamentoRepository.DeleteAsync(request.Id);

        return request.Id;
    }
}