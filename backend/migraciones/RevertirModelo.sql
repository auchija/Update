START TRANSACTION;
DO $validacion$
BEGIN
    IF EXISTS (SELECT 1 FROM usuarios WHERE hash_contrasena IS NULL OR char_length(hash_contrasena) > 500) THEN
        RAISE EXCEPTION 'El esquema anterior exige un hash no nulo de máximo 500 caracteres. Revisa los usuarios antes de revertir.';
    END IF;
END $validacion$;
DROP INDEX IF EXISTS ix_usuarios_correo_normalizado;

ALTER TABLE usuarios DROP CONSTRAINT fk_usuarios_id_tipo_perfil_perfiles;

ALTER TABLE comentarios DROP CONSTRAINT fk_comentarios_perfil_autor_id_perfiles;

ALTER TABLE comentarios DROP CONSTRAINT fk_comentarios_responde_a_perfil_id_perfiles;

ALTER TABLE emprendimientos DROP CONSTRAINT fk_emprendimientos_id_tipo_perfil_perfiles;

ALTER TABLE oportunidades DROP CONSTRAINT fk_oportunidades_perfil_autor_id_perfiles;

ALTER TABLE participantes DROP CONSTRAINT fk_participantes_perfil_id_perfiles;

ALTER TABLE publicaciones DROP CONSTRAINT fk_publicaciones_perfil_autor_id_perfiles;

ALTER TABLE solicitudes_cotizacion DROP CONSTRAINT fk_solicitudes_cotizacion_perfil_solicitante_id_perfiles;

ALTER TABLE ciudades DROP CONSTRAINT fk_ciudades_departamento_id_departamentos;

ALTER TABLE comentarios DROP CONSTRAINT fk_comentarios_publicacion_id_publicaciones;

ALTER TABLE conversaciones DROP CONSTRAINT fk_conversaciones_ultimo_mensaje_id_mensajes;

ALTER TABLE participantes DROP CONSTRAINT fk_participantes_ultimo_mensaje_leido_id_mensajes;

ALTER TABLE solicitudes_cotizacion DROP CONSTRAINT fk_solicitudes_cotizacion_cotizacion_aceptada_id_id_co_e86cb7ca;

DROP TABLE bloqueos;

DROP TABLE cotizacion_items;

DROP TABLE documentos_verificacion;

DROP TABLE enlaces_emprendimiento;

DROP TABLE guardados;

DROP TABLE historial_solicitudes;

DROP TABLE intereses_usuario;

DROP TABLE menciones;

DROP TABLE mensajes_adjuntos;

DROP TABLE miembros_emprendimiento;

DROP TABLE notificaciones;

DROP TABLE permisos_rol;

DROP TABLE postulaciones;

DROP TABLE productos_archivos;

DROP TABLE productos_etiquetas;

DROP TABLE publicaciones_archivos;

DROP TABLE publicaciones_etiquetas;

DROP TABLE reacciones_comentario;

DROP TABLE reacciones_publicacion;

DROP TABLE reportes;

DROP TABLE resenas_archivos;

DROP TABLE seguimientos;

DROP TABLE sesiones;

DROP TABLE solicitud_items;

DROP TABLE solicitudes_mentoria;

DROP TABLE tokens_recuperacion;

DROP TABLE usuario_tipos_persona;

DROP TABLE usuarios_reservados;

DROP TABLE variantes_archivo;

DROP TABLE verificaciones;

DROP TABLE etiquetas;

DROP TABLE tipos_reaccion;

DROP TABLE motivos_reporte;

DROP TABLE resenas;

DROP TABLE ofertas_mentoria;

DROP TABLE tipos_persona;

DROP TABLE perfiles;

DROP TABLE archivos;

DROP TABLE departamentos;

DROP TABLE paises;

DROP TABLE publicaciones;

DROP TABLE comentarios;

DROP TABLE oportunidades;

DROP TABLE tipos_publicacion;

DROP TABLE tipos_oportunidad;

DROP TABLE mensajes;

DROP TABLE participantes;

DROP TABLE productos;

DROP TABLE cotizaciones;

DROP TABLE monedas;

DROP TABLE solicitudes_cotizacion;

DROP TABLE ciudades;

DROP TABLE conversaciones;

DROP TABLE emprendimientos;

DROP TABLE categorias;

ALTER TABLE usuarios DROP CONSTRAINT pk_usuarios;

DROP INDEX ix_usuarios_id_tipo_perfil;

ALTER TABLE usuarios DROP CONSTRAINT ck_usuarios_correo_longitud;

