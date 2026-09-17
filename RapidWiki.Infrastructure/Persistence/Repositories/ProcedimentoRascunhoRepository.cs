using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Commmon.Dto;
using RapidWiki.Application.GetRascunhos;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class ProcedimentoRascunhoRepository : IProcedimentoRascunhoRepository
{
    private readonly RapidWikiDbContext _context;

    public ProcedimentoRascunhoRepository(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task<ProcedimentoRascunho?> GetByProcedimentoIdAsync(Guid procedimentoId, CancellationToken cancellationToken = default)
    {
        return await _context.ProcedimentosRascunhos
            .Include(r => r.Departamentos)
            .FirstOrDefaultAsync(r => r.ProcedimentoId == procedimentoId, cancellationToken);
    }

    public async Task<ProcedimentoRascunho?> GetByProcedimentoIdByUserAsync(Guid procedimentoId, Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default)
    {
        var query = _context.ProcedimentosRascunhos
            .Include(r => r.Departamentos)
            .Where(r => r.ProcedimentoId == procedimentoId);

        if (!hasGlobalAccess)
        {
            query = query.Where(r => r.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(ProcedimentoRascunho rascunho, CancellationToken cancellationToken = default)
    {
        await _context.ProcedimentosRascunhos.AddAsync(rascunho, cancellationToken);
    }

    public void Delete(ProcedimentoRascunho rascunho)
    {
        _context.ProcedimentosRascunhos.Remove(rascunho);
    }

    public async Task<GetRascunhosResult> GetPagedAsync(Guid usuarioId, bool hasGlobalAccess, int page, string? search, CancellationToken cancellationToken = default)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var procedimentosRascunho = _context.Procedimentos
            .AsNoTracking()
            .Where(p => p.Status == StatusProcedimento.Rascunho);

        var revisoesRascunho = _context.ProcedimentosRascunhos
            .AsNoTracking()
            .AsQueryable();

        if (!hasGlobalAccess)
        {
            procedimentosRascunho = procedimentosRascunho
                .Where(p => p.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));

            revisoesRascunho = revisoesRascunho
                .Where(r => r.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));
        }

        var novosRascunhos = procedimentosRascunho
            .Select(p => new
            {
                Id = p.Id,
                p.Titulo,
                p.Descricao,
                AutorId = p.Autor.Id,
                AutorNome = p.Autor.Nome,
                EditorId = p.AtualizadoPorId ?? p.AutorId,
                EditorNome = p.AtualizadoPor != null ? p.AtualizadoPor.Nome : p.Autor.Nome,
                p.AtualizadoEm,
                PossuiVersaoPublicada = false
            });

        var alteracoesNaoPublicadas = revisoesRascunho
            .Select(r => new
            {
                Id = r.ProcedimentoId,
                r.Titulo,
                r.Descricao,
                AutorId = r.Procedimento.Autor.Id,
                AutorNome = r.Procedimento.Autor.Nome,
                EditorId = r.Editor.Id,
                EditorNome = r.Editor.Nome,
                r.AtualizadoEm,
                PossuiVersaoPublicada = true
            });

        var query = novosRascunhos.Concat(alteracoesNaoPublicadas);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(r => r.Titulo.Contains(term) || r.Descricao.Contains(term));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var rawItems = await query
            .OrderByDescending(r => r.AtualizadoEm)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = rawItems
            .Select(r => new RascunhoDto
            {
                Id = r.Id,
                Titulo = r.Titulo,
                Descricao = r.Descricao,
                AutorId = r.AutorId,
                AutorNome = r.AutorNome,
                EditorId = r.EditorId,
                EditorNome = r.EditorNome,
                AtualizadoEm = r.AtualizadoEm,
                PossuiVersaoPublicada = r.PossuiVersaoPublicada
            })
            .ToList();

        return new GetRascunhosResult
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Items = items
        };
    }
}
