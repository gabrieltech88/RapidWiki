using Microsoft.EntityFrameworkCore;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.GetArquivos;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories;

public class ArquivoRepository : IArquivoRepository
{
    private readonly RapidWikiDbContext _context;

    public ArquivoRepository(RapidWikiDbContext context)
    {
        _context = context;
    }

    public async Task<Arquivo> CreateAsync(Arquivo entity)
    {
        var result = await _context.Arquivos.AddAsync(entity);

        return result.Entity;
    }

    public async Task<GetArquivosResult> GetPagedByUserAsync(Guid usuarioId, bool hasGlobalAccess, int page, string? search, Guid? departamentoId, CancellationToken cancellationToken = default)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var query = _context.Arquivos.AsNoTracking().AsQueryable();

        if (departamentoId.HasValue)
        {
            var departamentoSelecionadoId = departamentoId.Value;

            if (hasGlobalAccess)
            {
                query = query.Where(arquivo => arquivo.Departamentos.Any(departamento => departamento.Id == departamentoSelecionadoId));
            }
            else
            {
                query = query.Where(arquivo => arquivo.Departamentos
                    .Any(departamento => departamento.Id == departamentoSelecionadoId &&
                        departamento.Usuarios.Any(usuario => usuario.Id == usuarioId))
                );
            }
        }
        else if (!hasGlobalAccess)
        {
            query = query.Where(arquivo => arquivo.Departamentos
                .Any(departamento => departamento.Usuarios.Any(usuario => usuario.Id == usuarioId))
            );
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();

            query = query.Where(arquivo => arquivo.Nome.Contains(term) || arquivo.Tipo.Contains(term));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(arquivo => arquivo.CriadoEm)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(arquivo => new ArquivoDto
            {
                Id = arquivo.Id,
                Nome = arquivo.Nome,
                Tipo = arquivo.Tipo,
                TamanhoBytes = arquivo.TamanhoBytes,
                CriadoEm = arquivo.CriadoEm,
                Departamentos = arquivo.Departamentos.Select(departamento => new ArquivoDepartamentoDto
                {
                    Id = departamento.Id,
                    Nome = departamento.Nome
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new GetArquivosResult
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var arquivo = await _context.Arquivos.FindAsync(id);

        if (arquivo is null)
            return;

        _context.Arquivos.Remove(arquivo);
    }

    public async Task<IEnumerable<Arquivo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Arquivo> GetByIdAsync(Guid id)
    {
        return await _context.Arquivos.FindAsync(id);
    }

    public Task UpdateAsync(Arquivo entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Arquivo?> GetAccessibleByIdAsync(
    Guid arquivoId,
    Guid usuarioId,
    bool hasGlobalAccess,
    CancellationToken cancellationToken = default)
    {
        var query = _context.Arquivos
            .AsNoTracking()
            .Include(arquivo => arquivo.Departamentos)
            .AsQueryable();

        query = query.Where(
            arquivo =>
                arquivo.Id == arquivoId
        );

        if (!hasGlobalAccess)
        {
            query = query.Where(
                arquivo =>
                    arquivo.Departamentos.Any(
                        departamento =>
                            departamento.Usuarios.Any(
                                usuario =>
                                    usuario.Id == usuarioId
                            )
                    )
            );
        }

        return await query
            .FirstOrDefaultAsync(
                cancellationToken
            );
    }
}