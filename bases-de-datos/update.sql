BEGIN;


CREATE TYPE tipo_perfil         AS ENUM ('persona', 'emprendimiento');
CREATE TYPE estado_cuenta       AS ENUM ('activo', 'suspendido', 'eliminado');
CREATE TYPE rol_plataforma      AS ENUM ('usuario', 'moderador', 'administrador');

CREATE TYPE rol_emprendimiento   AS ENUM ('propietario', 'administrador', 'editor', 'soporte', 'miembro');
CREATE TYPE estado_miembro       AS ENUM ('invitado', 'activo', 'retirado', 'expulsado');
CREATE TYPE etapa_emprendimiento AS ENUM ('idea', 'validacion', 'lanzamiento', 'crecimiento', 'consolidado');
CREATE TYPE estado_verificacion  AS ENUM ('sin_verificar', 'pendiente', 'aprobado', 'rechazado');

CREATE TYPE estado_seguimiento      AS ENUM ('pendiente', 'activo');
CREATE TYPE visibilidad_publicacion AS ENUM ('publico', 'seguidores');

CREATE TYPE tipo_producto     AS ENUM ('producto', 'servicio');
CREATE TYPE estado_producto   AS ENUM ('borrador', 'activo', 'pausado', 'archivado');
CREATE TYPE tipo_precio       AS ENUM ('fijo', 'desde', 'a_convenir', 'gratis');
CREATE TYPE estado_inventario AS ENUM ('disponible', 'agotado', 'bajo_pedido', 'no_aplica');

CREATE TYPE tipo_archivo        AS ENUM ('imagen', 'video', 'documento');
CREATE TYPE proposito_archivo   AS ENUM ('foto_perfil', 'portada', 'publicacion', 'producto', 'mensaje', 'resena', 'verificacion', 'otro');
CREATE TYPE estado_archivo      AS ENUM ('pendiente', 'listo', 'fallido', 'eliminado');
CREATE TYPE visibilidad_archivo AS ENUM ('publico', 'privado');

CREATE TYPE tipo_conversacion AS ENUM ('directa', 'grupo');
CREATE TYPE rol_participante  AS ENUM ('miembro', 'administrador');
CREATE TYPE tipo_mensaje      AS ENUM ('texto', 'archivo', 'producto', 'cotizacion', 'sistema');

CREATE TYPE estado_solicitud  AS ENUM ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado');
CREATE TYPE estado_cotizacion AS ENUM ('enviado', 'reemplazado', 'aceptado', 'rechazado', 'vencido');

CREATE TYPE estado_oportunidad AS ENUM ('abierto', 'cerrado', 'cubierto');
CREATE TYPE estado_postulacion AS ENUM ('pendiente', 'aceptado', 'rechazado', 'retirado');
CREATE TYPE estado_mentoria    AS ENUM ('solicitado', 'aceptado', 'rechazado', 'agendado', 'completado', 'cancelado');
CREATE TYPE modalidad_mentoria AS ENUM ('virtual', 'presencial', 'ambas');

CREATE TYPE estado_resena     AS ENUM ('publicado', 'oculto');
CREATE TYPE objetivo_reporte  AS ENUM ('perfil', 'publicacion', 'comentario', 'producto', 'mensaje', 'resena', 'oportunidad');
CREATE TYPE estado_reporte    AS ENUM ('abierto', 'en_revision', 'resuelto', 'descartado');
CREATE TYPE tipo_notificacion AS ENUM (
  'seguimiento', 'solicitud_seguimiento', 'seguimiento_aceptado',
  'reaccion', 'comentario', 'respuesta', 'mencion', 'compartido',
  'mensaje', 'cotizacion_actualizada', 'postulacion_actualizada',
  'mentoria_actualizada', 'resena', 'invitacion_emprendimiento',
  'respuesta_aceptada', 'sistema'
);


CREATE TABLE monedas (
  codigo     char(3)  PRIMARY KEY,
  nombre     text     NOT NULL,
  simbolo    text     NOT NULL,
  decimales  smallint NOT NULL DEFAULT 0,
  activo     boolean  NOT NULL DEFAULT true
);

CREATE TABLE paises (
  codigo                 char(2) PRIMARY KEY,
  nombre                 text    NOT NULL,
  moneda_defecto_codigo  char(3) NOT NULL REFERENCES monedas (codigo),
  prefijo_telefono       text    NOT NULL
);

