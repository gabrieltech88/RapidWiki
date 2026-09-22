using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.GetProcedimentosRequest;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class ProcedimentoRepository : IProcedimentoRepository
{
    private readonly RapidWikiDbContext _context;

    public ProcedimentoRepository(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task<Procedimento> CreateAsync(Procedimento entity)
    {
        var result = await _context.Procedimentos.AddAsync(entity);
        return result.Entity;
    }

    public async Task<GetProcedimentosResult> GetPagedByUserAsync(Guid usuarioId, bool hasGlobalAccess, int page, string? search, Guid? departamentoId, StatusProcedimento? status, CancellationToken cancellationToken = default)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var query = _context.Procedimentos.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        if (departamentoId.HasValue)
        {
            var departamentoSelecionadoId = departamentoId.Value;

            if (hasGlobalAccess)
            {
                query = query.Where(p => p.Departamentos.Any(d => d.Id == departamentoSelecionadoId));
            }
            else
            {
                query = query.Where(p => p.Departamentos.Any(d => d.Id == departamentoSelecionadoId && d.Usuarios.Any(u => u.Id == usuarioId)));
            }
        }
        else if (!hasGlobalAccess)
        {
            query = query.Where(p => p.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p => p.Titulo.Contains(term) || p.Descricao.Contains(term));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.AtualizadoEm)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProcedimentoDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Descricao = p.Descricao,
                Conteudo = p.Conteudo,
                Autor = new AutorDto
                {
                    Id = p.Autor.Id,
                    Nome = p.Autor.Nome,
                },
                AtualizadoEm = p.AtualizadoEm,
                Status = p.Status
            })
            .ToListAsync(cancellationToken);

        return new GetProcedimentosResult
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
        };
    }

    public async Task<Procedimento?> GetByIdForUpdateAsync(Guid procedimentoId, Guid usuarioId, bool hasGlobalAccess, CancellationToken cancellationToken = default)
    {
        var query = _context.Procedimentos
            .Include(p => p.Departamentos)
            .Where(p => p.Id == procedimentoId);

        if (!hasGlobalAccess)
        {
            query = query.Where(p => p.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id)
    {
        var procedimento = await _context.Procedimentos
            .Include(p => p.Departamentos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (procedimento is null)
        {
            return;
        }

        _context.Procedimentos.Remove(procedimento);
    }

    public async Task<IEnumerable<Procedimento>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Procedimento> GetByIdByUserAsync(Guid procedimentoId, Guid usuarioId, bool hasGlobalAccess)
    {
        var query = _context.Procedimentos
            .AsNoTracking()
            .Include(p => p.Autor)
            .Include(p => p.Departamentos)
            .Where(p => p.Id == procedimentoId);

        if (!hasGlobalAccess)
        {
            query = query.Where(p => p.Departamentos.Any(d => d.Usuarios.Any(u => u.Id == usuarioId)));
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Procedimento> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Procedimento entity)
    {
        throw new NotImplementedException();
    }
}
