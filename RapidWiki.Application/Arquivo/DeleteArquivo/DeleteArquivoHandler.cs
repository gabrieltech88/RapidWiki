using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.DeleteArquivo;

public class DeleteArquivoHandler
    : IRequestHandler<DeleteArquivoRequest, bool>
{
    private readonly IArquivoRepository _arquivoRepository;
    private readonly IArquivoStorageService _arquivoStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArquivoHandler(
        IArquivoRepository arquivoRepository,
        IArquivoStorageService arquivoStorageService,
        IUnitOfWork unitOfWork)
    {
        _arquivoRepository = arquivoRepository;
        _arquivoStorageService = arquivoStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteArquivoRequest request,
        CancellationToken cancellationToken)
    {
        var arquivo =
            await _arquivoRepository.GetByIdAsync(
                request.ArquivoId
            );

        if (arquivo is null)
            return false;

        await _arquivoStorageService.ExcluirAsync(
            arquivo.Id,
            arquivo.Nome
        );

        await _arquivoRepository.DeleteAsync(
            arquivo.Id
        );

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}