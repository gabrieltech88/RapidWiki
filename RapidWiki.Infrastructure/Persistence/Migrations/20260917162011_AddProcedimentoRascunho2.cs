using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidWiki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProcedimentoRascunho2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_ProcedimentoRascunho_ProcedimentoRascunhoId",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedimentoRascunho_Procedimentos_ProcedimentoId",
                table: "ProcedimentoRascunho");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedimentoRascunho_Usuarios_EditorId",
                table: "ProcedimentoRascunho");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcedimentoRascunho",
                table: "ProcedimentoRascunho");

            migrationBuilder.RenameTable(
                name: "ProcedimentoRascunho",
                newName: "ProcedimentosRascunhos");

            migrationBuilder.RenameIndex(
                name: "IX_ProcedimentoRascunho_ProcedimentoId",
                table: "ProcedimentosRascunhos",
                newName: "IX_ProcedimentosRascunhos_ProcedimentoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcedimentoRascunho_EditorId",
                table: "ProcedimentosRascunhos",
                newName: "IX_ProcedimentosRascunhos_EditorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcedimentosRascunhos",
                table: "ProcedimentosRascunhos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_ProcedimentosRascunhos_ProcedimentoRascunhoId",
                table: "Departamentos",
                column: "ProcedimentoRascunhoId",
                principalTable: "ProcedimentosRascunhos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedimentosRascunhos_Procedimentos_ProcedimentoId",
                table: "ProcedimentosRascunhos",
                column: "ProcedimentoId",
                principalTable: "Procedimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedimentosRascunhos_Usuarios_EditorId",
                table: "ProcedimentosRascunhos",
                column: "EditorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_ProcedimentosRascunhos_ProcedimentoRascunhoId",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedimentosRascunhos_Procedimentos_ProcedimentoId",
                table: "ProcedimentosRascunhos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedimentosRascunhos_Usuarios_EditorId",
                table: "ProcedimentosRascunhos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcedimentosRascunhos",
                table: "ProcedimentosRascunhos");

            migrationBuilder.RenameTable(
                name: "ProcedimentosRascunhos",
                newName: "ProcedimentoRascunho");

            migrationBuilder.RenameIndex(
                name: "IX_ProcedimentosRascunhos_ProcedimentoId",
                table: "ProcedimentoRascunho",
                newName: "IX_ProcedimentoRascunho_ProcedimentoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcedimentosRascunhos_EditorId",
                table: "ProcedimentoRascunho",
                newName: "IX_ProcedimentoRascunho_EditorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcedimentoRascunho",
                table: "ProcedimentoRascunho",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_ProcedimentoRascunho_ProcedimentoRascunhoId",
                table: "Departamentos",
                column: "ProcedimentoRascunhoId",
                principalTable: "ProcedimentoRascunho",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedimentoRascunho_Procedimentos_ProcedimentoId",
                table: "ProcedimentoRascunho",
                column: "ProcedimentoId",
                principalTable: "Procedimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedimentoRascunho_Usuarios_EditorId",
                table: "ProcedimentoRascunho",
                column: "EditorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