ALTER TABLE usuarios DROP CONSTRAINT ck_usuarios_hash_contrasena_longitud;

ALTER TABLE usuarios DROP CONSTRAINT ck_usuarios_rol_plataforma_enum;

ALTER TABLE usuarios DROP CONSTRAINT ck_usuarios_tipo_perfil;

ALTER TABLE usuarios DROP CONSTRAINT ck_usuarios_tipo_perfil_enum;

ALTER TABLE usuarios RENAME TO "Usuarios";

ALTER TABLE "Usuarios" RENAME COLUMN correo TO "Correo";

ALTER TABLE "Usuarios" RENAME COLUMN id TO "Id";

ALTER TABLE "Usuarios" RENAME COLUMN ultimo_ingreso_en TO "UltimoIngresoEn";

ALTER TABLE "Usuarios" RENAME COLUMN tipo_perfil TO "TipoPerfil";

ALTER TABLE "Usuarios" RENAME COLUMN terminos_aceptados_en TO "TerminosAceptadosEn";

ALTER TABLE "Usuarios" RENAME COLUMN rol_plataforma TO "RolPlataforma";

ALTER TABLE "Usuarios" RENAME COLUMN intentos_fallidos TO "IntentosFallidos";

ALTER TABLE "Usuarios" RENAME COLUMN hash_contrasena TO "HashContrasena";

ALTER TABLE "Usuarios" RENAME COLUMN creado_en TO "CreadoEn";

ALTER TABLE "Usuarios" RENAME COLUMN correo_verificado_en TO "CorreoVerificadoEn";

ALTER TABLE "Usuarios" RENAME COLUMN contrasena_cambiada_en TO "ContrasenaCambiadaEn";

ALTER TABLE "Usuarios" RENAME COLUMN bloqueado_hasta TO "BloqueadoHasta";

ALTER TABLE "Usuarios" RENAME COLUMN actualizado_en TO "ActualizadoEn";

ALTER TABLE "Usuarios" ALTER COLUMN "Correo" TYPE character varying(256);

ALTER TABLE "Usuarios" ALTER COLUMN "UltimoIngresoEn" TYPE timestamp with time zone;

ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" DROP DEFAULT;
ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" TYPE integer USING (CASE "TipoPerfil" WHEN 'persona' THEN 0 END);
ALTER TABLE "Usuarios" ALTER COLUMN "TipoPerfil" SET DEFAULT 0;

ALTER TABLE "Usuarios" ALTER COLUMN "TerminosAceptadosEn" TYPE timestamp with time zone;

ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" DROP DEFAULT;
ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" TYPE integer USING (CASE "RolPlataforma" WHEN 'usuario' THEN 0 WHEN 'moderador' THEN 1 WHEN 'administrador' THEN 2 END);
ALTER TABLE "Usuarios" ALTER COLUMN "RolPlataforma" SET DEFAULT 0;

ALTER TABLE "Usuarios" ALTER COLUMN "HashContrasena" TYPE character varying(500);
UPDATE "Usuarios" SET "HashContrasena" = '' WHERE "HashContrasena" IS NULL;
ALTER TABLE "Usuarios" ALTER COLUMN "HashContrasena" SET NOT NULL;
ALTER TABLE "Usuarios" ALTER COLUMN "HashContrasena" SET DEFAULT '';

ALTER TABLE "Usuarios" ALTER COLUMN "CreadoEn" TYPE timestamp with time zone;
ALTER TABLE "Usuarios" ALTER COLUMN "CreadoEn" DROP DEFAULT;

ALTER TABLE "Usuarios" ALTER COLUMN "CorreoVerificadoEn" TYPE timestamp with time zone;

ALTER TABLE "Usuarios" ALTER COLUMN "ContrasenaCambiadaEn" TYPE timestamp with time zone;

ALTER TABLE "Usuarios" ALTER COLUMN "BloqueadoHasta" TYPE timestamp with time zone;

ALTER TABLE "Usuarios" ALTER COLUMN "ActualizadoEn" TYPE timestamp with time zone;
ALTER TABLE "Usuarios" ALTER COLUMN "ActualizadoEn" DROP NOT NULL;
ALTER TABLE "Usuarios" ALTER COLUMN "ActualizadoEn" DROP DEFAULT;

ALTER TABLE "Usuarios" ADD CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id");

CREATE UNIQUE INDEX "IX_Usuarios_Correo" ON "Usuarios" ("Correo");

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20261002032844_ImplementarModeloUpDate';

COMMIT;

