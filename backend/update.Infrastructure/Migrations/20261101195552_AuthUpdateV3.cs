using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthUpdateV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_usuarios_id_tipo_perfil_perfiles",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "ix_usuarios_id_tipo_perfil",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "tipo_perfil",
                table: "usuarios");

            migrationBuilder.AddColumn<Guid>(
                name: "usuario_id",
                table: "perfiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_usuario_id",
                table: "perfiles",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "fk_perfiles_usuario_id_usuarios",
                table: "perfiles",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_perfiles_usuario_id_usuarios",
                table: "perfiles");

            migrationBuilder.DropIndex(
                name: "ix_perfiles_usuario_id",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "perfiles");

            migrationBuilder.AddColumn<string>(
                name: "tipo_perfil",
                table: "usuarios",
                type: "text",
                maxLength: 14,
                nullable: false,
                defaultValue: "persona");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_id_tipo_perfil",
                table: "usuarios",
                columns: new[] { "id", "tipo_perfil" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_usuarios_id_tipo_perfil_perfiles",
                table: "usuarios",
                columns: new[] { "id", "tipo_perfil" },
                principalTable: "perfiles",
                principalColumns: new[] { "id", "tipo" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
