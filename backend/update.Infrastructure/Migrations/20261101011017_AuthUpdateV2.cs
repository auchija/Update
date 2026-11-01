using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthUpdateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "refresh_tokens");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "refresh_tokens",
                newName: "hash");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "refresh_tokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "refresh_tokens",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "RevocadoEn",
                table: "refresh_tokens",
                newName: "revocado_en");

            migrationBuilder.RenameColumn(
                name: "ExpiraEn",
                table: "refresh_tokens",
                newName: "expira_en");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_usuario_id",
                table: "refresh_tokens",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_usuario_id",
                table: "refresh_tokens");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                newName: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "hash",
                table: "RefreshTokens",
                newName: "Hash");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RefreshTokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "RefreshTokens",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "revocado_en",
                table: "RefreshTokens",
                newName: "RevocadoEn");

            migrationBuilder.RenameColumn(
                name: "expira_en",
                table: "RefreshTokens",
                newName: "ExpiraEn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");
        }
    }
}