CREATE TABLE departamentos (
  id          integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  pais_codigo char(2) NOT NULL REFERENCES paises (codigo),
  nombre      text    NOT NULL,
  codigo      text
);

CREATE TABLE ciudades (
  id              integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  departamento_id integer      NOT NULL REFERENCES departamentos (id),
  nombre          text         NOT NULL,
  codigo          text,
  latitud         numeric(9,6) NOT NULL,
  longitud        numeric(9,6) NOT NULL
);

CREATE TABLE categorias (
  id       integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  padre_id integer  REFERENCES categorias (id),
  slug     text     NOT NULL UNIQUE,
  nombre   text     NOT NULL,
  icono    text,
  orden    smallint NOT NULL DEFAULT 0,
  activo   boolean  NOT NULL DEFAULT true
);

CREATE TABLE tipos_persona (
  codigo      text     PRIMARY KEY,
  nombre      text     NOT NULL,
  descripcion text,
  orden       smallint NOT NULL DEFAULT 0,
  activo      boolean  NOT NULL DEFAULT true
);

CREATE TABLE tipos_publicacion (
  codigo                     text         PRIMARY KEY,
  nombre                     text         NOT NULL,
  descripcion                text,
  icono                      text,
  peso_descubrimiento        numeric(4,2) NOT NULL DEFAULT 1.00,
  permite_respuesta_aceptada boolean      NOT NULL DEFAULT false,
  orden                      smallint     NOT NULL DEFAULT 0,
  activo                     boolean      NOT NULL DEFAULT true
);

CREATE TABLE tipos_reaccion (
  codigo text     PRIMARY KEY,
  nombre text     NOT NULL,
  emoji  text     NOT NULL,
  orden  smallint NOT NULL DEFAULT 0,
  activo boolean  NOT NULL DEFAULT true
);

CREATE TABLE tipos_oportunidad (
  codigo      text     PRIMARY KEY,
  nombre      text     NOT NULL,
  descripcion text,
  orden       smallint NOT NULL DEFAULT 0,
  activo      boolean  NOT NULL DEFAULT true
);

CREATE TABLE motivos_reporte (
  codigo text     PRIMARY KEY,
  nombre text     NOT NULL,
  orden  smallint NOT NULL DEFAULT 0,
  activo boolean  NOT NULL DEFAULT true
);

CREATE TABLE usuarios_reservados (
  nombre_usuario text PRIMARY KEY
);


CREATE TABLE perfiles (
  id                  uuid          PRIMARY KEY DEFAULT gen_random_uuid(),
  tipo                tipo_perfil   NOT NULL,
  nombre_usuario      text          NOT NULL,
  nombre_visible      text          NOT NULL,
  titular             text,
  biografia           text,
  foto_archivo_id     uuid,
  portada_archivo_id  uuid,
  ciudad_id           integer       REFERENCES ciudades (id),
  sitio_web           text,
  es_privado          boolean       NOT NULL DEFAULT false,
  estado              estado_cuenta NOT NULL DEFAULT 'activo',
  total_seguidores    integer       NOT NULL DEFAULT 0,
  total_seguidos      integer       NOT NULL DEFAULT 0,
  total_publicaciones integer       NOT NULL DEFAULT 0,
  creado_en           timestamptz   NOT NULL DEFAULT now(),
  actualizado_en      timestamptz   NOT NULL DEFAULT now(),
  eliminado_en        timestamptz,
  UNIQUE (id, tipo)
);

CREATE UNIQUE INDEX perfiles_nombre_usuario_uk ON perfiles (lower(nombre_usuario));

CREATE TABLE usuarios (
  id                     uuid           PRIMARY KEY,
  tipo_perfil            tipo_perfil    NOT NULL DEFAULT 'persona'
                                        CHECK (tipo_perfil = 'persona'),
  correo                 text           NOT NULL,
  hash_contrasena        text,
  rol_plataforma         rol_plataforma NOT NULL DEFAULT 'usuario',
  correo_verificado_en   timestamptz,
  terminos_aceptados_en  timestamptz    NOT NULL,
  ultimo_ingreso_en      timestamptz,
  intentos_fallidos      smallint       NOT NULL DEFAULT 0,
  bloqueado_hasta        timestamptz,
  contrasena_cambiada_en timestamptz,
  creado_en              timestamptz    NOT NULL DEFAULT now(),
  actualizado_en         timestamptz    NOT NULL DEFAULT now(),
  FOREIGN KEY (id, tipo_perfil) REFERENCES perfiles (id, tipo)
);

