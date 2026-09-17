using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.UpdateDepartamento;

public class UpdateDepartamentoHandler : IRequestHandler<UpdateDepartamentoRequest, Guid>
{
    private readonly IDepartamentoRepository _departamentoRepository;

    public UpdateDepartamentoHandler(IDepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<Guid> Handle(UpdateDepartamentoRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("O nome do departamento é obrigatório.");

        var departamento = await _departamentoRepository.GetByIdAsync(request.Id);

        if (departamento is null)
            throw new KeyNotFoundException("Departamento não encontrado.");

        departamento.AtualizarNome(request.Nome.Trim());

        await _departamentoRepository.UpdateAsync(departamento);

        return departamento.Id;
    }
}