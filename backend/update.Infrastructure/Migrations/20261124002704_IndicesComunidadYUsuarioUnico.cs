using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndicesComunidadYUsuarioUnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_seguimientos_lista_seguidores",
                table: "seguimientos",
                columns: new[] { "perfil_seguido_id", "estado", "creado_en" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_seguimientos_lista_seguidos",
                table: "seguimientos",
                columns: new[] { "usuario_seguidor_id", "estado", "creado_en" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_perfil_pagina",
                table: "publicaciones",
                columns: new[] { "perfil_autor_id", "creado_en", "id" },
                descending: new[] { false, true, true },
                filter: "eliminado_en IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_sugeridos",
                table: "perfiles",
                columns: new[] { "tipo", "estado", "total_publicaciones", "total_seguidores", "id" },
                descending: new[] { false, false, true, true, false },
                filter: "eliminado_en IS NULL");

            migrationBuilder.CreateIndex(
                name: "ux_perfiles_nombre_usuario_activo",
                table: "perfiles",
                column: "nombre_usuario",
                unique: true,
                filter: "eliminado_en IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_hilo",
                table: "comentarios",
                columns: new[] { "publicacion_id", "creado_en", "id" },
                filter: "eliminado_en IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_seguimientos_lista_seguidores",
                table: "seguimientos");

            migrationBuilder.DropIndex(
                name: "ix_seguimientos_lista_seguidos",
                table: "seguimientos");

            migrationBuilder.DropIndex(
                name: "ix_publicaciones_perfil_pagina",
                table: "publicaciones");

            migrationBuilder.DropIndex(
                name: "ix_perfiles_sugeridos",
                table: "perfiles");

            migrationBuilder.DropIndex(
                name: "ux_perfiles_nombre_usuario_activo",
                table: "perfiles");

            migrationBuilder.DropIndex(
                name: "ix_comentarios_hilo",
                table: "comentarios");
        }
    }
}
