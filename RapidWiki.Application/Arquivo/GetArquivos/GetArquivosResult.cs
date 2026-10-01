using RapidWiki.Application.Common.Dto;

namespace RapidWiki.Application.GetArquivos;

public class GetArquivosResult
{
    public List<ArquivoDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
}