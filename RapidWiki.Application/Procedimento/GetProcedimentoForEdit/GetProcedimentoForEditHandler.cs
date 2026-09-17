using MediatR;
using RapidWiki.Application.Common;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.GetProcedimentoById;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.GetProcedimentoForEdit;

public class GetProcedimentoForEditHandler
    : IRequestHandler<
        GetProcedimentoForEditRequest,
        GetProcedimentoByIdResult>
{
    private readonly IProcedimentoRepository
        _procedimentoRepository;

    private readonly IProcedimentoRascunhoRepository
        _procedimentoRascunhoRepository;

    private readonly ICurrentUser
        _currentUser;


    public GetProcedimentoForEditHandler(
        IProcedimentoRepository procedimentoRepository,
        IProcedimentoRascunhoRepository procedimentoRascunhoRepository,
        ICurrentUser currentUser)
    {
        _procedimentoRepository =
            procedimentoRepository;

        _procedimentoRascunhoRepository =
            procedimentoRascunhoRepository;

        _currentUser =
            currentUser;
    }


    public async Task<GetProcedimentoByIdResult> Handle(
        GetProcedimentoForEditRequest request,
        CancellationToken cancellationToken)
    {
        var usuarioId =
            _currentUser.Id;

        var isAdmin =
            _currentUser.IsInRole(
                "Admin"
            );


        /*
         * Busca o procedimento original.
         *
         * Para Editor, continua respeitando
         * os departamentos permitidos.
         *
         * Para Admin, possui acesso global.
         */
        var procedimento =
            await _procedimentoRepository
                .GetByIdByUserAsync(
                    request.Id,
                    usuarioId,
                    isAdmin
                );


        if (procedimento is null)
        {
            throw new KeyNotFoundException(
                "Procedimento não encontrado."
            );
        }


        /*
         * Só procuramos uma revisão separada
         * quando o procedimento já possui uma
         * versão publicada.
         */
        if (
            procedimento.Status ==
            StatusProcedimento.Publicado
        )
        {
            var rascunho =
                await _procedimentoRascunhoRepository
                    .GetByProcedimentoIdAsync(
                        procedimento.Id,
                        cancellationToken
                    );


            if (rascunho is not null)
            {
                return new GetProcedimentoByIdResult
                {
                    /*
                     * Mantemos o ID do procedimento.
                     *
                     * O frontend continua editando:
                     *
                     * /procedures/{procedimentoId}/edit
                     *
                     * e não precisa conhecer o ID
                     * interno da revisão.
                     */
                    Id =
                        procedimento.Id,

                    Titulo =
                        rascunho.Titulo,

                    Descricao =
                        rascunho.Descricao,

                    Autor =
                        new AutorDto
                        {
                            Id =
                                procedimento.Autor.Id,

                            Nome =
                                procedimento.Autor.Nome
                        },

                    Departamentos =
                        rascunho
                            .Departamentos
                            .Select(
                                departamento =>
                                    new DepartamentoDto
                                    {
                                        Id =
                                            departamento.Id,

                                        Nome =
                                            departamento.Nome
                                    }
                            )
                            .ToList(),

                    Conteudo =
                        rascunho.Conteudo,

                    /*
                     * Para o formulário, essa
                     * versão é um rascunho.
                     */
                    Status =
                        StatusProcedimento.Rascunho,

                    CriadoEm =
                        procedimento.CriadoEm,

                    AtualizadoEm =
                        rascunho.AtualizadoEm
                };
            }
        }


        /*
         * Não existe revisão.
         *
         * Pode ser:
         *
         * - procedimento publicado ainda sem edição;
         * - procedimento que nunca foi publicado.
         */
        return new GetProcedimentoByIdResult
        {
            Id =
                procedimento.Id,

            Titulo =
                procedimento.Titulo,

            Descricao =
                procedimento.Descricao,

            Autor =
                new AutorDto
                {
                    Id =
                        procedimento.Autor.Id,

                    Nome =
                        procedimento.Autor.Nome
                },

            Departamentos =
                procedimento
                    .Departamentos
                    .Select(
                        departamento =>
                            new DepartamentoDto
                            {
                                Id =
                                    departamento.Id,

                                Nome =
                                    departamento.Nome
                            }
                    )
                    .ToList(),

            Conteudo =
                procedimento.Conteudo,

            Status =
                procedimento.Status,

            CriadoEm =
                procedimento.CriadoEm,

            AtualizadoEm =
                procedimento.AtualizadoEm
        };
    }
}