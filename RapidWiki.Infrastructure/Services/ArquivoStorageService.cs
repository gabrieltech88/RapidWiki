using Microsoft.Extensions.Configuration;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Infrastructure.Storage;

public class ArquivoStorageService : IArquivoStorageService
{
    private readonly string _filesPath;

    public ArquivoStorageService(IConfiguration configuration)
    {
        _filesPath = configuration["Storage:FilesPath"] ?? throw new InvalidOperationException("Storage:FilesPath não está configurado.");
    }

    public async Task SaveAsync(Guid arquivoId, string nomeArquivo, Stream conteudo)
    {
        var diretorioArquivo = Path.Combine(_filesPath, arquivoId.ToString());

        Directory.CreateDirectory(diretorioArquivo);

        var nomeSeguro = Path.GetFileName(nomeArquivo);

        var caminhoArquivo = Path.Combine(diretorioArquivo, nomeSeguro);

        await using var fileStream = new FileStream(caminhoArquivo, FileMode.CreateNew, FileAccess.Write, FileShare.None);

        await conteudo.CopyToAsync(fileStream);
    }

    public Task ExcluirAsync(Guid arquivoId, string nomeArquivo)
    {
        var nomeSeguro = Path.GetFileName(nomeArquivo);

        var caminhoArquivo = Path.Combine(_filesPath, arquivoId.ToString(), nomeSeguro);

        if (File.Exists(caminhoArquivo))
        {
            File.Delete(caminhoArquivo);
        }

        var diretorioArquivo = Path.Combine(_filesPath, arquivoId.ToString());

        if (Directory.Exists(diretorioArquivo))
        {
            Directory.Delete(diretorioArquivo, recursive: true);
        }

        return Task.CompletedTask;
    }

    public Task<Stream?> OpenReadAsync(Guid arquivoId, string nomeArquivo)
    {
        var nomeSeguro = Path.GetFileName(nomeArquivo);

        var caminho = Path.Combine(_filesPath, arquivoId.ToString(), nomeSeguro);

        if (!File.Exists(caminho))
            return Task.FromResult<Stream?>(null);

        Stream stream = new FileStream(caminho, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult<Stream?>(stream);
    }
}