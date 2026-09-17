namespace RapidWiki.Domain.Entities;

public class ProcedimentoRascunho
{
    public Guid Id { get; private set; }

    public Guid ProcedimentoId { get; private set; }
    public Procedimento Procedimento { get; private set; } = null!;

    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;

    public ICollection<Departamento> Departamentos { get; private set; } = [];

    public Guid EditorId { get; private set; }
    public Usuario Editor { get; private set; } = null!;

    public DateTime CriadoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private ProcedimentoRascunho()
    {
    }

    public ProcedimentoRascunho(
        Procedimento procedimento,
        string titulo,
        string descricao,
        string conteudo,
        IEnumerable<Departamento> departamentos,
        Guid editorId)
    {
        Id = Guid.NewGuid();

        ProcedimentoId = procedimento.Id;
        Procedimento = procedimento;

        Titulo = titulo;
        Descricao = descricao;
        Conteudo = conteudo;

        Departamentos = departamentos.ToList();

        EditorId = editorId;

        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Atualizar(
        string titulo,
        string descricao,
        string conteudo,
        IEnumerable<Departamento> departamentos,
        Guid editorId)
    {
        Titulo = titulo;
        Descricao = descricao;
        Conteudo = conteudo;

        Departamentos.Clear();

        foreach (var departamento in departamentos)
        {
            Departamentos.Add(departamento);
        }

        EditorId = editorId;

        AtualizadoEm = DateTime.UtcNow;
    }
}