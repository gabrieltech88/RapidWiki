namespace RapidWiki.Domain.Entities;

public class Usuario
{
    public ICollection<Procedimento> Procedimentos { get; private set; } = [];
    public ICollection<Departamento> Departamentos { get; private set; } = [];

    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public bool DeveAlterarSenha { get; private set; }

    private Usuario() { }

    public Usuario(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
        DeveAlterarSenha = true;
    }

    public void Atualizar(string nome, IEnumerable<Departamento> departamentos)
    {
        Nome = nome;

        Departamentos.Clear();

        foreach (var departamento in departamentos)
        {
            Departamentos.Add(departamento);
        }
    }

    public void ConfirmarTrocaDeSenha()
    {
        DeveAlterarSenha = false;
    }

    public void ExigirTrocaDeSenha()
    {
        DeveAlterarSenha = true;
    }
}