using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidWiki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeveAlterarSenhaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeveAlterarSenha",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeveAlterarSenha",
                table: "Usuarios");
        }
    }
}
