namespace RapidWiki.Domain.Entities
{
    public class Departamento
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public ICollection<Usuario> Usuarios { get; private set; } = [];
        public ICollection<Procedimento> Procedimentos { get; private set; } = [];

        private Departamento() { }

        public Departamento(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;
        }
    }
}