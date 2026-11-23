using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <summary>
    /// Índices de las listas paginadas (feed, "Mis publicaciones", "Mis archivos", "Mis emprendimientos")
    /// y categorías base para crear emprendimientos. Reemplaza a los antiguos backend/demo/indices.sql
    /// y categoria.sql: usa IF NOT EXISTS y ON CONFLICT para no fallar en una base donde ya se corrieron.
    /// </summary>
    public partial class IndicesHistorialYCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE INDEX IF NOT EXISTS ix_publicaciones_feed_pagina
                    ON publicaciones (visibilidad, creado_en DESC, id DESC) WHERE eliminado_en IS NULL;
                CREATE INDEX IF NOT EXISTS ix_publicaciones_historial_autor
                    ON publicaciones (creado_por_usuario_id, creado_en DESC, id DESC) WHERE eliminado_en IS NULL;
                CREATE INDEX IF NOT EXISTS ix_emprendimientos_lista_usuario
                    ON emprendimientos (creado_por_usuario_id, id);
                CREATE INDEX IF NOT EXISTS ix_archivos_historial_usuario
                    ON archivos (subido_por_usuario_id, id) WHERE eliminado_en IS NULL;
                """);

            migrationBuilder.Sql("""
                INSERT INTO categorias (slug, nombre, orden, activo) VALUES
                    ('tecnologia', 'Tecnología', 1, true),
                    ('agro-alimentos', 'Agro y alimentos', 2, true),
                    ('comercio', 'Comercio', 3, true),
                    ('servicios', 'Servicios', 4, true),
                    ('moda-diseno', 'Moda y diseño', 5, true),
                    ('salud-bienestar', 'Salud y bienestar', 6, true),
                    ('educacion', 'Educación', 7, true),
                    ('otros', 'Otros', 8, true)
                ON CONFLICT (slug) DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Las categorías no se borran: puede haber emprendimientos que las usan.
            migrationBuilder.Sql("""
                DROP INDEX IF EXISTS ix_publicaciones_feed_pagina;
                DROP INDEX IF EXISTS ix_publicaciones_historial_autor;
                DROP INDEX IF EXISTS ix_emprendimientos_lista_usuario;
                DROP INDEX IF EXISTS ix_archivos_historial_usuario;
                """);
        }
    }
}
