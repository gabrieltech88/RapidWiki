using MediatR;
using RapidWiki.Application.Common;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.GetProcedimentoById;

public class GetProcedimentoByIdHandler : IRequestHandler<GetProcedimentoByIdRequest, GetProcedimentoByIdResult>
{
    private readonly IProcedimentoRepository _procedimentoRepository;
    private readonly ICurrentUser _currentUser;

    public GetProcedimentoByIdHandler(IProcedimentoRepository procedimentoRepository, ICurrentUser currentUser)
    {
        _procedimentoRepository = procedimentoRepository;
        _currentUser = currentUser;
    }

    public async Task<GetProcedimentoByIdResult> Handle(GetProcedimentoByIdRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.Id;
        var hasGlobalAccess = _currentUser.IsInRole("Admin");

        var procedimento = await _procedimentoRepository.GetByIdByUserAsync(request.Id, usuarioId, hasGlobalAccess);

        if (procedimento is null)
        {
            throw new KeyNotFoundException("Procedimento não encontrado.");
        }

        return new GetProcedimentoByIdResult
        {
            Id = procedimento.Id,
            Titulo = procedimento.Titulo,
            Descricao = procedimento.Descricao,
            Autor = new AutorDto
            {
                Id = procedimento.Autor.Id,
                Nome = procedimento.Autor.Nome
            },
            Departamentos = procedimento.Departamentos.Select(departamento =>
                new DepartamentoDto
                {
                    Id = departamento.Id,
                    Nome = departamento.Nome
                }
            ).ToList(),
            Conteudo = procedimento.Conteudo,
            Status = procedimento.Status,
            CriadoEm = procedimento.CriadoEm,
            AtualizadoEm = procedimento.AtualizadoEm
        };
    }

    
}