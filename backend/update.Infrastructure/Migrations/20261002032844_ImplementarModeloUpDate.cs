using System;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace update.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImplementarModeloUpDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Comprueba la compatibilidad antes de modificar datos existentes.
            migrationBuilder.Sql("""
                DO $validacion$
                BEGIN
                    IF EXISTS (SELECT 1 FROM "Usuarios" WHERE "TipoPerfil" <> 0) THEN
                        RAISE EXCEPTION 'La tabla usuarios del DBML requiere perfiles de persona. Revisa los usuarios con TipoPerfil distinto de 0.';
                    END IF;
                    IF EXISTS (SELECT 1 FROM "Usuarios" WHERE "RolPlataforma" NOT IN (0, 1, 2) OR char_length("Correo") > 254) THEN
                        RAISE EXCEPTION 'Existen roles o longitudes de correo incompatibles con el DBML.';
                    END IF;
                END $validacion$;
                UPDATE "Usuarios" SET "ActualizadoEn" = COALESCE("ActualizadoEn", "CreadoEn");
                """);

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "usuarios");

            migrationBuilder.RenameColumn(
                name: "Correo",
                table: "usuarios",
                newName: "correo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "usuarios",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UltimoIngresoEn",
                table: "usuarios",
                newName: "ultimo_ingreso_en");

            migrationBuilder.RenameColumn(
                name: "TipoPerfil",
                table: "usuarios",
                newName: "tipo_perfil");

            migrationBuilder.RenameColumn(
                name: "TerminosAceptadosEn",
                table: "usuarios",
                newName: "terminos_aceptados_en");

            migrationBuilder.RenameColumn(
                name: "RolPlataforma",
                table: "usuarios",
                newName: "rol_plataforma");

            migrationBuilder.RenameColumn(
                name: "IntentosFallidos",
                table: "usuarios",
                newName: "intentos_fallidos");

            migrationBuilder.RenameColumn(
                name: "HashContrasena",
                table: "usuarios",
                newName: "hash_contrasena");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "usuarios",
                newName: "creado_en");

            migrationBuilder.RenameColumn(
                name: "CorreoVerificadoEn",
                table: "usuarios",
                newName: "correo_verificado_en");

            migrationBuilder.RenameColumn(
                name: "ContrasenaCambiadaEn",
                table: "usuarios",
                newName: "contrasena_cambiada_en");

            migrationBuilder.RenameColumn(
                name: "BloqueadoHasta",
                table: "usuarios",
                newName: "bloqueado_hasta");

            migrationBuilder.RenameColumn(
                name: "ActualizadoEn",
                table: "usuarios",
                newName: "actualizado_en");

            migrationBuilder.AlterColumn<string>(
                name: "correo",
                table: "usuarios",
                type: "text",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ultimo_ingreso_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            // La conversión numérica conserva el significado de cada enum.
            migrationBuilder.Sql("""
                ALTER TABLE usuarios ALTER COLUMN tipo_perfil DROP DEFAULT;
                ALTER TABLE usuarios ALTER COLUMN tipo_perfil TYPE text USING (CASE tipo_perfil WHEN 0 THEN 'persona' END);
                ALTER TABLE usuarios ALTER COLUMN tipo_perfil SET DEFAULT 'persona';
                """);

            migrationBuilder.AlterColumn<DateTime>(
                name: "terminos_aceptados_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            // La conversión numérica conserva el significado de cada enum.
            migrationBuilder.Sql("""
                ALTER TABLE usuarios ALTER COLUMN rol_plataforma DROP DEFAULT;
                ALTER TABLE usuarios ALTER COLUMN rol_plataforma TYPE text USING (CASE rol_plataforma WHEN 0 THEN 'usuario' WHEN 1 THEN 'moderador' WHEN 2 THEN 'administrador' END);
                ALTER TABLE usuarios ALTER COLUMN rol_plataforma SET DEFAULT 'usuario';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "hash_contrasena",
                table: "usuarios",
                type: "text",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "creado_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "correo_verificado_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "contrasena_cambiada_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "bloqueado_hasta",
                table: "usuarios",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "actualizado_en",
                table: "usuarios",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios",
                column: "id");

            migrationBuilder.CreateTable(
                name: "archivos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    subido_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo = table.Column<string>(type: "text", maxLength: 9, nullable: false),
                    proposito = table.Column<string>(type: "text", maxLength: 12, nullable: false),
                    proveedor_almacenamiento = table.Column<string>(type: "text", maxLength: 255, nullable: false, defaultValue: "r2"),
                    bucket = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    clave_objeto = table.Column<string>(type: "text", maxLength: 1024, nullable: false),
                    nombre_original = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    tipo_mime = table.Column<string>(type: "text", maxLength: 127, nullable: false),
                    extension = table.Column<string>(type: "text", maxLength: 16, nullable: false),
                    tamano_bytes = table.Column<long>(type: "bigint", nullable: false),
                    checksum_sha256 = table.Column<string>(type: "character(64)", maxLength: 64, nullable: true),
                    ancho = table.Column<int>(type: "integer", nullable: true),
                    alto = table.Column<int>(type: "integer", nullable: true),
                    duracion_segundos = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    visibilidad = table.Column<string>(type: "text", maxLength: 7, nullable: false, defaultValue: "publico"),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "pendiente"),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_archivos", x => x.id);
                    table.CheckConstraint("ck_archivos_bucket_longitud", "char_length(\"bucket\") <= 255");
                    table.CheckConstraint("ck_archivos_clave_objeto_longitud", "char_length(\"clave_objeto\") <= 1024");
                    table.CheckConstraint("ck_archivos_estado_enum", "\"estado\" IN ('pendiente', 'listo', 'fallido', 'eliminado')");
                    table.CheckConstraint("ck_archivos_extension_longitud", "char_length(\"extension\") <= 16");
                    table.CheckConstraint("ck_archivos_nombre_original_longitud", "char_length(\"nombre_original\") <= 255");
                    table.CheckConstraint("ck_archivos_proposito_enum", "\"proposito\" IN ('foto_perfil', 'portada', 'publicacion', 'producto', 'mensaje', 'resena', 'verificacion', 'otro')");
                    table.CheckConstraint("ck_archivos_proveedor_almacenamiento_longitud", "char_length(\"proveedor_almacenamiento\") <= 255");
                    table.CheckConstraint("ck_archivos_tipo_enum", "\"tipo\" IN ('imagen', 'video', 'documento')");
                    table.CheckConstraint("ck_archivos_tipo_mime_longitud", "char_length(\"tipo_mime\") <= 127");
                    table.CheckConstraint("ck_archivos_visibilidad_enum", "\"visibilidad\" IN ('publico', 'privado')");
                    table.ForeignKey(
                        name: "fk_archivos_subido_por_usuario_id_usuarios",
                        column: x => x.subido_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    padre_id = table.Column<int>(type: "integer", nullable: true),
                    slug = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    icono = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorias", x => x.id);
                    table.CheckConstraint("ck_categorias_icono_longitud", "char_length(\"icono\") <= 255");
                    table.CheckConstraint("ck_categorias_nombre_longitud", "char_length(\"nombre\") <= 255");
                    table.CheckConstraint("ck_categorias_slug_longitud", "char_length(\"slug\") <= 255");
                    table.ForeignKey(
                        name: "fk_categorias_padre_id_categorias",
                        column: x => x.padre_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "etiquetas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    total_usos = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_etiquetas", x => x.id);
                    table.CheckConstraint("ck_etiquetas_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "monedas",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "character(3)", maxLength: 3, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    simbolo = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    decimales = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_monedas", x => x.codigo);
                    table.CheckConstraint("ck_monedas_nombre_longitud", "char_length(\"nombre\") <= 255");
                    table.CheckConstraint("ck_monedas_simbolo_longitud", "char_length(\"simbolo\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "motivos_reporte",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motivos_reporte", x => x.codigo);
                    table.CheckConstraint("ck_motivos_reporte_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_motivos_reporte_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "permisos_rol",
                columns: table => new
                {
                    rol = table.Column<string>(type: "text", maxLength: 13, nullable: false),
                    permiso = table.Column<string>(type: "text", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permisos_rol", x => new { x.rol, x.permiso });
                    table.CheckConstraint("ck_permisos_rol_permiso_longitud", "char_length(\"permiso\") <= 64");
                    table.CheckConstraint("ck_permisos_rol_rol_enum", "\"rol\" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro')");
                });

            migrationBuilder.CreateTable(
                name: "sesiones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hash_token_refresco = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    familia_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reemplazado_por_id = table.Column<Guid>(type: "uuid", nullable: true),
                    agente_usuario = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    direccion_ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    ultimo_uso_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    expira_en = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    revocado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    motivo_revocacion = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sesiones", x => x.id);
                    table.CheckConstraint("ck_sesiones_agente_usuario_longitud", "char_length(\"agente_usuario\") <= 255");
                    table.CheckConstraint("ck_sesiones_hash_token_refresco_longitud", "char_length(\"hash_token_refresco\") <= 256");
                    table.CheckConstraint("ck_sesiones_motivo_revocacion_longitud", "char_length(\"motivo_revocacion\") <= 255");
                    table.ForeignKey(
                        name: "fk_sesiones_reemplazado_por_id_sesiones",
                        column: x => x.reemplazado_por_id,
                        principalTable: "sesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sesiones_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tipos_oportunidad",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipos_oportunidad", x => x.codigo);
                    table.CheckConstraint("ck_tipos_oportunidad_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_tipos_oportunidad_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_tipos_oportunidad_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "tipos_persona",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipos_persona", x => x.codigo);
                    table.CheckConstraint("ck_tipos_persona_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_tipos_persona_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_tipos_persona_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "tipos_publicacion",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    icono = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    peso_descubrimiento = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false, defaultValue: 1.00m),
                    permite_respuesta_aceptada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipos_publicacion", x => x.codigo);
                    table.CheckConstraint("ck_tipos_publicacion_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_tipos_publicacion_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_tipos_publicacion_icono_longitud", "char_length(\"icono\") <= 255");
                    table.CheckConstraint("ck_tipos_publicacion_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "tipos_reaccion",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    emoji = table.Column<string>(type: "text", maxLength: 32, nullable: false),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipos_reaccion", x => x.codigo);
                    table.CheckConstraint("ck_tipos_reaccion_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_tipos_reaccion_emoji_longitud", "char_length(\"emoji\") <= 32");
                    table.CheckConstraint("ck_tipos_reaccion_nombre_longitud", "char_length(\"nombre\") <= 255");
                });

            migrationBuilder.CreateTable(
                name: "tokens_recuperacion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hash_token = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    ip_solicitud = table.Column<IPAddress>(type: "inet", nullable: true),
                    expira_en = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    usado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens_recuperacion", x => x.id);
                    table.CheckConstraint("ck_tokens_recuperacion_hash_token_longitud", "char_length(\"hash_token\") <= 256");
                    table.ForeignKey(
                        name: "fk_tokens_recuperacion_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_reservados",
                columns: table => new
                {
                    nombre_usuario = table.Column<string>(type: "text", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios_reservados", x => x.nombre_usuario);
                    table.CheckConstraint("ck_usuarios_reservados_nombre_usuario_longitud", "char_length(\"nombre_usuario\") <= 50");
                });

            migrationBuilder.CreateTable(
                name: "variantes_archivo",
                columns: table => new
                {
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variante = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    clave_objeto = table.Column<string>(type: "text", maxLength: 1024, nullable: false),
                    tipo_mime = table.Column<string>(type: "text", maxLength: 127, nullable: false),
                    ancho = table.Column<int>(type: "integer", nullable: true),
                    alto = table.Column<int>(type: "integer", nullable: true),
                    tamano_bytes = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variantes_archivo", x => new { x.archivo_id, x.variante });
                    table.CheckConstraint("ck_variantes_archivo_clave_objeto_longitud", "char_length(\"clave_objeto\") <= 1024");
                    table.CheckConstraint("ck_variantes_archivo_tipo_mime_longitud", "char_length(\"tipo_mime\") <= 127");
                    table.CheckConstraint("ck_variantes_archivo_variante_longitud", "char_length(\"variante\") <= 64");
                    table.ForeignKey(
                        name: "fk_variantes_archivo_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "intereses_usuario",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoria_id = table.Column<int>(type: "integer", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intereses_usuario", x => new { x.usuario_id, x.categoria_id });
                    table.ForeignKey(
                        name: "fk_intereses_usuario_categoria_id_categorias",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_intereses_usuario_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ofertas_mentoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_mentor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    categoria_id = table.Column<int>(type: "integer", nullable: true),
                    modalidad = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "virtual"),
                    duracion_minutos = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)60),
                    es_gratis = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    precio = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: true),
                    moneda_codigo = table.Column<string>(type: "character(3)", maxLength: 3, nullable: false, defaultValue: "COP"),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ofertas_mentoria", x => x.id);
                    table.CheckConstraint("ck_ofertas_mentoria_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_ofertas_mentoria_modalidad_enum", "\"modalidad\" IN ('virtual', 'presencial', 'ambas')");
                    table.CheckConstraint("ck_ofertas_mentoria_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_ofertas_mentoria_categoria_id_categorias",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ofertas_mentoria_moneda_codigo_monedas",
                        column: x => x.moneda_codigo,
                        principalTable: "monedas",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ofertas_mentoria_usuario_mentor_id_usuarios",
                        column: x => x.usuario_mentor_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "paises",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "character(2)", maxLength: 2, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    moneda_defecto_codigo = table.Column<string>(type: "character(3)", maxLength: 3, nullable: false),
                    prefijo_telefono = table.Column<string>(type: "text", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paises", x => x.codigo);
                    table.CheckConstraint("ck_paises_nombre_longitud", "char_length(\"nombre\") <= 255");
                    table.CheckConstraint("ck_paises_prefijo_telefono_longitud", "char_length(\"prefijo_telefono\") <= 30");
                    table.ForeignKey(
                        name: "fk_paises_moneda_defecto_codigo_monedas",
                        column: x => x.moneda_defecto_codigo,
                        principalTable: "monedas",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reportes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_reportante_id = table.Column<Guid>(type: "uuid", nullable: false),
                    objetivo_tipo = table.Column<string>(type: "text", maxLength: 11, nullable: false),
                    objetivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    motivo_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    detalles = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 11, nullable: false, defaultValue: "abierto"),
                    resuelto_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    nota_resolucion = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    resuelto_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reportes", x => x.id);
                    table.CheckConstraint("ck_reportes_detalles_longitud", "char_length(\"detalles\") <= 4000");
                    table.CheckConstraint("ck_reportes_estado_enum", "\"estado\" IN ('abierto', 'en_revision', 'resuelto', 'descartado')");
                    table.CheckConstraint("ck_reportes_motivo_codigo_longitud", "char_length(\"motivo_codigo\") <= 64");
                    table.CheckConstraint("ck_reportes_nota_resolucion_longitud", "char_length(\"nota_resolucion\") <= 4000");
                    table.CheckConstraint("ck_reportes_objetivo_tipo_enum", "\"objetivo_tipo\" IN ('perfil', 'publicacion', 'comentario', 'producto', 'mensaje', 'resena', 'oportunidad')");
                    table.ForeignKey(
                        name: "fk_reportes_motivo_codigo_motivos_reporte",
                        column: x => x.motivo_codigo,
                        principalTable: "motivos_reporte",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_reportes_resuelto_por_usuario_id_usuarios",
                        column: x => x.resuelto_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_reportes_usuario_reportante_id_usuarios",
                        column: x => x.usuario_reportante_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario_tipos_persona",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_persona_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario_tipos_persona", x => new { x.usuario_id, x.tipo_persona_codigo });
                    table.CheckConstraint("ck_usuario_tipos_persona_tipo_persona_codigo_longitud", "char_length(\"tipo_persona_codigo\") <= 64");
                    table.ForeignKey(
                        name: "fk_usuario_tipos_persona_tipo_persona_codigo_tipos_persona",
                        column: x => x.tipo_persona_codigo,
                        principalTable: "tipos_persona",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_usuario_tipos_persona_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "departamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    pais_codigo = table.Column<string>(type: "character(2)", maxLength: 2, nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departamentos", x => x.id);
                    table.CheckConstraint("ck_departamentos_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_departamentos_nombre_longitud", "char_length(\"nombre\") <= 255");
                    table.ForeignKey(
                        name: "fk_departamentos_pais_codigo_paises",
                        column: x => x.pais_codigo,
                        principalTable: "paises",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ciudades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    departamento_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    codigo = table.Column<string>(type: "text", maxLength: 64, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    longitud = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ciudades", x => x.id);
                    table.CheckConstraint("ck_ciudades_codigo_longitud", "char_length(\"codigo\") <= 64");
                    table.CheckConstraint("ck_ciudades_nombre_longitud", "char_length(\"nombre\") <= 255");
                    table.ForeignKey(
                        name: "fk_ciudades_departamento_id_departamentos",
                        column: x => x.departamento_id,
                        principalTable: "departamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "perfiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tipo = table.Column<string>(type: "text", maxLength: 14, nullable: false),
                    nombre_usuario = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    nombre_visible = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    titular = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    biografia = table.Column<string>(type: "text", maxLength: 2000, nullable: true),
                    foto_archivo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    portada_archivo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ciudad_id = table.Column<int>(type: "integer", nullable: true),
                    sitio_web = table.Column<string>(type: "text", maxLength: 2048, nullable: true),
                    es_privado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "activo"),
                    total_seguidores = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_seguidos = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_publicaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_perfiles", x => x.id);
                    table.UniqueConstraint("AK_perfiles_id_tipo", x => new { x.id, x.tipo });
                    table.CheckConstraint("ck_perfiles_biografia_longitud", "char_length(\"biografia\") <= 2000");
                    table.CheckConstraint("ck_perfiles_estado_enum", "\"estado\" IN ('activo', 'suspendido', 'eliminado')");
                    table.CheckConstraint("ck_perfiles_nombre_usuario_longitud", "char_length(\"nombre_usuario\") <= 50");
                    table.CheckConstraint("ck_perfiles_nombre_visible_longitud", "char_length(\"nombre_visible\") <= 255");
                    table.CheckConstraint("ck_perfiles_sitio_web_longitud", "char_length(\"sitio_web\") <= 2048");
                    table.CheckConstraint("ck_perfiles_tipo_enum", "\"tipo\" IN ('persona', 'emprendimiento')");
                    table.CheckConstraint("ck_perfiles_titular_longitud", "char_length(\"titular\") <= 255");
                    table.ForeignKey(
                        name: "fk_perfiles_ciudad_id_ciudades",
                        column: x => x.ciudad_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_perfiles_foto_archivo_id_archivos",
                        column: x => x.foto_archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_perfiles_portada_archivo_id_archivos",
                        column: x => x.portada_archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bloqueos",
                columns: table => new
                {
                    usuario_bloqueador_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_bloqueado_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bloqueos", x => new { x.usuario_bloqueador_id, x.perfil_bloqueado_id });
                    table.ForeignKey(
                        name: "fk_bloqueos_perfil_bloqueado_id_perfiles",
                        column: x => x.perfil_bloqueado_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bloqueos_usuario_bloqueador_id_usuarios",
                        column: x => x.usuario_bloqueador_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "emprendimientos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_perfil = table.Column<string>(type: "text", maxLength: 14, nullable: false, defaultValue: "emprendimiento"),
                    categoria_id = table.Column<int>(type: "integer", nullable: false),
                    etapa = table.Column<string>(type: "text", maxLength: 11, nullable: false, defaultValue: "idea"),
                    fundado_en = table.Column<DateOnly>(type: "date", nullable: true),
                    razon_social = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    nit = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    correo_contacto = table.Column<string>(type: "text", maxLength: 254, nullable: true),
                    telefono_contacto = table.Column<string>(type: "text", maxLength: 30, nullable: true),
                    whatsapp = table.Column<string>(type: "text", maxLength: 30, nullable: true),
                    direccion = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    envios_nacionales = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ofrece_remoto = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado_verificacion = table.Column<string>(type: "text", maxLength: 13, nullable: false, defaultValue: "sin_verificar"),
                    verificado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    calificacion_promedio = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    total_calificaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emprendimientos", x => x.id);
                    table.CheckConstraint("ck_emprendimientos_correo_contacto_longitud", "char_length(\"correo_contacto\") <= 254");
                    table.CheckConstraint("ck_emprendimientos_direccion_longitud", "char_length(\"direccion\") <= 500");
                    table.CheckConstraint("ck_emprendimientos_estado_verificacion_enum", "\"estado_verificacion\" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')");
                    table.CheckConstraint("ck_emprendimientos_etapa_enum", "\"etapa\" IN ('idea', 'validacion', 'lanzamiento', 'crecimiento', 'consolidado')");
                    table.CheckConstraint("ck_emprendimientos_nit_longitud", "char_length(\"nit\") <= 255");
                    table.CheckConstraint("ck_emprendimientos_razon_social_longitud", "char_length(\"razon_social\") <= 255");
                    table.CheckConstraint("ck_emprendimientos_telefono_contacto_longitud", "char_length(\"telefono_contacto\") <= 30");
                    table.CheckConstraint("ck_emprendimientos_tipo_perfil", "\"tipo_perfil\" = 'emprendimiento'");
                    table.CheckConstraint("ck_emprendimientos_tipo_perfil_enum", "\"tipo_perfil\" IN ('persona', 'emprendimiento')");
                    table.CheckConstraint("ck_emprendimientos_whatsapp_longitud", "char_length(\"whatsapp\") <= 30");
                    table.ForeignKey(
                        name: "fk_emprendimientos_categoria_id_categorias",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_emprendimientos_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_emprendimientos_id_tipo_perfil_perfiles",
                        columns: x => new { x.id, x.tipo_perfil },
                        principalTable: "perfiles",
                        principalColumns: new[] { "id", "tipo" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notificaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_destinatario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo = table.Column<string>(type: "text", maxLength: 25, nullable: false),
                    perfil_actor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entidad_tipo = table.Column<string>(type: "text", maxLength: 64, nullable: true),
                    entidad_id = table.Column<Guid>(type: "uuid", nullable: true),
                    clave_grupo = table.Column<string>(type: "text", maxLength: 1024, nullable: true),
                    datos = table.Column<JsonElement>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    leido_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notificaciones", x => x.id);
                    table.CheckConstraint("ck_notificaciones_clave_grupo_longitud", "char_length(\"clave_grupo\") <= 1024");
                    table.CheckConstraint("ck_notificaciones_entidad_tipo_longitud", "char_length(\"entidad_tipo\") <= 64");
                    table.CheckConstraint("ck_notificaciones_tipo_enum", "\"tipo\" IN ('seguimiento', 'solicitud_seguimiento', 'seguimiento_aceptado', 'reaccion', 'comentario', 'respuesta', 'mencion', 'compartido', 'mensaje', 'cotizacion_actualizada', 'postulacion_actualizada', 'mentoria_actualizada', 'resena', 'invitacion_emprendimiento', 'respuesta_aceptada', 'sistema')");
                    table.ForeignKey(
                        name: "fk_notificaciones_perfil_actor_id_perfiles",
                        column: x => x.perfil_actor_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notificaciones_usuario_destinatario_id_usuarios",
                        column: x => x.usuario_destinatario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "oportunidades",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    perfil_autor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    categoria_id = table.Column<int>(type: "integer", nullable: true),
                    ciudad_id = table.Column<int>(type: "integer", nullable: true),
                    es_remoto = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado = table.Column<string>(type: "text", maxLength: 8, nullable: false, defaultValue: "abierto"),
                    expira_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    total_postulaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_oportunidades", x => x.id);
                    table.CheckConstraint("ck_oportunidades_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_oportunidades_estado_enum", "\"estado\" IN ('abierto', 'cerrado', 'cubierto')");
                    table.CheckConstraint("ck_oportunidades_tipo_codigo_longitud", "char_length(\"tipo_codigo\") <= 64");
                    table.CheckConstraint("ck_oportunidades_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_oportunidades_categoria_id_categorias",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_oportunidades_ciudad_id_ciudades",
                        column: x => x.ciudad_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_oportunidades_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_oportunidades_perfil_autor_id_perfiles",
                        column: x => x.perfil_autor_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_oportunidades_tipo_codigo_tipos_oportunidad",
                        column: x => x.tipo_codigo,
                        principalTable: "tipos_oportunidad",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seguimientos",
                columns: table => new
                {
                    usuario_seguidor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_seguido_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "activo"),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    aceptado_en = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seguimientos", x => new { x.usuario_seguidor_id, x.perfil_seguido_id });
                    table.CheckConstraint("ck_seguimientos_estado_enum", "\"estado\" IN ('pendiente', 'activo')");
                    table.ForeignKey(
                        name: "fk_seguimientos_perfil_seguido_id_perfiles",
                        column: x => x.perfil_seguido_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_seguimientos_usuario_seguidor_id_usuarios",
                        column: x => x.usuario_seguidor_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "solicitudes_mentoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    oferta_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_mentoreado_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mensaje = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    estado = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "solicitado"),
                    agendado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    url_reunion = table.Column<string>(type: "text", maxLength: 2048, nullable: true),
                    calificacion_mentoreado = table.Column<short>(type: "smallint", nullable: true),
                    comentario_mentoreado = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solicitudes_mentoria", x => x.id);
                    table.CheckConstraint("ck_solicitudes_mentoria_calificacion_mentoreado", "\"calificacion_mentoreado\" BETWEEN 1 AND 5");
                    table.CheckConstraint("ck_solicitudes_mentoria_comentario_mentoreado_longitud", "char_length(\"comentario_mentoreado\") <= 4000");
                    table.CheckConstraint("ck_solicitudes_mentoria_estado_enum", "\"estado\" IN ('solicitado', 'aceptado', 'rechazado', 'agendado', 'completado', 'cancelado')");
                    table.CheckConstraint("ck_solicitudes_mentoria_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
                    table.CheckConstraint("ck_solicitudes_mentoria_url_reunion_longitud", "char_length(\"url_reunion\") <= 2048");
                    table.ForeignKey(
                        name: "fk_solicitudes_mentoria_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_mentoria_oferta_id_ofertas_mentoria",
                        column: x => x.oferta_id,
                        principalTable: "ofertas_mentoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_mentoria_perfil_mentoreado_id_perfiles",
                        column: x => x.perfil_mentoreado_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "enlaces_emprendimiento",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    plataforma = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    url = table.Column<string>(type: "text", maxLength: 2048, nullable: false),
                    orden = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enlaces_emprendimiento", x => x.id);
                    table.CheckConstraint("ck_enlaces_emprendimiento_plataforma_longitud", "char_length(\"plataforma\") <= 64");
                    table.CheckConstraint("ck_enlaces_emprendimiento_url_longitud", "char_length(\"url\") <= 2048");
                    table.ForeignKey(
                        name: "fk_enlaces_emprendimiento_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "miembros_emprendimiento",
                columns: table => new
                {
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol = table.Column<string>(type: "text", maxLength: 13, nullable: false, defaultValue: "miembro"),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "invitado"),
                    invitado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    unido_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_miembros_emprendimiento", x => new { x.emprendimiento_id, x.usuario_id });
                    table.CheckConstraint("ck_miembros_emprendimiento_estado_enum", "\"estado\" IN ('invitado', 'activo', 'retirado', 'expulsado')");
                    table.CheckConstraint("ck_miembros_emprendimiento_rol_enum", "\"rol\" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro')");
                    table.CheckConstraint("ck_miembros_emprendimiento_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_miembros_emprendimiento_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_miembros_emprendimiento_invitado_por_usuario_id_usuarios",
                        column: x => x.invitado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_miembros_emprendimiento_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo = table.Column<string>(type: "text", maxLength: 8, nullable: false),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    categoria_id = table.Column<int>(type: "integer", nullable: false),
                    tipo_precio = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "fijo"),
                    precio = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: true),
                    moneda_codigo = table.Column<string>(type: "character(3)", maxLength: 3, nullable: false, defaultValue: "COP"),
                    estado_inventario = table.Column<string>(type: "text", maxLength: 11, nullable: false, defaultValue: "disponible"),
                    cantidad_inventario = table.Column<int>(type: "integer", nullable: true),
                    ciudad_id = table.Column<int>(type: "integer", nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "borrador"),
                    calificacion_promedio = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    total_calificaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_guardados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    publicado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_productos", x => x.id);
                    table.UniqueConstraint("AK_productos_id_emprendimiento_id", x => new { x.id, x.emprendimiento_id });
                    table.CheckConstraint("ck_productos_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_productos_estado_enum", "\"estado\" IN ('borrador', 'activo', 'pausado', 'archivado')");
                    table.CheckConstraint("ck_productos_estado_inventario_enum", "\"estado_inventario\" IN ('disponible', 'agotado', 'bajo_pedido', 'no_aplica')");
                    table.CheckConstraint("ck_productos_slug_longitud", "char_length(\"slug\") <= 255");
                    table.CheckConstraint("ck_productos_tipo_enum", "\"tipo\" IN ('producto', 'servicio')");
                    table.CheckConstraint("ck_productos_tipo_precio_enum", "\"tipo_precio\" IN ('fijo', 'desde', 'a_convenir', 'gratis')");
                    table.CheckConstraint("ck_productos_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_productos_categoria_id_categorias",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_ciudad_id_ciudades",
                        column: x => x.ciudad_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_moneda_codigo_monedas",
                        column: x => x.moneda_codigo,
                        principalTable: "monedas",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "verificaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estado = table.Column<string>(type: "text", maxLength: 13, nullable: false, defaultValue: "pendiente"),
                    metodo = table.Column<string>(type: "text", maxLength: 64, nullable: false, defaultValue: "documentos"),
                    enviado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    revisado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    notas_revisor = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    enviado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    revisado_en = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_verificaciones", x => x.id);
                    table.CheckConstraint("ck_verificaciones_estado_enum", "\"estado\" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')");
                    table.CheckConstraint("ck_verificaciones_metodo_longitud", "char_length(\"metodo\") <= 64");
                    table.CheckConstraint("ck_verificaciones_notas_revisor_longitud", "char_length(\"notas_revisor\") <= 4000");
                    table.ForeignKey(
                        name: "fk_verificaciones_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_verificaciones_enviado_por_usuario_id_usuarios",
                        column: x => x.enviado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_verificaciones_revisado_por_usuario_id_usuarios",
                        column: x => x.revisado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "postulaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    oportunidad_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_postulante_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mensaje = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "pendiente"),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_postulaciones", x => x.id);
                    table.CheckConstraint("ck_postulaciones_estado_enum", "\"estado\" IN ('pendiente', 'aceptado', 'rechazado', 'retirado')");
                    table.CheckConstraint("ck_postulaciones_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
                    table.ForeignKey(
                        name: "fk_postulaciones_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_postulaciones_oportunidad_id_oportunidades",
                        column: x => x.oportunidad_id,
                        principalTable: "oportunidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_postulaciones_perfil_postulante_id_perfiles",
                        column: x => x.perfil_postulante_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productos_archivos",
                columns: table => new
                {
                    producto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    posicion = table.Column<short>(type: "smallint", nullable: false),
                    texto_alternativo = table.Column<string>(type: "text", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_productos_archivos", x => new { x.producto_id, x.archivo_id });
                    table.CheckConstraint("ck_productos_archivos_texto_alternativo_longitud", "char_length(\"texto_alternativo\") <= 255");
                    table.ForeignKey(
                        name: "fk_productos_archivos_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_archivos_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productos_etiquetas",
                columns: table => new
                {
                    producto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    etiqueta_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_productos_etiquetas", x => new { x.producto_id, x.etiqueta_id });
                    table.ForeignKey(
                        name: "fk_productos_etiquetas_etiqueta_id_etiquetas",
                        column: x => x.etiqueta_id,
                        principalTable: "etiquetas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_productos_etiquetas_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documentos_verificacion",
                columns: table => new
                {
                    verificacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_documento = table.Column<string>(type: "text", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documentos_verificacion", x => new { x.verificacion_id, x.archivo_id });
                    table.CheckConstraint("ck_documentos_verificacion_tipo_documento_longitud", "char_length(\"tipo_documento\") <= 64");
                    table.ForeignKey(
                        name: "fk_documentos_verificacion_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_documentos_verificacion_verificacion_id_verificaciones",
                        column: x => x.verificacion_id,
                        principalTable: "verificaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comentarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comentario_padre_id = table.Column<Guid>(type: "uuid", nullable: true),
                    perfil_autor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    responde_a_perfil_id = table.Column<Guid>(type: "uuid", nullable: true),
                    contenido = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    total_reacciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_respuestas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    editado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comentarios", x => x.id);
                    table.UniqueConstraint("AK_comentarios_id_publicacion_id", x => new { x.id, x.publicacion_id });
                    table.CheckConstraint("ck_comentarios_contenido_longitud", "char_length(\"contenido\") <= 10000");
                    table.ForeignKey(
                        name: "fk_comentarios_comentario_padre_id_publicacion_id_comentarios",
                        columns: x => new { x.comentario_padre_id, x.publicacion_id },
                        principalTable: "comentarios",
                        principalColumns: new[] { "id", "publicacion_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_comentarios_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_comentarios_perfil_autor_id_perfiles",
                        column: x => x.perfil_autor_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_comentarios_responde_a_perfil_id_perfiles",
                        column: x => x.responde_a_perfil_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    perfil_autor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_publicacion_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false, defaultValue: "general"),
                    contenido = table.Column<string>(type: "text", maxLength: 10000, nullable: false, defaultValue: ""),
                    visibilidad = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "publico"),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    oportunidad_id = table.Column<Guid>(type: "uuid", nullable: true),
                    publicacion_compartida_id = table.Column<Guid>(type: "uuid", nullable: true),
                    comentario_aceptado_id = table.Column<Guid>(type: "uuid", nullable: true),
                    comentarios_habilitados = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    fijado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    total_reacciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_comentarios = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_compartidos = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_guardados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    puntaje_descubrimiento = table.Column<double>(type: "float8", nullable: false, defaultValue: 0.0),
                    puntaje_actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    editado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_publicaciones", x => x.id);
                    table.CheckConstraint("ck_publicaciones_contenido_longitud", "char_length(\"contenido\") <= 10000");
                    table.CheckConstraint("ck_publicaciones_tipo_publicacion_codigo_longitud", "char_length(\"tipo_publicacion_codigo\") <= 64");
                    table.CheckConstraint("ck_publicaciones_visibilidad_enum", "\"visibilidad\" IN ('publico', 'seguidores')");
                    table.ForeignKey(
                        name: "fk_publicaciones_comentario_aceptado_id_id_comentarios",
                        columns: x => new { x.comentario_aceptado_id, x.id },
                        principalTable: "comentarios",
                        principalColumns: new[] { "id", "publicacion_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_oportunidad_id_oportunidades",
                        column: x => x.oportunidad_id,
                        principalTable: "oportunidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_perfil_autor_id_perfiles",
                        column: x => x.perfil_autor_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_publicacion_compartida_id_publicaciones",
                        column: x => x.publicacion_compartida_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_tipo_publicacion_codigo_tipos_publicacion",
                        column: x => x.tipo_publicacion_codigo,
                        principalTable: "tipos_publicacion",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reacciones_comentario",
                columns: table => new
                {
                    comentario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_reaccion_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reacciones_comentario", x => new { x.comentario_id, x.perfil_id });
                    table.CheckConstraint("ck_reacciones_comentario_tipo_reaccion_codigo_longitud", "char_length(\"tipo_reaccion_codigo\") <= 64");
                    table.ForeignKey(
                        name: "fk_reacciones_comentario_comentario_id_comentarios",
                        column: x => x.comentario_id,
                        principalTable: "comentarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reacciones_comentario_perfil_id_perfiles",
                        column: x => x.perfil_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_reacciones_comentario_tipo_reaccion_codigo_tipos_reaccion",
                        column: x => x.tipo_reaccion_codigo,
                        principalTable: "tipos_reaccion",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "guardados",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guardados", x => x.id);
                    table.CheckConstraint("ck_guardados_un_objetivo", "(\"publicacion_id\" IS NOT NULL) <> (\"producto_id\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_guardados_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_guardados_publicacion_id_publicaciones",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_guardados_usuario_id_usuarios",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "menciones",
                columns: table => new
                {
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menciones", x => new { x.publicacion_id, x.perfil_id });
                    table.ForeignKey(
                        name: "fk_menciones_perfil_id_perfiles",
                        column: x => x.perfil_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_menciones_publicacion_id_publicaciones",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicaciones_archivos",
                columns: table => new
                {
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    posicion = table.Column<short>(type: "smallint", nullable: false),
                    texto_alternativo = table.Column<string>(type: "text", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_publicaciones_archivos", x => new { x.publicacion_id, x.archivo_id });
                    table.CheckConstraint("ck_publicaciones_archivos_texto_alternativo_longitud", "char_length(\"texto_alternativo\") <= 255");
                    table.ForeignKey(
                        name: "fk_publicaciones_archivos_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_archivos_publicacion_id_publicaciones",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicaciones_etiquetas",
                columns: table => new
                {
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    etiqueta_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_publicaciones_etiquetas", x => new { x.publicacion_id, x.etiqueta_id });
                    table.ForeignKey(
                        name: "fk_publicaciones_etiquetas_etiqueta_id_etiquetas",
                        column: x => x.etiqueta_id,
                        principalTable: "etiquetas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_publicaciones_etiquetas_publicacion_id_publicaciones",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reacciones_publicacion",
                columns: table => new
                {
                    publicacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_reaccion_codigo = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reacciones_publicacion", x => new { x.publicacion_id, x.perfil_id });
                    table.CheckConstraint("ck_reacciones_publicacion_tipo_reaccion_codigo_longitud", "char_length(\"tipo_reaccion_codigo\") <= 64");
                    table.ForeignKey(
                        name: "fk_reacciones_publicacion_perfil_id_perfiles",
                        column: x => x.perfil_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_reacciones_publicacion_publicacion_id_publicaciones",
                        column: x => x.publicacion_id,
                        principalTable: "publicaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reacciones_publicacion_tipo_reaccion_codigo_tipos_reaccion",
                        column: x => x.tipo_reaccion_codigo,
                        principalTable: "tipos_reaccion",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "conversaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tipo = table.Column<string>(type: "text", maxLength: 7, nullable: false, defaultValue: "directa"),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    clave_directa = table.Column<string>(type: "text", maxLength: 1024, nullable: true),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ultimo_mensaje_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ultimo_mensaje_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversaciones", x => x.id);
                    table.CheckConstraint("ck_conversaciones_clave_directa_longitud", "char_length(\"clave_directa\") <= 1024");
                    table.CheckConstraint("ck_conversaciones_tipo_enum", "\"tipo\" IN ('directa', 'grupo')");
                    table.CheckConstraint("ck_conversaciones_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_conversaciones_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cotizacion_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    cotizacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    total_linea = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: true, computedColumnSql: "cantidad * precio_unitario", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cotizacion_items", x => x.id);
                    table.CheckConstraint("ck_cotizacion_items_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.ForeignKey(
                        name: "fk_cotizacion_items_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cotizaciones",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    solicitud_cotizacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<short>(type: "smallint", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    moneda_codigo = table.Column<string>(type: "character(3)", maxLength: 3, nullable: false, defaultValue: "COP"),
                    subtotal = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false, defaultValue: 0m),
                    descuento = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false, defaultValue: 0m),
                    impuesto = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: true, computedColumnSql: "subtotal - descuento + impuesto", stored: true),
                    valido_hasta = table.Column<DateOnly>(type: "date", nullable: true),
                    notas = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 11, nullable: false, defaultValue: "enviado"),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cotizaciones", x => x.id);
                    table.UniqueConstraint("AK_cotizaciones_id_solicitud_cotizacion_id", x => new { x.id, x.solicitud_cotizacion_id });
                    table.CheckConstraint("ck_cotizaciones_estado_enum", "\"estado\" IN ('enviado', 'reemplazado', 'aceptado', 'rechazado', 'vencido')");
                    table.CheckConstraint("ck_cotizaciones_notas_longitud", "char_length(\"notas\") <= 4000");
                    table.ForeignKey(
                        name: "fk_cotizaciones_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_cotizaciones_moneda_codigo_monedas",
                        column: x => x.moneda_codigo,
                        principalTable: "monedas",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "solicitudes_cotizacion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_solicitante_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversacion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    mensaje = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    fecha_deseada = table.Column<DateOnly>(type: "date", nullable: true),
                    ciudad_entrega_id = table.Column<int>(type: "integer", nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "pendiente"),
                    cotizacion_aceptada_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cerrado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solicitudes_cotizacion", x => x.id);
                    table.CheckConstraint("ck_solicitudes_cotizacion_estado_enum", "\"estado\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
                    table.CheckConstraint("ck_solicitudes_cotizacion_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
                    table.CheckConstraint("ck_solicitudes_cotizacion_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_ciudad_entrega_id_ciudades",
                        column: x => x.ciudad_entrega_id,
                        principalTable: "ciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_conversacion_id_conversaciones",
                        column: x => x.conversacion_id,
                        principalTable: "conversaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_cotizacion_aceptada_id_id_co_e86cb7ca",
                        columns: x => new { x.cotizacion_aceptada_id, x.id },
                        principalTable: "cotizaciones",
                        principalColumns: new[] { "id", "solicitud_cotizacion_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_creado_por_usuario_id_usuarios",
                        column: x => x.creado_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitudes_cotizacion_perfil_solicitante_id_perfiles",
                        column: x => x.perfil_solicitante_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historial_solicitudes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    solicitud_cotizacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estado_anterior = table.Column<string>(type: "text", maxLength: 10, nullable: true),
                    estado_nuevo = table.Column<string>(type: "text", maxLength: 10, nullable: false),
                    usuario_actor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    nota = table.Column<string>(type: "text", maxLength: 4000, nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historial_solicitudes", x => x.id);
                    table.CheckConstraint("ck_historial_solicitudes_estado_anterior_enum", "\"estado_anterior\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
                    table.CheckConstraint("ck_historial_solicitudes_estado_nuevo_enum", "\"estado_nuevo\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
                    table.CheckConstraint("ck_historial_solicitudes_nota_longitud", "char_length(\"nota\") <= 4000");
                    table.ForeignKey(
                        name: "fk_historial_solicitudes_solicitud_cotizacion_id_solic_3cc7a2f1",
                        column: x => x.solicitud_cotizacion_id,
                        principalTable: "solicitudes_cotizacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_historial_solicitudes_usuario_actor_id_usuarios",
                        column: x => x.usuario_actor_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resenas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    usuario_resenador_id = table.Column<Guid>(type: "uuid", nullable: false),
                    emprendimiento_id = table.Column<Guid>(type: "uuid", nullable: false),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    solicitud_cotizacion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    calificacion = table.Column<short>(type: "smallint", nullable: false),
                    titulo = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    contenido = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    estado = table.Column<string>(type: "text", maxLength: 9, nullable: false, defaultValue: "publicado"),
                    respuesta_emprendimiento = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    respondido_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    respondido_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    actualizado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resenas", x => x.id);
                    table.CheckConstraint("ck_resenas_calificacion", "\"calificacion\" BETWEEN 1 AND 5");
                    table.CheckConstraint("ck_resenas_contenido_longitud", "char_length(\"contenido\") <= 10000");
                    table.CheckConstraint("ck_resenas_estado_enum", "\"estado\" IN ('publicado', 'oculto')");
                    table.CheckConstraint("ck_resenas_respuesta_emprendimiento_longitud", "char_length(\"respuesta_emprendimiento\") <= 10000");
                    table.CheckConstraint("ck_resenas_titulo_longitud", "char_length(\"titulo\") <= 255");
                    table.ForeignKey(
                        name: "fk_resenas_emprendimiento_id_emprendimientos",
                        column: x => x.emprendimiento_id,
                        principalTable: "emprendimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resenas_producto_id_emprendimiento_id_productos",
                        columns: x => new { x.producto_id, x.emprendimiento_id },
                        principalTable: "productos",
                        principalColumns: new[] { "id", "emprendimiento_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resenas_respondido_por_usuario_id_usuarios",
                        column: x => x.respondido_por_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resenas_solicitud_cotizacion_id_solicitudes_cotizacion",
                        column: x => x.solicitud_cotizacion_id,
                        principalTable: "solicitudes_cotizacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resenas_usuario_resenador_id_usuarios",
                        column: x => x.usuario_resenador_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "solicitud_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    solicitud_cotizacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    descripcion = table.Column<string>(type: "text", maxLength: 10000, nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 1m),
                    notas = table.Column<string>(type: "text", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solicitud_items", x => x.id);
                    table.CheckConstraint("ck_solicitud_items_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
                    table.CheckConstraint("ck_solicitud_items_notas_longitud", "char_length(\"notas\") <= 4000");
                    table.ForeignKey(
                        name: "fk_solicitud_items_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_solicitud_items_solicitud_cotizacion_id_solicitudes_c716681e",
                        column: x => x.solicitud_cotizacion_id,
                        principalTable: "solicitudes_cotizacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resenas_archivos",
                columns: table => new
                {
                    resena_id = table.Column<Guid>(type: "uuid", nullable: false),
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    posicion = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resenas_archivos", x => new { x.resena_id, x.archivo_id });
                    table.ForeignKey(
                        name: "fk_resenas_archivos_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resenas_archivos_resena_id_resenas",
                        column: x => x.resena_id,
                        principalTable: "resenas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mensajes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    conversacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_remitente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_remitente_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "texto"),
                    contenido = table.Column<string>(type: "text", maxLength: 10000, nullable: true),
                    producto_id = table.Column<Guid>(type: "uuid", nullable: true),
                    solicitud_cotizacion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    responde_a_mensaje_id = table.Column<Guid>(type: "uuid", nullable: true),
                    editado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    eliminado_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    creado_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mensajes", x => x.id);
                    table.CheckConstraint("ck_mensajes_contenido_longitud", "char_length(\"contenido\") <= 10000");
                    table.CheckConstraint("ck_mensajes_tipo_enum", "\"tipo\" IN ('texto', 'archivo', 'producto', 'cotizacion', 'sistema')");
                    table.ForeignKey(
                        name: "fk_mensajes_producto_id_productos",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_mensajes_responde_a_mensaje_id_mensajes",
                        column: x => x.responde_a_mensaje_id,
                        principalTable: "mensajes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_mensajes_solicitud_cotizacion_id_solicitudes_cotizacion",
                        column: x => x.solicitud_cotizacion_id,
                        principalTable: "solicitudes_cotizacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_mensajes_usuario_remitente_id_usuarios",
                        column: x => x.usuario_remitente_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mensajes_adjuntos",
                columns: table => new
                {
                    mensaje_id = table.Column<Guid>(type: "uuid", nullable: false),
                    archivo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    posicion = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mensajes_adjuntos", x => new { x.mensaje_id, x.archivo_id });
                    table.ForeignKey(
                        name: "fk_mensajes_adjuntos_archivo_id_archivos",
                        column: x => x.archivo_id,
                        principalTable: "archivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_mensajes_adjuntos_mensaje_id_mensajes",
                        column: x => x.mensaje_id,
                        principalTable: "mensajes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participantes",
                columns: table => new
                {
                    conversacion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    perfil_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol = table.Column<string>(type: "text", maxLength: 13, nullable: false, defaultValue: "miembro"),
                    usuario_asignado_id = table.Column<Guid>(type: "uuid", nullable: true),
                    unido_en = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    salio_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    ultimo_mensaje_leido_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ultima_lectura_en = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    silenciado_hasta = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    archivado_en = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_participantes", x => new { x.conversacion_id, x.perfil_id });
                    table.CheckConstraint("ck_participantes_rol_enum", "\"rol\" IN ('miembro', 'administrador')");
                    table.ForeignKey(
                        name: "fk_participantes_conversacion_id_conversaciones",
                        column: x => x.conversacion_id,
                        principalTable: "conversaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_participantes_perfil_id_perfiles",
                        column: x => x.perfil_id,
                        principalTable: "perfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_participantes_ultimo_mensaje_leido_id_mensajes",
                        column: x => x.ultimo_mensaje_leido_id,
                        principalTable: "mensajes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_participantes_usuario_asignado_id_usuarios",
                        column: x => x.usuario_asignado_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_id_tipo_perfil",
                table: "usuarios",
                columns: new[] { "id", "tipo_perfil" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_usuarios_correo_longitud",
                table: "usuarios",
                sql: "char_length(\"correo\") <= 254");

            migrationBuilder.AddCheckConstraint(
                name: "ck_usuarios_hash_contrasena_longitud",
                table: "usuarios",
                sql: "char_length(\"hash_contrasena\") <= 512");

            migrationBuilder.AddCheckConstraint(
                name: "ck_usuarios_rol_plataforma_enum",
                table: "usuarios",
                sql: "\"rol_plataforma\" IN ('usuario', 'moderador', 'administrador')");

            migrationBuilder.AddCheckConstraint(
                name: "ck_usuarios_tipo_perfil",
                table: "usuarios",
                sql: "\"tipo_perfil\" = 'persona'");

            migrationBuilder.AddCheckConstraint(
                name: "ck_usuarios_tipo_perfil_enum",
                table: "usuarios",
                sql: "\"tipo_perfil\" IN ('persona', 'emprendimiento')");

            migrationBuilder.CreateIndex(
                name: "ix_archivos_estado_creado_en",
                table: "archivos",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_archivos_subido_por_usuario_id",
                table: "archivos",
                column: "subido_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ux_archivos_clave_objeto",
                table: "archivos",
                column: "clave_objeto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bloqueos_perfil_bloqueado_id",
                table: "bloqueos",
                column: "perfil_bloqueado_id");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_padre_id",
                table: "categorias",
                column: "padre_id");

            migrationBuilder.CreateIndex(
                name: "ux_categorias_slug",
                table: "categorias",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ciudades_departamento_id",
                table: "ciudades",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_comentario_padre_id_publicacion_id",
                table: "comentarios",
                columns: new[] { "comentario_padre_id", "publicacion_id" });

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_creado_por_usuario_id",
                table: "comentarios",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_perfil_autor_id",
                table: "comentarios",
                column: "perfil_autor_id");

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_publicacion_id",
                table: "comentarios",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_comentarios_responde_a_perfil_id",
                table: "comentarios",
                column: "responde_a_perfil_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversaciones_creado_por_usuario_id",
                table: "conversaciones",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversaciones_ultimo_mensaje_id",
                table: "conversaciones",
                column: "ultimo_mensaje_id");

            migrationBuilder.CreateIndex(
                name: "ux_conversaciones_clave_directa",
                table: "conversaciones",
                column: "clave_directa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cotizacion_items_cotizacion_id",
                table: "cotizacion_items",
                column: "cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_cotizacion_items_producto_id",
                table: "cotizacion_items",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_cotizaciones_creado_por_usuario_id",
                table: "cotizaciones",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_cotizaciones_estado_creado_en",
                table: "cotizaciones",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_cotizaciones_moneda_codigo",
                table: "cotizaciones",
                column: "moneda_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_cotizaciones_solicitud_cotizacion_id",
                table: "cotizaciones",
                column: "solicitud_cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_departamentos_pais_codigo",
                table: "departamentos",
                column: "pais_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_documentos_verificacion_archivo_id",
                table: "documentos_verificacion",
                column: "archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_emprendimientos_categoria_id",
                table: "emprendimientos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_emprendimientos_creado_por_usuario_id",
                table: "emprendimientos",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_emprendimientos_id_tipo_perfil",
                table: "emprendimientos",
                columns: new[] { "id", "tipo_perfil" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_enlaces_emprendimiento_emprendimiento_id",
                table: "enlaces_emprendimiento",
                column: "emprendimiento_id");

            migrationBuilder.CreateIndex(
                name: "ux_etiquetas_nombre",
                table: "etiquetas",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_guardados_producto_id",
                table: "guardados",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_guardados_publicacion_id",
                table: "guardados",
                column: "publicacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_guardados_usuario_id",
                table: "guardados",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_solicitudes_solicitud_cotizacion_id",
                table: "historial_solicitudes",
                column: "solicitud_cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_solicitudes_usuario_actor_id",
                table: "historial_solicitudes",
                column: "usuario_actor_id");

            migrationBuilder.CreateIndex(
                name: "ix_intereses_usuario_categoria_id",
                table: "intereses_usuario",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_menciones_perfil_id",
                table: "menciones",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_conversacion_id_perfil_remitente_id",
                table: "mensajes",
                columns: new[] { "conversacion_id", "perfil_remitente_id" });

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_producto_id",
                table: "mensajes",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_responde_a_mensaje_id",
                table: "mensajes",
                column: "responde_a_mensaje_id");

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_solicitud_cotizacion_id",
                table: "mensajes",
                column: "solicitud_cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_usuario_remitente_id",
                table: "mensajes",
                column: "usuario_remitente_id");

            migrationBuilder.CreateIndex(
                name: "ix_mensajes_adjuntos_archivo_id",
                table: "mensajes_adjuntos",
                column: "archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_emprendimiento_estado_creado_en",
                table: "miembros_emprendimiento",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_miembros_emprendimiento_invitado_por_usuario_id",
                table: "miembros_emprendimiento",
                column: "invitado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_miembros_emprendimiento_usuario_id",
                table: "miembros_emprendimiento",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_notificaciones_perfil_actor_id",
                table: "notificaciones",
                column: "perfil_actor_id");

            migrationBuilder.CreateIndex(
                name: "ix_notificaciones_usuario_destinatario_id",
                table: "notificaciones",
                column: "usuario_destinatario_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_mentoria_categoria_id",
                table: "ofertas_mentoria",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_mentoria_moneda_codigo",
                table: "ofertas_mentoria",
                column: "moneda_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_mentoria_usuario_mentor_id",
                table: "ofertas_mentoria",
                column: "usuario_mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_categoria_id",
                table: "oportunidades",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_ciudad_id",
                table: "oportunidades",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_creado_por_usuario_id",
                table: "oportunidades",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_estado_creado_en",
                table: "oportunidades",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_perfil_autor_id",
                table: "oportunidades",
                column: "perfil_autor_id");

            migrationBuilder.CreateIndex(
                name: "ix_oportunidades_tipo_codigo",
                table: "oportunidades",
                column: "tipo_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_paises_moneda_defecto_codigo",
                table: "paises",
                column: "moneda_defecto_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_participantes_perfil_id",
                table: "participantes",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "ix_participantes_ultimo_mensaje_leido_id",
                table: "participantes",
                column: "ultimo_mensaje_leido_id");

            migrationBuilder.CreateIndex(
                name: "ix_participantes_usuario_asignado_id",
                table: "participantes",
                column: "usuario_asignado_id");

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_ciudad_id",
                table: "perfiles",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_estado_creado_en",
                table: "perfiles",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_foto_archivo_id",
                table: "perfiles",
                column: "foto_archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_perfiles_portada_archivo_id",
                table: "perfiles",
                column: "portada_archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_postulaciones_creado_por_usuario_id",
                table: "postulaciones",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_postulaciones_estado_creado_en",
                table: "postulaciones",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_postulaciones_oportunidad_id",
                table: "postulaciones",
                column: "oportunidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_postulaciones_perfil_postulante_id",
                table: "postulaciones",
                column: "perfil_postulante_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_categoria_id",
                table: "productos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_ciudad_id",
                table: "productos",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_creado_por_usuario_id",
                table: "productos",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_emprendimiento_id",
                table: "productos",
                column: "emprendimiento_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_estado_creado_en",
                table: "productos",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_productos_moneda_codigo",
                table: "productos",
                column: "moneda_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_productos_archivos_archivo_id",
                table: "productos_archivos",
                column: "archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_productos_etiquetas_etiqueta_id",
                table: "productos_etiquetas",
                column: "etiqueta_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_comentario_aceptado_id_id",
                table: "publicaciones",
                columns: new[] { "comentario_aceptado_id", "id" });

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_creado_por_usuario_id",
                table: "publicaciones",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_oportunidad_id",
                table: "publicaciones",
                column: "oportunidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_perfil_autor_id",
                table: "publicaciones",
                column: "perfil_autor_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_producto_id",
                table: "publicaciones",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_publicacion_compartida_id",
                table: "publicaciones",
                column: "publicacion_compartida_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_tipo_publicacion_codigo",
                table: "publicaciones",
                column: "tipo_publicacion_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_archivos_archivo_id",
                table: "publicaciones_archivos",
                column: "archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_publicaciones_etiquetas_etiqueta_id",
                table: "publicaciones_etiquetas",
                column: "etiqueta_id");

            migrationBuilder.CreateIndex(
                name: "ix_reacciones_comentario_perfil_id",
                table: "reacciones_comentario",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "ix_reacciones_comentario_tipo_reaccion_codigo",
                table: "reacciones_comentario",
                column: "tipo_reaccion_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_reacciones_publicacion_perfil_id",
                table: "reacciones_publicacion",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "ix_reacciones_publicacion_tipo_reaccion_codigo",
                table: "reacciones_publicacion",
                column: "tipo_reaccion_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_reportes_estado_creado_en",
                table: "reportes",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_reportes_motivo_codigo",
                table: "reportes",
                column: "motivo_codigo");

            migrationBuilder.CreateIndex(
                name: "ix_reportes_resuelto_por_usuario_id",
                table: "reportes",
                column: "resuelto_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_reportes_usuario_reportante_id",
                table: "reportes",
                column: "usuario_reportante_id");

            migrationBuilder.CreateIndex(
                name: "ix_resenas_emprendimiento_id",
                table: "resenas",
                column: "emprendimiento_id");

            migrationBuilder.CreateIndex(
                name: "ix_resenas_estado_creado_en",
                table: "resenas",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_resenas_producto_id_emprendimiento_id",
                table: "resenas",
                columns: new[] { "producto_id", "emprendimiento_id" });

            migrationBuilder.CreateIndex(
                name: "ix_resenas_respondido_por_usuario_id",
                table: "resenas",
                column: "respondido_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_resenas_solicitud_cotizacion_id",
                table: "resenas",
                column: "solicitud_cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_resenas_usuario_resenador_id",
                table: "resenas",
                column: "usuario_resenador_id");

            migrationBuilder.CreateIndex(
                name: "ix_resenas_archivos_archivo_id",
                table: "resenas_archivos",
                column: "archivo_id");

            migrationBuilder.CreateIndex(
                name: "ix_seguimientos_estado_creado_en",
                table: "seguimientos",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_seguimientos_perfil_seguido_id",
                table: "seguimientos",
                column: "perfil_seguido_id");

            migrationBuilder.CreateIndex(
                name: "ix_sesiones_reemplazado_por_id",
                table: "sesiones",
                column: "reemplazado_por_id");

            migrationBuilder.CreateIndex(
                name: "ix_sesiones_usuario_id",
                table: "sesiones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ux_sesiones_hash_token_refresco",
                table: "sesiones",
                column: "hash_token_refresco",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_solicitud_items_producto_id",
                table: "solicitud_items",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitud_items_solicitud_cotizacion_id",
                table: "solicitud_items",
                column: "solicitud_cotizacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_ciudad_entrega_id",
                table: "solicitudes_cotizacion",
                column: "ciudad_entrega_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_conversacion_id",
                table: "solicitudes_cotizacion",
                column: "conversacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_cotizacion_aceptada_id_id",
                table: "solicitudes_cotizacion",
                columns: new[] { "cotizacion_aceptada_id", "id" });

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_creado_por_usuario_id",
                table: "solicitudes_cotizacion",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_emprendimiento_id",
                table: "solicitudes_cotizacion",
                column: "emprendimiento_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_estado_creado_en",
                table: "solicitudes_cotizacion",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_cotizacion_perfil_solicitante_id",
                table: "solicitudes_cotizacion",
                column: "perfil_solicitante_id");

            migrationBuilder.CreateIndex(
                name: "ux_solicitudes_cotizacion_numero",
                table: "solicitudes_cotizacion",
                column: "numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_mentoria_creado_por_usuario_id",
                table: "solicitudes_mentoria",
                column: "creado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_mentoria_estado_creado_en",
                table: "solicitudes_mentoria",
                columns: new[] { "estado", "creado_en" });

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_mentoria_oferta_id",
                table: "solicitudes_mentoria",
                column: "oferta_id");

            migrationBuilder.CreateIndex(
                name: "ix_solicitudes_mentoria_perfil_mentoreado_id",
                table: "solicitudes_mentoria",
                column: "perfil_mentoreado_id");

            migrationBuilder.CreateIndex(
                name: "ix_tokens_recuperacion_usuario_id",
                table: "tokens_recuperacion",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ux_tokens_recuperacion_hash_token",
                table: "tokens_recuperacion",
                column: "hash_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuario_tipos_persona_tipo_persona_codigo",
                table: "usuario_tipos_persona",
                column: "tipo_persona_codigo");

            migrationBuilder.CreateIndex(
                name: "ux_variantes_archivo_clave_objeto",
                table: "variantes_archivo",
                column: "clave_objeto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_verificaciones_emprendimiento_id",
                table: "verificaciones",
                column: "emprendimiento_id");

            migrationBuilder.CreateIndex(
                name: "ix_verificaciones_enviado_por_usuario_id",
                table: "verificaciones",
                column: "enviado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_verificaciones_revisado_por_usuario_id",
                table: "verificaciones",
                column: "revisado_por_usuario_id");

            // Cada usuario existente obtiene el perfil requerido, con el mismo UUID y auditoría.
            // El nombre provisional es único y puede editarse posteriormente.
            migrationBuilder.Sql("""
                INSERT INTO perfiles (id, tipo, nombre_usuario, nombre_visible, creado_en, actualizado_en)
                SELECT id, 'persona', 'u_' || replace(id::text, '-', ''), correo, creado_en, actualizado_en
                FROM usuarios;
                CREATE INDEX ix_usuarios_correo_normalizado ON usuarios (lower(correo));
                """);

            migrationBuilder.AddForeignKey(
                name: "fk_usuarios_id_tipo_perfil_perfiles",
                table: "usuarios",
                columns: new[] { "id", "tipo_perfil" },
                principalTable: "perfiles",
                principalColumns: new[] { "id", "tipo" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_comentarios_publicacion_id_publicaciones",
                table: "comentarios",
                column: "publicacion_id",
                principalTable: "publicaciones",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_conversaciones_ultimo_mensaje_id_mensajes",
                table: "conversaciones",
                column: "ultimo_mensaje_id",
                principalTable: "mensajes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_cotizacion_items_cotizacion_id_cotizaciones",
                table: "cotizacion_items",
                column: "cotizacion_id",
                principalTable: "cotizaciones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cotizaciones_solicitud_cotizacion_id_solicitudes_cotizacion",
                table: "cotizaciones",
                column: "solicitud_cotizacion_id",
                principalTable: "solicitudes_cotizacion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_mensajes_conversacion_id_perfil_remitente_id_participantes",
                table: "mensajes",
                columns: new[] { "conversacion_id", "perfil_remitente_id" },
                principalTable: "participantes",
                principalColumns: new[] { "conversacion_id", "perfil_id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $validacion$
                BEGIN
                    IF EXISTS (SELECT 1 FROM usuarios WHERE hash_contrasena IS NULL OR char_length(hash_contrasena) > 500) THEN
                        RAISE EXCEPTION 'El esquema anterior exige un hash no nulo de máximo 500 caracteres. Revisa los usuarios antes de revertir.';
                    END IF;
                END $validacion$;
                DROP INDEX IF EXISTS ix_usuarios_correo_normalizado;
                """);

            migrationBuilder.DropForeignKey(
                name: "fk_usuarios_id_tipo_perfil_perfiles",
                table: "usuarios");

            migrationBuilder.DropForeignKey(
                name: "fk_comentarios_perfil_autor_id_perfiles",
                table: "comentarios");

            migrationBuilder.DropForeignKey(
                name: "fk_comentarios_responde_a_perfil_id_perfiles",
                table: "comentarios");

            migrationBuilder.DropForeignKey(
                name: "fk_emprendimientos_id_tipo_perfil_perfiles",
                table: "emprendimientos");

            migrationBuilder.DropForeignKey(
                name: "fk_oportunidades_perfil_autor_id_perfiles",
                table: "oportunidades");

            migrationBuilder.DropForeignKey(
                name: "fk_participantes_perfil_id_perfiles",
                table: "participantes");

            migrationBuilder.DropForeignKey(
                name: "fk_publicaciones_perfil_autor_id_perfiles",
                table: "publicaciones");

            migrationBuilder.DropForeignKey(
                name: "fk_solicitudes_cotizacion_perfil_solicitante_id_perfiles",
                table: "solicitudes_cotizacion");

            migrationBuilder.DropForeignKey(
                name: "fk_ciudades_departamento_id_departamentos",
                table: "ciudades");

            migrationBuilder.DropForeignKey(
                name: "fk_comentarios_publicacion_id_publicaciones",
                table: "comentarios");

            migrationBuilder.DropForeignKey(
                name: "fk_conversaciones_ultimo_mensaje_id_mensajes",
                table: "conversaciones");

            migrationBuilder.DropForeignKey(
                name: "fk_participantes_ultimo_mensaje_leido_id_mensajes",
                table: "participantes");

            migrationBuilder.DropForeignKey(
                name: "fk_solicitudes_cotizacion_cotizacion_aceptada_id_id_co_e86cb7ca",
                table: "solicitudes_cotizacion");

            migrationBuilder.DropTable(
                name: "bloqueos");

            migrationBuilder.DropTable(
                name: "cotizacion_items");

            migrationBuilder.DropTable(
                name: "documentos_verificacion");

            migrationBuilder.DropTable(
                name: "enlaces_emprendimiento");

            migrationBuilder.DropTable(
                name: "guardados");

            migrationBuilder.DropTable(
                name: "historial_solicitudes");

            migrationBuilder.DropTable(
                name: "intereses_usuario");

            migrationBuilder.DropTable(
                name: "menciones");

            migrationBuilder.DropTable(
                name: "mensajes_adjuntos");

            migrationBuilder.DropTable(
                name: "miembros_emprendimiento");

            migrationBuilder.DropTable(
                name: "notificaciones");

            migrationBuilder.DropTable(
                name: "permisos_rol");

            migrationBuilder.DropTable(
                name: "postulaciones");

            migrationBuilder.DropTable(
                name: "productos_archivos");

            migrationBuilder.DropTable(
                name: "productos_etiquetas");

            migrationBuilder.DropTable(
                name: "publicaciones_archivos");

            migrationBuilder.DropTable(
                name: "publicaciones_etiquetas");

            migrationBuilder.DropTable(
                name: "reacciones_comentario");

            migrationBuilder.DropTable(
                name: "reacciones_publicacion");

            migrationBuilder.DropTable(
                name: "reportes");

            migrationBuilder.DropTable(
                name: "resenas_archivos");

            migrationBuilder.DropTable(
                name: "seguimientos");

            migrationBuilder.DropTable(
                name: "sesiones");

            migrationBuilder.DropTable(
                name: "solicitud_items");

            migrationBuilder.DropTable(
                name: "solicitudes_mentoria");

            migrationBuilder.DropTable(
                name: "tokens_recuperacion");

            migrationBuilder.DropTable(
                name: "usuario_tipos_persona");

            migrationBuilder.DropTable(
                name: "usuarios_reservados");

            migrationBuilder.DropTable(
                name: "variantes_archivo");

            migrationBuilder.DropTable(
                name: "verificaciones");

            migrationBuilder.DropTable(
                name: "etiquetas");

            migrationBuilder.DropTable(
                name: "tipos_reaccion");

            migrationBuilder.DropTable(
                name: "motivos_reporte");

            migrationBuilder.DropTable(
                name: "resenas");

            migrationBuilder.DropTable(
                name: "ofertas_mentoria");

            migrationBuilder.DropTable(
                name: "tipos_persona");

            migrationBuilder.DropTable(
                name: "perfiles");

            migrationBuilder.DropTable(
                name: "archivos");

            migrationBuilder.DropTable(
                name: "departamentos");

            migrationBuilder.DropTable(
                name: "paises");

            migrationBuilder.DropTable(
                name: "publicaciones");

            migrationBuilder.DropTable(
                name: "comentarios");

            migrationBuilder.DropTable(
                name: "oportunidades");

            migrationBuilder.DropTable(
                name: "tipos_publicacion");

            migrationBuilder.DropTable(
                name: "tipos_oportunidad");

            migrationBuilder.DropTable(
                name: "mensajes");

            migrationBuilder.DropTable(
                name: "participantes");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "cotizaciones");

            migrationBuilder.DropTable(
                name: "monedas");

            migrationBuilder.DropTable(
                name: "solicitudes_cotizacion");

            migrationBuilder.DropTable(
                name: "ciudades");

            migrationBuilder.DropTable(
                name: "conversaciones");

            migrationBuilder.DropTable(
                name: "emprendimientos");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "ix_usuarios_id_tipo_perfil",
                table: "usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "ck_usuarios_correo_longitud",
                table: "usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "ck_usuarios_hash_contrasena_longitud",
                table: "usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "ck_usuarios_rol_plataforma_enum",
                table: "usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "ck_usuarios_tipo_perfil",
                table: "usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "ck_usuarios_tipo_perfil_enum",
                table: "usuarios");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "correo",
                table: "Usuarios",
                newName: "Correo");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuarios",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ultimo_ingreso_en",
                table: "Usuarios",
                newName: "UltimoIngresoEn");

            migrationBuilder.RenameColumn(
                name: "tipo_perfil",
                table: "Usuarios",
                newName: "TipoPerfil");

            migrationBuilder.RenameColumn(
                name: "terminos_aceptados_en",
                table: "Usuarios",
                newName: "TerminosAceptadosEn");

            migrationBuilder.RenameColumn(
                name: "rol_plataforma",
                table: "Usuarios",
                newName: "RolPlataforma");

            migrationBuilder.RenameColumn(
                name: "intentos_fallidos",
                table: "Usuarios",
                newName: "IntentosFallidos");

            migrationBuilder.RenameColumn(
                name: "hash_contrasena",
                table: "Usuarios",
                newName: "HashContrasena");

            migrationBuilder.RenameColumn(
                name: "creado_en",
                table: "Usuarios",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "correo_verificado_en",
                table: "Usuarios",
                newName: "CorreoVerificadoEn");

            migrationBuilder.RenameColumn(
                name: "contrasena_cambiada_en",
                table: "Usuarios",
                newName: "ContrasenaCambiadaEn");

            migrationBuilder.RenameColumn(
                name: "bloqueado_hasta",
                table: "Usuarios",
                newName: "BloqueadoHasta");

            migrationBuilder.RenameColumn(
                name: "actualizado_en",
                table: "Usuarios",
                newName: "ActualizadoEn");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimoIngresoEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.Sql("""
                ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" DROP DEFAULT;
                ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" TYPE integer USING (CASE "TipoPerfil" WHEN 'persona' THEN 0 END);
                ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" SET DEFAULT 0;
                """);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TerminosAceptadosEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.Sql("""
                ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" DROP DEFAULT;
                ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" TYPE integer USING (CASE "RolPlataforma" WHEN 'usuario' THEN 0 WHEN 'moderador' THEN 1 WHEN 'administrador' THEN 2 END);
                ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" SET DEFAULT 0;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "HashContrasena",
                table: "Usuarios",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreadoEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CorreoVerificadoEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContrasenaCambiadaEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BloqueadoHasta",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }
    }
}
