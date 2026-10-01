namespace RapidWiki.Application.Common.Dto;

public class ArquivoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public long TamanhoBytes { get; set; }
    public DateTime CriadoEm { get; set; }
    public List<ArquivoDepartamentoDto> Departamentos { get; set; } = [];
}

public class ArquivoDepartamentoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
}