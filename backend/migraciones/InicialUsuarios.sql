CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Usuarios" (
    "Id" uuid NOT NULL,
    "TipoPerfil" integer NOT NULL DEFAULT 0,
    "Correo" character varying(256) NOT NULL,
    "HashContrasena" character varying(500) NOT NULL,
    "RolPlataforma" integer NOT NULL DEFAULT 0,
    "CorreoVerificadoEn" timestamp with time zone,
    "TerminosAceptadosEn" timestamp with time zone NOT NULL,
    "UltimoIngresoEn" timestamp with time zone,
    "IntentosFallidos" smallint NOT NULL DEFAULT 0,
    "BloqueadoHasta" timestamp with time zone,
    "ContrasenaCambiadaEn" timestamp with time zone,
    "CreadoEn" timestamp with time zone NOT NULL,
    "ActualizadoEn" timestamp with time zone,
    CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Usuarios_Correo" ON "Usuarios" ("Correo");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261001230947_CreateUsuariosTable', '10.0.12');

COMMIT;

