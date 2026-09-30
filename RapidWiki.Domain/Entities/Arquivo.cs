namespace RapidWiki.Domain.Entities;

public class Arquivo
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public long TamanhoBytes { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public ICollection<Departamento> Departamentos { get; private set; } = new List<Departamento>();
    private Arquivo()
    {
        Nome = null!;
        Tipo = null!;
        Departamentos = [];
        CriadoEm = DateTime.UtcNow;
    }
    public Arquivo(string nome, string tipo, long tamanhoBytes)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
        Departamentos = [];
        CriadoEm = DateTime.UtcNow;
        
    }
}