using MediatR;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Application.AddArquivo;

public class AddArquivoHandler : IRequestHandler<AddArquivoRequest, Guid>
{
    private readonly IArquivoRepository _arquivoRepository;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IArquivoStorageService _arquivoStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddArquivoHandler(
        IArquivoRepository arquivoRepository,
        IDepartamentoRepository departamentoRepository,
        IArquivoStorageService arquivoStorageService,
        IUnitOfWork unitOfWork)
    {
        _arquivoRepository = arquivoRepository;
        _departamentoRepository = departamentoRepository;
        _arquivoStorageService = arquivoStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddArquivoRequest request, CancellationToken cancellationToken)
    {
        var departamentosIds = request.DepartamentoIds.Distinct().ToList();

        if (departamentosIds.Count == 0)
        {
            throw new ArgumentException(
                "Selecione pelo menos um departamento."
            );
        }

        var departamentos = new List<Departamento>();

        foreach (var departamentoId in departamentosIds)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(departamentoId);
            if (departamento is null)
            {
                throw new KeyNotFoundException($"Departamento {departamentoId} não encontrado.");
            }

            departamentos.Add(departamento);
        }

        var nomeArquivo = Path.GetFileName(request.Nome);

        var arquivo = new Arquivo(
            nomeArquivo,
            request.Tipo,
            request.TamanhoBytes
        );

        foreach (var departamento in departamentos)
        {
            arquivo.Departamentos.Add(departamento);
        }

        await _arquivoRepository.CreateAsync(arquivo);

        try
        {
            await _arquivoStorageService.SaveAsync(
                arquivo.Id,
                arquivo.Nome,
                request.Conteudo
            );

            await _unitOfWork.SaveChangesAsync();
        }
        catch
        {
            await _arquivoStorageService.ExcluirAsync(
                arquivo.Id,
                arquivo.Nome
            );

            throw;
        }

        return arquivo.Id;
    }
}