CREATE UNIQUE INDEX usuarios_correo_uk ON usuarios (lower(correo));

CREATE TABLE sesiones (
  id                  uuid        PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_id          uuid        NOT NULL REFERENCES usuarios (id),
  hash_token_refresco text        NOT NULL UNIQUE,
  familia_id          uuid        NOT NULL,
  reemplazado_por_id  uuid        REFERENCES sesiones (id),
  agente_usuario      text,
  direccion_ip        inet,
  creado_en           timestamptz NOT NULL DEFAULT now(),
  ultimo_uso_en       timestamptz NOT NULL DEFAULT now(),
  expira_en           timestamptz NOT NULL,
  revocado_en         timestamptz,
  motivo_revocacion   text
);

CREATE TABLE tokens_recuperacion (
  id            uuid        PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_id    uuid        NOT NULL REFERENCES usuarios (id),
  hash_token    text        NOT NULL UNIQUE,
  ip_solicitud  inet,
  creado_en     timestamptz NOT NULL DEFAULT now(),
  expira_en     timestamptz NOT NULL,
  usado_en      timestamptz
);

CREATE TABLE usuario_tipos_persona (
  usuario_id          uuid        NOT NULL REFERENCES usuarios (id),
  tipo_persona_codigo text        NOT NULL REFERENCES tipos_persona (codigo),
  creado_en           timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (usuario_id, tipo_persona_codigo)
);

CREATE TABLE intereses_usuario (
  usuario_id   uuid        NOT NULL REFERENCES usuarios (id),
  categoria_id integer     NOT NULL REFERENCES categorias (id),
  creado_en    timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (usuario_id, categoria_id)
);


CREATE TABLE archivos (
  id                       uuid                PRIMARY KEY DEFAULT gen_random_uuid(),
  subido_por_usuario_id    uuid                NOT NULL REFERENCES usuarios (id),
  tipo                     tipo_archivo        NOT NULL,
  proposito                proposito_archivo   NOT NULL,
  proveedor_almacenamiento text                NOT NULL DEFAULT 'r2',
  bucket                   text                NOT NULL,
  clave_objeto             text                NOT NULL UNIQUE,
  nombre_original          text,
  tipo_mime                text                NOT NULL,
  extension                text                NOT NULL,
  tamano_bytes             bigint              NOT NULL,
  checksum_sha256          char(64),
  ancho                    integer,
  alto                     integer,
  duracion_segundos        numeric(8,2),
  visibilidad              visibilidad_archivo NOT NULL DEFAULT 'publico',
  estado                   estado_archivo      NOT NULL DEFAULT 'pendiente',
  creado_en                timestamptz         NOT NULL DEFAULT now(),
  actualizado_en           timestamptz         NOT NULL DEFAULT now(),
  eliminado_en             timestamptz
);

CREATE TABLE variantes_archivo (
  archivo_id   uuid   NOT NULL REFERENCES archivos (id),
  variante     text   NOT NULL,
  clave_objeto text   NOT NULL UNIQUE,
  tipo_mime    text   NOT NULL,
  ancho        integer,
  alto         integer,
  tamano_bytes bigint NOT NULL,
  PRIMARY KEY (archivo_id, variante)
);

ALTER TABLE perfiles
  ADD FOREIGN KEY (foto_archivo_id)    REFERENCES archivos (id),
  ADD FOREIGN KEY (portada_archivo_id) REFERENCES archivos (id);


CREATE TABLE emprendimientos (
  id                    uuid                 PRIMARY KEY,
  tipo_perfil           tipo_perfil          NOT NULL DEFAULT 'emprendimiento'
                                             CHECK (tipo_perfil = 'emprendimiento'),
  categoria_id          integer              NOT NULL REFERENCES categorias (id),
  etapa                 etapa_emprendimiento NOT NULL DEFAULT 'idea',
  fundado_en            date,
  razon_social          text,
  nit                   text,
  correo_contacto       text,
  telefono_contacto     text,
  whatsapp              text,
  direccion             text,
  envios_nacionales     boolean              NOT NULL DEFAULT false,
  ofrece_remoto         boolean              NOT NULL DEFAULT false,
  estado_verificacion   estado_verificacion  NOT NULL DEFAULT 'sin_verificar',
  verificado_en         timestamptz,
  calificacion_promedio numeric(3,2),
  total_calificaciones  integer              NOT NULL DEFAULT 0,
  creado_por_usuario_id uuid                 NOT NULL REFERENCES usuarios (id),
  creado_en             timestamptz          NOT NULL DEFAULT now(),
  actualizado_en        timestamptz          NOT NULL DEFAULT now(),
  FOREIGN KEY (id, tipo_perfil) REFERENCES perfiles (id, tipo)
);

