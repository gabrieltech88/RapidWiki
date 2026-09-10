using RapidWiki.Domain.Enums;

namespace RapidWiki.Domain.Entities;

public class Procedimento
{
    public Guid Id { get; set; }
    required public string Titulo { get; set; }
    required public Guid AutorId { get; set; }
    required public Usuario Autor { get; set; }
    required public string Descricao { get; set; }
    required public ICollection<Departamento> Departamentos { get; set; } = [];
    public DateTime CriadoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }
    required public string Conteudo { get; set; }
    public StatusProcedimento Status { get; set; }

    private Procedimento() { }

    public Procedimento(string titulo, Guid autorId, Usuario autor, StatusProcedimento status, string descricao, ICollection<Departamento> departamentos, string conteudo)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        AutorId = autorId;
        Autor = autor;
        Descricao = descricao;
        Departamentos = departamentos;
        Conteudo = conteudo;

        Status = status;

        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }
}
