public interface IArquivoStorageService
{
    Task SaveAsync(Guid arquivoId, string nomeArquivo, Stream conteudo);
    Task ExcluirAsync(Guid arquivoId, string nomeArquivo);
    Task<Stream?> OpenReadAsync(Guid arquivoId, string nomeArquivo);
    
}