CREATE TABLE miembros_emprendimiento (
  emprendimiento_id       uuid               NOT NULL REFERENCES emprendimientos (id),
  usuario_id              uuid               NOT NULL REFERENCES usuarios (id),
  rol                     rol_emprendimiento NOT NULL DEFAULT 'miembro',
  titulo                  text,
  estado                  estado_miembro     NOT NULL DEFAULT 'invitado',
  invitado_por_usuario_id uuid               REFERENCES usuarios (id),
  creado_en               timestamptz        NOT NULL DEFAULT now(),
  unido_en                timestamptz,
  actualizado_en          timestamptz        NOT NULL DEFAULT now(),
  PRIMARY KEY (emprendimiento_id, usuario_id)
);

CREATE TABLE permisos_rol (
  rol     rol_emprendimiento NOT NULL,
  permiso text               NOT NULL,
  PRIMARY KEY (rol, permiso)
);

CREATE TABLE enlaces_emprendimiento (
  id                uuid     PRIMARY KEY DEFAULT gen_random_uuid(),
  emprendimiento_id uuid     NOT NULL REFERENCES emprendimientos (id),
  plataforma        text     NOT NULL,
  url               text     NOT NULL,
  orden             smallint NOT NULL DEFAULT 0
);

CREATE TABLE verificaciones (
  id                      uuid                PRIMARY KEY DEFAULT gen_random_uuid(),
  emprendimiento_id       uuid                NOT NULL REFERENCES emprendimientos (id),
  estado                  estado_verificacion NOT NULL DEFAULT 'pendiente',
  metodo                  text                NOT NULL DEFAULT 'documentos',
  enviado_por_usuario_id  uuid                NOT NULL REFERENCES usuarios (id),
  revisado_por_usuario_id uuid                REFERENCES usuarios (id),
  notas_revisor           text,
  enviado_en              timestamptz         NOT NULL DEFAULT now(),
  revisado_en             timestamptz
);

CREATE TABLE documentos_verificacion (
  verificacion_id uuid NOT NULL REFERENCES verificaciones (id),
  archivo_id      uuid NOT NULL REFERENCES archivos (id),
  tipo_documento  text NOT NULL,
  PRIMARY KEY (verificacion_id, archivo_id)
);


CREATE TABLE seguimientos (
  usuario_seguidor_id uuid               NOT NULL REFERENCES usuarios (id),
  perfil_seguido_id   uuid               NOT NULL REFERENCES perfiles (id),
  estado              estado_seguimiento NOT NULL DEFAULT 'activo',
  creado_en           timestamptz        NOT NULL DEFAULT now(),
  aceptado_en         timestamptz,
  PRIMARY KEY (usuario_seguidor_id, perfil_seguido_id)
);

CREATE TABLE bloqueos (
  usuario_bloqueador_id uuid        NOT NULL REFERENCES usuarios (id),
  perfil_bloqueado_id   uuid        NOT NULL REFERENCES perfiles (id),
  creado_en             timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (usuario_bloqueador_id, perfil_bloqueado_id)
);


