using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsuariosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoPerfil = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Correo = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    HashContrasena = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RolPlataforma = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CorreoVerificadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TerminosAceptadosEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoIngresoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IntentosFallidos = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    BloqueadoHasta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContrasenaCambiadaEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
