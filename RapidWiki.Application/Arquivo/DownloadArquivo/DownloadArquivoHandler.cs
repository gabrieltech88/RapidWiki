using MediatR;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Application.DownloadArquivo;

public class DownloadArquivoHandler
    : IRequestHandler<
        DownloadArquivoRequest,
        DownloadArquivoResult?>
{
    private readonly ICurrentUser _currentUser;
    private readonly IArquivoRepository _arquivoRepository;
    private readonly IArquivoStorageService _arquivoStorageService;

    public DownloadArquivoHandler(
        ICurrentUser currentUser,
        IArquivoRepository arquivoRepository,
        IArquivoStorageService arquivoStorageService)
    {
        _currentUser = currentUser;
        _arquivoRepository = arquivoRepository;
        _arquivoStorageService = arquivoStorageService;
    }

    public async Task<DownloadArquivoResult?> Handle(
        DownloadArquivoRequest request,
        CancellationToken cancellationToken)
    {
        var usuarioId =
            _currentUser.Id;

        var isAdmin =
            _currentUser.IsInRole("Admin");

        var arquivo =
            await _arquivoRepository
                .GetAccessibleByIdAsync(
                    request.ArquivoId,
                    usuarioId,
                    isAdmin,
                    cancellationToken
                );

        if (arquivo is null)
            return null;

        var stream =
            await _arquivoStorageService
                .OpenReadAsync(
                    arquivo.Id,
                    arquivo.Nome
                );

        if (stream is null)
            return null;

        return new DownloadArquivoResult
        {
            Stream = stream,
            Nome = arquivo.Nome,
            Tipo = arquivo.Tipo
        };
    }
}