CREATE TABLE etiquetas (
  id         integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  nombre     text        NOT NULL UNIQUE,
  total_usos integer     NOT NULL DEFAULT 0,
  creado_en  timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE productos (
  id                    uuid              PRIMARY KEY DEFAULT gen_random_uuid(),
  emprendimiento_id     uuid              NOT NULL REFERENCES emprendimientos (id),
  creado_por_usuario_id uuid              NOT NULL REFERENCES usuarios (id),
  tipo                  tipo_producto     NOT NULL,
  titulo                text              NOT NULL,
  slug                  text              NOT NULL,
  descripcion           text,
  categoria_id          integer           NOT NULL REFERENCES categorias (id),
  tipo_precio           tipo_precio       NOT NULL DEFAULT 'fijo',
  precio                numeric(14,2),
  moneda_codigo         char(3)           NOT NULL DEFAULT 'COP' REFERENCES monedas (codigo),
  estado_inventario     estado_inventario NOT NULL DEFAULT 'disponible',
  cantidad_inventario   integer,
  ciudad_id             integer           REFERENCES ciudades (id),
  estado                estado_producto   NOT NULL DEFAULT 'borrador',
  calificacion_promedio numeric(3,2),
  total_calificaciones  integer           NOT NULL DEFAULT 0,
  total_guardados       integer           NOT NULL DEFAULT 0,
  publicado_en          timestamptz,
  creado_en             timestamptz       NOT NULL DEFAULT now(),
  actualizado_en        timestamptz       NOT NULL DEFAULT now(),
  eliminado_en          timestamptz,
  UNIQUE (id, emprendimiento_id)
);

CREATE TABLE productos_archivos (
  producto_id       uuid     NOT NULL REFERENCES productos (id),
  archivo_id        uuid     NOT NULL REFERENCES archivos (id),
  posicion          smallint NOT NULL,
  texto_alternativo text,
  PRIMARY KEY (producto_id, archivo_id)
);

CREATE TABLE productos_etiquetas (
  producto_id uuid    NOT NULL REFERENCES productos (id),
  etiqueta_id integer NOT NULL REFERENCES etiquetas (id),
  PRIMARY KEY (producto_id, etiqueta_id)
);


CREATE TABLE oportunidades (
  id                    uuid               PRIMARY KEY DEFAULT gen_random_uuid(),
  perfil_autor_id       uuid               NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id uuid               NOT NULL REFERENCES usuarios (id),
  tipo_codigo           text               NOT NULL REFERENCES tipos_oportunidad (codigo),
  titulo                text               NOT NULL,
  descripcion           text               NOT NULL,
  categoria_id          integer            REFERENCES categorias (id),
  ciudad_id             integer            REFERENCES ciudades (id),
  es_remoto             boolean            NOT NULL DEFAULT false,
  estado                estado_oportunidad NOT NULL DEFAULT 'abierto',
  expira_en             timestamptz,
  total_postulaciones   integer            NOT NULL DEFAULT 0,
  creado_en             timestamptz        NOT NULL DEFAULT now(),
  actualizado_en        timestamptz        NOT NULL DEFAULT now(),
  eliminado_en          timestamptz
);

CREATE TABLE postulaciones (
  id                    uuid               PRIMARY KEY DEFAULT gen_random_uuid(),
  oportunidad_id        uuid               NOT NULL REFERENCES oportunidades (id),
  perfil_postulante_id  uuid               NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id uuid               NOT NULL REFERENCES usuarios (id),
  mensaje               text               NOT NULL,
  estado                estado_postulacion NOT NULL DEFAULT 'pendiente',
  creado_en             timestamptz        NOT NULL DEFAULT now(),
  actualizado_en        timestamptz        NOT NULL DEFAULT now()
);

CREATE TABLE ofertas_mentoria (
  id                uuid               PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_mentor_id uuid               NOT NULL REFERENCES usuarios (id),
  titulo            text               NOT NULL,
  descripcion       text               NOT NULL,
  categoria_id      integer            REFERENCES categorias (id),
  modalidad         modalidad_mentoria NOT NULL DEFAULT 'virtual',
  duracion_minutos  smallint           NOT NULL DEFAULT 60,
  es_gratis         boolean            NOT NULL DEFAULT true,
  precio            numeric(14,2),
  moneda_codigo     char(3)            NOT NULL DEFAULT 'COP' REFERENCES monedas (codigo),
  activo            boolean            NOT NULL DEFAULT true,
  creado_en         timestamptz        NOT NULL DEFAULT now(),
  actualizado_en    timestamptz        NOT NULL DEFAULT now()
);

CREATE TABLE solicitudes_mentoria (
  id                     uuid            PRIMARY KEY DEFAULT gen_random_uuid(),
  oferta_id              uuid            NOT NULL REFERENCES ofertas_mentoria (id),
  perfil_mentoreado_id   uuid            NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id  uuid            NOT NULL REFERENCES usuarios (id),
  mensaje                text            NOT NULL,
  estado                 estado_mentoria NOT NULL DEFAULT 'solicitado',
  agendado_en            timestamptz,
  url_reunion            text,
  calificacion_mentoreado smallint,
  comentario_mentoreado  text,
  creado_en              timestamptz     NOT NULL DEFAULT now(),
  actualizado_en         timestamptz     NOT NULL DEFAULT now()
);


CREATE TABLE publicaciones (
  id                        uuid                    PRIMARY KEY DEFAULT gen_random_uuid(),
  perfil_autor_id           uuid                    NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id     uuid                    NOT NULL REFERENCES usuarios (id),
  tipo_publicacion_codigo   text                    NOT NULL DEFAULT 'general'
                                                    REFERENCES tipos_publicacion (codigo),
  contenido                 text                    NOT NULL DEFAULT '',
  visibilidad               visibilidad_publicacion NOT NULL DEFAULT 'publico',
  producto_id               uuid                    REFERENCES productos (id),
  oportunidad_id            uuid                    REFERENCES oportunidades (id),
  publicacion_compartida_id uuid                    REFERENCES publicaciones (id),
  comentario_aceptado_id    uuid,
  comentarios_habilitados   boolean                 NOT NULL DEFAULT true,
  fijado                    boolean                 NOT NULL DEFAULT false,
  total_reacciones          integer                 NOT NULL DEFAULT 0,
  total_comentarios         integer                 NOT NULL DEFAULT 0,
  total_compartidos         integer                 NOT NULL DEFAULT 0,
  total_guardados           integer                 NOT NULL DEFAULT 0,
  puntaje_descubrimiento    float8                  NOT NULL DEFAULT 0,
  puntaje_actualizado_en    timestamptz,
  editado_en                timestamptz,
  creado_en                 timestamptz             NOT NULL DEFAULT now(),
  actualizado_en            timestamptz             NOT NULL DEFAULT now(),
  eliminado_en              timestamptz
);

CREATE TABLE publicaciones_archivos (
  publicacion_id    uuid     NOT NULL REFERENCES publicaciones (id),
  archivo_id        uuid     NOT NULL REFERENCES archivos (id),
  posicion          smallint NOT NULL,
  texto_alternativo text,
  PRIMARY KEY (publicacion_id, archivo_id)
);

CREATE TABLE publicaciones_etiquetas (
  publicacion_id uuid    NOT NULL REFERENCES publicaciones (id),
  etiqueta_id    integer NOT NULL REFERENCES etiquetas (id),
  PRIMARY KEY (publicacion_id, etiqueta_id)
);

CREATE TABLE menciones (
  publicacion_id uuid NOT NULL REFERENCES publicaciones (id),
  perfil_id      uuid NOT NULL REFERENCES perfiles (id),
  PRIMARY KEY (publicacion_id, perfil_id)
);

CREATE TABLE comentarios (
  id                    uuid        PRIMARY KEY DEFAULT gen_random_uuid(),
  publicacion_id        uuid        NOT NULL REFERENCES publicaciones (id),
  comentario_padre_id   uuid,
  perfil_autor_id       uuid        NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id uuid        NOT NULL REFERENCES usuarios (id),
  responde_a_perfil_id  uuid        REFERENCES perfiles (id),
  contenido             text        NOT NULL,
  total_reacciones      integer     NOT NULL DEFAULT 0,
  total_respuestas      integer     NOT NULL DEFAULT 0,
  editado_en            timestamptz,
  creado_en             timestamptz NOT NULL DEFAULT now(),
  actualizado_en        timestamptz NOT NULL DEFAULT now(),
  eliminado_en          timestamptz,
  UNIQUE (id, publicacion_id),
  FOREIGN KEY (comentario_padre_id, publicacion_id) REFERENCES comentarios (id, publicacion_id)
);

ALTER TABLE publicaciones
  ADD FOREIGN KEY (comentario_aceptado_id, id) REFERENCES comentarios (id, publicacion_id);

CREATE TABLE reacciones_publicacion (
  publicacion_id       uuid        NOT NULL REFERENCES publicaciones (id),
  perfil_id            uuid        NOT NULL REFERENCES perfiles (id),
  tipo_reaccion_codigo text        NOT NULL REFERENCES tipos_reaccion (codigo),
  creado_en            timestamptz NOT NULL DEFAULT now(),
  actualizado_en       timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (publicacion_id, perfil_id)
);

CREATE TABLE reacciones_comentario (
  comentario_id        uuid        NOT NULL REFERENCES comentarios (id),
  perfil_id            uuid        NOT NULL REFERENCES perfiles (id),
  tipo_reaccion_codigo text        NOT NULL REFERENCES tipos_reaccion (codigo),
  creado_en            timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (comentario_id, perfil_id)
);

CREATE TABLE guardados (
  id             uuid        PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_id     uuid        NOT NULL REFERENCES usuarios (id),
  publicacion_id uuid        REFERENCES publicaciones (id),
  producto_id    uuid        REFERENCES productos (id),
  creado_en      timestamptz NOT NULL DEFAULT now()
);


CREATE TABLE solicitudes_cotizacion (
  id                     uuid             PRIMARY KEY DEFAULT gen_random_uuid(),
  numero                 bigint           GENERATED ALWAYS AS IDENTITY UNIQUE,
  emprendimiento_id      uuid             NOT NULL REFERENCES emprendimientos (id),
  perfil_solicitante_id  uuid             NOT NULL REFERENCES perfiles (id),
  creado_por_usuario_id  uuid             NOT NULL REFERENCES usuarios (id),
  conversacion_id        uuid,
  titulo                 text             NOT NULL,
  mensaje                text,
  fecha_deseada          date,
  ciudad_entrega_id      integer          REFERENCES ciudades (id),
  estado                 estado_solicitud NOT NULL DEFAULT 'pendiente',
  cotizacion_aceptada_id uuid,
  creado_en              timestamptz      NOT NULL DEFAULT now(),
  actualizado_en         timestamptz      NOT NULL DEFAULT now(),
  cerrado_en             timestamptz
);

CREATE TABLE solicitud_items (
  id                      uuid          PRIMARY KEY DEFAULT gen_random_uuid(),
  solicitud_cotizacion_id uuid          NOT NULL REFERENCES solicitudes_cotizacion (id),
  producto_id             uuid          REFERENCES productos (id),
  descripcion             text          NOT NULL,
  cantidad                numeric(12,2) NOT NULL DEFAULT 1,
  notas                   text
);

CREATE TABLE cotizaciones (
  id                      uuid              PRIMARY KEY DEFAULT gen_random_uuid(),
  solicitud_cotizacion_id uuid              NOT NULL REFERENCES solicitudes_cotizacion (id),
  version                 smallint          NOT NULL,
  creado_por_usuario_id   uuid              NOT NULL REFERENCES usuarios (id),
  moneda_codigo           char(3)           NOT NULL DEFAULT 'COP' REFERENCES monedas (codigo),
  subtotal                numeric(14,2)     NOT NULL DEFAULT 0,
  descuento               numeric(14,2)     NOT NULL DEFAULT 0,
  impuesto                numeric(14,2)     NOT NULL DEFAULT 0,
  total                   numeric(14,2)     GENERATED ALWAYS AS (subtotal - descuento + impuesto) STORED,
  valido_hasta            date,
  notas                   text,
  estado                  estado_cotizacion NOT NULL DEFAULT 'enviado',
  creado_en               timestamptz       NOT NULL DEFAULT now(),
  UNIQUE (id, solicitud_cotizacion_id)
);

CREATE TABLE cotizacion_items (
  id              uuid          PRIMARY KEY DEFAULT gen_random_uuid(),
  cotizacion_id   uuid          NOT NULL REFERENCES cotizaciones (id),
  producto_id     uuid          REFERENCES productos (id),
  descripcion     text          NOT NULL,
  cantidad        numeric(12,2) NOT NULL,
  precio_unitario numeric(14,2) NOT NULL,
  total_linea     numeric(14,2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED
);

CREATE TABLE historial_solicitudes (
  id                      bigint           GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  solicitud_cotizacion_id uuid             NOT NULL REFERENCES solicitudes_cotizacion (id),
  estado_anterior         estado_solicitud,
  estado_nuevo            estado_solicitud NOT NULL,
  usuario_actor_id        uuid             REFERENCES usuarios (id),
  nota                    text,
  creado_en               timestamptz      NOT NULL DEFAULT now()
);

ALTER TABLE solicitudes_cotizacion
  ADD FOREIGN KEY (cotizacion_aceptada_id, id) REFERENCES cotizaciones (id, solicitud_cotizacion_id);


CREATE TABLE conversaciones (
  id                    uuid              PRIMARY KEY DEFAULT gen_random_uuid(),
  tipo                  tipo_conversacion NOT NULL DEFAULT 'directa',
  titulo                text,
  clave_directa         text              UNIQUE,
  creado_por_usuario_id uuid              NOT NULL REFERENCES usuarios (id),
  ultimo_mensaje_id     uuid,
  ultimo_mensaje_en     timestamptz,
  creado_en             timestamptz       NOT NULL DEFAULT now(),
  actualizado_en        timestamptz       NOT NULL DEFAULT now()
);

CREATE TABLE participantes (
  conversacion_id         uuid             NOT NULL REFERENCES conversaciones (id),
  perfil_id               uuid             NOT NULL REFERENCES perfiles (id),
  rol                     rol_participante NOT NULL DEFAULT 'miembro',
  usuario_asignado_id     uuid             REFERENCES usuarios (id),
  unido_en                timestamptz      NOT NULL DEFAULT now(),
  salio_en                timestamptz,
  ultimo_mensaje_leido_id uuid,
  ultima_lectura_en       timestamptz,
  silenciado_hasta        timestamptz,
  archivado_en            timestamptz,
  PRIMARY KEY (conversacion_id, perfil_id)
);

CREATE TABLE mensajes (
  id                      uuid         PRIMARY KEY DEFAULT gen_random_uuid(),
  conversacion_id         uuid         NOT NULL,
  perfil_remitente_id     uuid         NOT NULL,
  usuario_remitente_id    uuid         REFERENCES usuarios (id),
  tipo                    tipo_mensaje NOT NULL DEFAULT 'texto',
  contenido               text,
  producto_id             uuid         REFERENCES productos (id),
  solicitud_cotizacion_id uuid         REFERENCES solicitudes_cotizacion (id),
  responde_a_mensaje_id   uuid         REFERENCES mensajes (id),
  creado_en               timestamptz  NOT NULL DEFAULT now(),
  editado_en              timestamptz,
  eliminado_en            timestamptz,
  FOREIGN KEY (conversacion_id, perfil_remitente_id) REFERENCES participantes (conversacion_id, perfil_id)
);

CREATE TABLE mensajes_adjuntos (
  mensaje_id uuid     NOT NULL REFERENCES mensajes (id),
  archivo_id uuid     NOT NULL REFERENCES archivos (id),
  posicion   smallint NOT NULL,
  PRIMARY KEY (mensaje_id, archivo_id)
);

ALTER TABLE conversaciones
  ADD FOREIGN KEY (ultimo_mensaje_id) REFERENCES mensajes (id);

ALTER TABLE participantes
  ADD FOREIGN KEY (ultimo_mensaje_leido_id) REFERENCES mensajes (id);

ALTER TABLE solicitudes_cotizacion
  ADD FOREIGN KEY (conversacion_id) REFERENCES conversaciones (id);


CREATE TABLE resenas (
  id                        uuid          PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_resenador_id      uuid          NOT NULL REFERENCES usuarios (id),
  emprendimiento_id         uuid          NOT NULL REFERENCES emprendimientos (id),
  producto_id               uuid,
  solicitud_cotizacion_id   uuid          REFERENCES solicitudes_cotizacion (id),
  calificacion              smallint      NOT NULL,
  titulo                    text,
  contenido                 text,
  estado                    estado_resena NOT NULL DEFAULT 'publicado',
  respuesta_emprendimiento  text,
  respondido_por_usuario_id uuid          REFERENCES usuarios (id),
  respondido_en             timestamptz,
  creado_en                 timestamptz   NOT NULL DEFAULT now(),
  actualizado_en            timestamptz   NOT NULL DEFAULT now(),
  eliminado_en              timestamptz,
  FOREIGN KEY (producto_id, emprendimiento_id) REFERENCES productos (id, emprendimiento_id)
);

CREATE TABLE resenas_archivos (
  resena_id  uuid     NOT NULL REFERENCES resenas (id),
  archivo_id uuid     NOT NULL REFERENCES archivos (id),
  posicion   smallint NOT NULL,
  PRIMARY KEY (resena_id, archivo_id)
);


CREATE TABLE notificaciones (
  id                      uuid              PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_destinatario_id uuid              NOT NULL REFERENCES usuarios (id),
  tipo                    tipo_notificacion NOT NULL,
  perfil_actor_id         uuid              REFERENCES perfiles (id),
  entidad_tipo            text,
  entidad_id              uuid,
  clave_grupo             text,
  datos                   jsonb             NOT NULL DEFAULT '{}',
  leido_en                timestamptz,
  creado_en               timestamptz       NOT NULL DEFAULT now()
);

CREATE TABLE reportes (
  id                      uuid             PRIMARY KEY DEFAULT gen_random_uuid(),
  usuario_reportante_id   uuid             NOT NULL REFERENCES usuarios (id),
  objetivo_tipo           objetivo_reporte NOT NULL,
  objetivo_id             uuid             NOT NULL,
  motivo_codigo           text             NOT NULL REFERENCES motivos_reporte (codigo),
  detalles                text,
  estado                  estado_reporte   NOT NULL DEFAULT 'abierto',
  resuelto_por_usuario_id uuid             REFERENCES usuarios (id),
  nota_resolucion         text,
  creado_en               timestamptz      NOT NULL DEFAULT now(),
  resuelto_en             timestamptz
);

COMMIT;
