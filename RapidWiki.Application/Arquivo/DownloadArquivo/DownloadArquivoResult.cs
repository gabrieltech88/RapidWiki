namespace RapidWiki.Application.DownloadArquivo;

public class DownloadArquivoResult
{
    public Stream Stream { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Tipo { get; set; } = null!;
}