using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidWiki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProcedimentoRascunho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AtualizadoPorId",
                table: "Procedimentos",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcedimentoRascunhoId",
                table: "Departamentos",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ProcedimentoRascunho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProcedimentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Conteudo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedimentoRascunho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedimentoRascunho_Procedimentos_ProcedimentoId",
                        column: x => x.ProcedimentoId,
                        principalTable: "Procedimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedimentoRascunho_Usuarios_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Procedimentos_AtualizadoPorId",
                table: "Procedimentos",
                column: "AtualizadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_ProcedimentoRascunhoId",
                table: "Departamentos",
                column: "ProcedimentoRascunhoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedimentoRascunho_EditorId",
                table: "ProcedimentoRascunho",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedimentoRascunho_ProcedimentoId",
                table: "ProcedimentoRascunho",
                column: "ProcedimentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_ProcedimentoRascunho_ProcedimentoRascunhoId",
                table: "Departamentos",
                column: "ProcedimentoRascunhoId",
                principalTable: "ProcedimentoRascunho",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedimentos_Usuarios_AtualizadoPorId",
                table: "Procedimentos",
                column: "AtualizadoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_ProcedimentoRascunho_ProcedimentoRascunhoId",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Procedimentos_Usuarios_AtualizadoPorId",
                table: "Procedimentos");

            migrationBuilder.DropTable(
                name: "ProcedimentoRascunho");

            migrationBuilder.DropIndex(
                name: "IX_Procedimentos_AtualizadoPorId",
                table: "Procedimentos");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_ProcedimentoRascunhoId",
                table: "Departamentos");

            migrationBuilder.DropColumn(
                name: "AtualizadoPorId",
                table: "Procedimentos");

            migrationBuilder.DropColumn(
                name: "ProcedimentoRascunhoId",
                table: "Departamentos");
        }
    }
}
