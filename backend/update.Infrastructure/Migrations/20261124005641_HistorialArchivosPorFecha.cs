using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HistorialArchivosPorFecha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_archivos_historial_usuario",
                table: "archivos");

            migrationBuilder.CreateIndex(
                name: "ix_archivos_historial_usuario",
                table: "archivos",
                columns: new[] { "subido_por_usuario_id", "creado_en", "id" },
                descending: new[] { false, true, true },
                filter: "eliminado_en IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_archivos_historial_usuario",
                table: "archivos");

            migrationBuilder.CreateIndex(
                name: "ix_archivos_historial_usuario",
                table: "archivos",
                columns: new[] { "subido_por_usuario_id", "id" },
                filter: "eliminado_en IS NULL");
        }
    }
}
