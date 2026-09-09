
namespace RapidWiki.Application.Usuario.CreateUsuario
{
    public record CreateUsuarioResult
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
    }

}