CREATE TABLE categorias (
    id integer GENERATED ALWAYS AS IDENTITY,
    padre_id integer,
    slug text NOT NULL,
    nombre text NOT NULL,
    icono text,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_categorias PRIMARY KEY (id),
    CONSTRAINT ck_categorias_icono_longitud CHECK (char_length("icono") <= 255),
    CONSTRAINT ck_categorias_nombre_longitud CHECK (char_length("nombre") <= 255),
    CONSTRAINT ck_categorias_slug_longitud CHECK (char_length("slug") <= 255),
    CONSTRAINT fk_categorias_padre_id_categorias FOREIGN KEY (padre_id) REFERENCES categorias (id) ON DELETE RESTRICT
);


CREATE TABLE etiquetas (
    id integer GENERATED ALWAYS AS IDENTITY,
    nombre text NOT NULL,
    total_usos integer NOT NULL DEFAULT 0,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_etiquetas PRIMARY KEY (id),
    CONSTRAINT ck_etiquetas_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE monedas (
    codigo character(3) NOT NULL,
    nombre text NOT NULL,
    simbolo text NOT NULL,
    decimales smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_monedas PRIMARY KEY (codigo),
    CONSTRAINT ck_monedas_nombre_longitud CHECK (char_length("nombre") <= 255),
    CONSTRAINT ck_monedas_simbolo_longitud CHECK (char_length("simbolo") <= 255)
);


CREATE TABLE motivos_reporte (
    codigo text NOT NULL,
    nombre text NOT NULL,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_motivos_reporte PRIMARY KEY (codigo),
    CONSTRAINT ck_motivos_reporte_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_motivos_reporte_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE permisos_rol (
    rol text NOT NULL,
    permiso text NOT NULL,
    CONSTRAINT pk_permisos_rol PRIMARY KEY (rol, permiso),
    CONSTRAINT ck_permisos_rol_permiso_longitud CHECK (char_length("permiso") <= 64),
    CONSTRAINT ck_permisos_rol_rol_enum CHECK ("rol" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro'))
);


CREATE TABLE tipos_oportunidad (
    codigo text NOT NULL,
    nombre text NOT NULL,
    descripcion text,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_tipos_oportunidad PRIMARY KEY (codigo),
    CONSTRAINT ck_tipos_oportunidad_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_tipos_oportunidad_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_tipos_oportunidad_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE tipos_persona (
    codigo text NOT NULL,
    nombre text NOT NULL,
    descripcion text,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_tipos_persona PRIMARY KEY (codigo),
    CONSTRAINT ck_tipos_persona_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_tipos_persona_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_tipos_persona_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE tipos_publicacion (
    codigo text NOT NULL,
    nombre text NOT NULL,
    descripcion text,
    icono text,
    peso_descubrimiento numeric(4,2) NOT NULL DEFAULT 1.0,
    permite_respuesta_aceptada boolean NOT NULL DEFAULT FALSE,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_tipos_publicacion PRIMARY KEY (codigo),
    CONSTRAINT ck_tipos_publicacion_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_tipos_publicacion_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_tipos_publicacion_icono_longitud CHECK (char_length("icono") <= 255),
    CONSTRAINT ck_tipos_publicacion_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE tipos_reaccion (
    codigo text NOT NULL,
    nombre text NOT NULL,
    emoji text NOT NULL,
    orden smallint NOT NULL DEFAULT 0,
    activo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT pk_tipos_reaccion PRIMARY KEY (codigo),
    CONSTRAINT ck_tipos_reaccion_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_tipos_reaccion_emoji_longitud CHECK (char_length("emoji") <= 32),
    CONSTRAINT ck_tipos_reaccion_nombre_longitud CHECK (char_length("nombre") <= 255)
);


CREATE TABLE usuarios_reservados (
    nombre_usuario text NOT NULL,
    CONSTRAINT pk_usuarios_reservados PRIMARY KEY (nombre_usuario),
    CONSTRAINT ck_usuarios_reservados_nombre_usuario_longitud CHECK (char_length("nombre_usuario") <= 50)
);


CREATE TABLE paises (
    codigo character(2) NOT NULL,
    nombre text NOT NULL,
    moneda_defecto_codigo character(3) NOT NULL,
    prefijo_telefono text NOT NULL,
    CONSTRAINT pk_paises PRIMARY KEY (codigo),
    CONSTRAINT ck_paises_nombre_longitud CHECK (char_length("nombre") <= 255),
    CONSTRAINT ck_paises_prefijo_telefono_longitud CHECK (char_length("prefijo_telefono") <= 30),
    CONSTRAINT fk_paises_moneda_defecto_codigo_monedas FOREIGN KEY (moneda_defecto_codigo) REFERENCES monedas (codigo) ON DELETE RESTRICT
);


CREATE TABLE departamentos (
    id integer GENERATED ALWAYS AS IDENTITY,
    pais_codigo character(2) NOT NULL,
    nombre text NOT NULL,
    codigo text,
    CONSTRAINT pk_departamentos PRIMARY KEY (id),
    CONSTRAINT ck_departamentos_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_departamentos_nombre_longitud CHECK (char_length("nombre") <= 255),
    CONSTRAINT fk_departamentos_pais_codigo_paises FOREIGN KEY (pais_codigo) REFERENCES paises (codigo) ON DELETE RESTRICT
);


CREATE TABLE ciudades (
    id integer GENERATED ALWAYS AS IDENTITY,
    departamento_id integer NOT NULL,
    nombre text NOT NULL,
    codigo text,
    latitud numeric(9,6) NOT NULL,
    longitud numeric(9,6) NOT NULL,
    CONSTRAINT pk_ciudades PRIMARY KEY (id),
    CONSTRAINT ck_ciudades_codigo_longitud CHECK (char_length("codigo") <= 64),
    CONSTRAINT ck_ciudades_nombre_longitud CHECK (char_length("nombre") <= 255),
    CONSTRAINT fk_ciudades_departamento_id_departamentos FOREIGN KEY (departamento_id) REFERENCES departamentos (id) ON DELETE RESTRICT
);


CREATE TABLE archivos (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    subido_por_usuario_id uuid NOT NULL,
    tipo text NOT NULL,
    proposito text NOT NULL,
    proveedor_almacenamiento text NOT NULL DEFAULT 'r2',
    bucket text NOT NULL,
    clave_objeto text NOT NULL,
    nombre_original text,
    tipo_mime text NOT NULL,
    extension text NOT NULL,
    tamano_bytes bigint NOT NULL,
    checksum_sha256 character(64),
    ancho integer,
    alto integer,
    duracion_segundos numeric(8,2),
    visibilidad text NOT NULL DEFAULT 'publico',
    estado text NOT NULL DEFAULT 'pendiente',
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_archivos PRIMARY KEY (id),
    CONSTRAINT ck_archivos_bucket_longitud CHECK (char_length("bucket") <= 255),
    CONSTRAINT ck_archivos_clave_objeto_longitud CHECK (char_length("clave_objeto") <= 1024),
    CONSTRAINT ck_archivos_estado_enum CHECK ("estado" IN ('pendiente', 'listo', 'fallido', 'eliminado')),
    CONSTRAINT ck_archivos_extension_longitud CHECK (char_length("extension") <= 16),
    CONSTRAINT ck_archivos_nombre_original_longitud CHECK (char_length("nombre_original") <= 255),
    CONSTRAINT ck_archivos_proposito_enum CHECK ("proposito" IN ('foto_perfil', 'portada', 'publicacion', 'producto', 'mensaje', 'resena', 'verificacion', 'otro')),
    CONSTRAINT ck_archivos_proveedor_almacenamiento_longitud CHECK (char_length("proveedor_almacenamiento") <= 255),
    CONSTRAINT ck_archivos_tipo_enum CHECK ("tipo" IN ('imagen', 'video', 'documento')),
    CONSTRAINT ck_archivos_tipo_mime_longitud CHECK (char_length("tipo_mime") <= 127),
    CONSTRAINT ck_archivos_visibilidad_enum CHECK ("visibilidad" IN ('publico', 'privado'))
);


CREATE TABLE perfiles (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    tipo text NOT NULL,
    nombre_usuario text NOT NULL,
    nombre_visible text NOT NULL,
    titular text,
    biografia text,
    foto_archivo_id uuid,
    portada_archivo_id uuid,
    ciudad_id integer,
    sitio_web text,
    es_privado boolean NOT NULL DEFAULT FALSE,
    estado text NOT NULL DEFAULT 'activo',
    total_seguidores integer NOT NULL DEFAULT 0,
    total_seguidos integer NOT NULL DEFAULT 0,
    total_publicaciones integer NOT NULL DEFAULT 0,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_perfiles PRIMARY KEY (id),
    CONSTRAINT "AK_perfiles_id_tipo" UNIQUE (id, tipo),
    CONSTRAINT ck_perfiles_biografia_longitud CHECK (char_length("biografia") <= 2000),
    CONSTRAINT ck_perfiles_estado_enum CHECK ("estado" IN ('activo', 'suspendido', 'eliminado')),
    CONSTRAINT ck_perfiles_nombre_usuario_longitud CHECK (char_length("nombre_usuario") <= 50),
    CONSTRAINT ck_perfiles_nombre_visible_longitud CHECK (char_length("nombre_visible") <= 255),
    CONSTRAINT ck_perfiles_sitio_web_longitud CHECK (char_length("sitio_web") <= 2048),
    CONSTRAINT ck_perfiles_tipo_enum CHECK ("tipo" IN ('persona', 'emprendimiento')),
    CONSTRAINT ck_perfiles_titular_longitud CHECK (char_length("titular") <= 255),
    CONSTRAINT fk_perfiles_ciudad_id_ciudades FOREIGN KEY (ciudad_id) REFERENCES ciudades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_perfiles_foto_archivo_id_archivos FOREIGN KEY (foto_archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_perfiles_portada_archivo_id_archivos FOREIGN KEY (portada_archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT
);


CREATE TABLE variantes_archivo (
    archivo_id uuid NOT NULL,
    variante text NOT NULL,
    clave_objeto text NOT NULL,
    tipo_mime text NOT NULL,
    ancho integer,
    alto integer,
    tamano_bytes bigint NOT NULL,
    CONSTRAINT pk_variantes_archivo PRIMARY KEY (archivo_id, variante),
    CONSTRAINT ck_variantes_archivo_clave_objeto_longitud CHECK (char_length("clave_objeto") <= 1024),
    CONSTRAINT ck_variantes_archivo_tipo_mime_longitud CHECK (char_length("tipo_mime") <= 127),
    CONSTRAINT ck_variantes_archivo_variante_longitud CHECK (char_length("variante") <= 64),
    CONSTRAINT fk_variantes_archivo_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE CASCADE
);


CREATE TABLE usuarios (
    id uuid NOT NULL,
    tipo_perfil text NOT NULL DEFAULT 'persona',
    correo text NOT NULL,
    hash_contrasena text,
    rol_plataforma text NOT NULL DEFAULT 'usuario',
    correo_verificado_en timestamptz,
    terminos_aceptados_en timestamptz NOT NULL,
    ultimo_ingreso_en timestamptz,
    intentos_fallidos smallint NOT NULL DEFAULT 0,
    bloqueado_hasta timestamptz,
    contrasena_cambiada_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_usuarios PRIMARY KEY (id),
    CONSTRAINT ck_usuarios_correo_longitud CHECK (char_length("correo") <= 254),
    CONSTRAINT ck_usuarios_hash_contrasena_longitud CHECK (char_length("hash_contrasena") <= 512),
    CONSTRAINT ck_usuarios_rol_plataforma_enum CHECK ("rol_plataforma" IN ('usuario', 'moderador', 'administrador')),
    CONSTRAINT ck_usuarios_tipo_perfil CHECK ("tipo_perfil" = 'persona'),
    CONSTRAINT ck_usuarios_tipo_perfil_enum CHECK ("tipo_perfil" IN ('persona', 'emprendimiento')),
    CONSTRAINT fk_usuarios_id_tipo_perfil_perfiles FOREIGN KEY (id, tipo_perfil) REFERENCES perfiles (id, tipo) ON DELETE RESTRICT
);


CREATE TABLE bloqueos (
    usuario_bloqueador_id uuid NOT NULL,
    perfil_bloqueado_id uuid NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_bloqueos PRIMARY KEY (usuario_bloqueador_id, perfil_bloqueado_id),
    CONSTRAINT fk_bloqueos_perfil_bloqueado_id_perfiles FOREIGN KEY (perfil_bloqueado_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_bloqueos_usuario_bloqueador_id_usuarios FOREIGN KEY (usuario_bloqueador_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE emprendimientos (
    id uuid NOT NULL,
    tipo_perfil text NOT NULL DEFAULT 'emprendimiento',
    categoria_id integer NOT NULL,
    etapa text NOT NULL DEFAULT 'idea',
    fundado_en date,
    razon_social text,
    nit text,
    correo_contacto text,
    telefono_contacto text,
    whatsapp text,
    direccion text,
    envios_nacionales boolean NOT NULL DEFAULT FALSE,
    ofrece_remoto boolean NOT NULL DEFAULT FALSE,
    estado_verificacion text NOT NULL DEFAULT 'sin_verificar',
    verificado_en timestamptz,
    calificacion_promedio numeric(3,2),
    total_calificaciones integer NOT NULL DEFAULT 0,
    creado_por_usuario_id uuid NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_emprendimientos PRIMARY KEY (id),
    CONSTRAINT ck_emprendimientos_correo_contacto_longitud CHECK (char_length("correo_contacto") <= 254),
    CONSTRAINT ck_emprendimientos_direccion_longitud CHECK (char_length("direccion") <= 500),
    CONSTRAINT ck_emprendimientos_estado_verificacion_enum CHECK ("estado_verificacion" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')),
    CONSTRAINT ck_emprendimientos_etapa_enum CHECK ("etapa" IN ('idea', 'validacion', 'lanzamiento', 'crecimiento', 'consolidado')),
    CONSTRAINT ck_emprendimientos_nit_longitud CHECK (char_length("nit") <= 255),
    CONSTRAINT ck_emprendimientos_razon_social_longitud CHECK (char_length("razon_social") <= 255),
    CONSTRAINT ck_emprendimientos_telefono_contacto_longitud CHECK (char_length("telefono_contacto") <= 30),
    CONSTRAINT ck_emprendimientos_tipo_perfil CHECK ("tipo_perfil" = 'emprendimiento'),
    CONSTRAINT ck_emprendimientos_tipo_perfil_enum CHECK ("tipo_perfil" IN ('persona', 'emprendimiento')),
    CONSTRAINT ck_emprendimientos_whatsapp_longitud CHECK (char_length("whatsapp") <= 30),
    CONSTRAINT fk_emprendimientos_categoria_id_categorias FOREIGN KEY (categoria_id) REFERENCES categorias (id) ON DELETE RESTRICT,
    CONSTRAINT fk_emprendimientos_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_emprendimientos_id_tipo_perfil_perfiles FOREIGN KEY (id, tipo_perfil) REFERENCES perfiles (id, tipo) ON DELETE RESTRICT
);


CREATE TABLE intereses_usuario (
    usuario_id uuid NOT NULL,
    categoria_id integer NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_intereses_usuario PRIMARY KEY (usuario_id, categoria_id),
    CONSTRAINT fk_intereses_usuario_categoria_id_categorias FOREIGN KEY (categoria_id) REFERENCES categorias (id) ON DELETE RESTRICT,
    CONSTRAINT fk_intereses_usuario_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE CASCADE
);


CREATE TABLE notificaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_destinatario_id uuid NOT NULL,
    tipo text NOT NULL,
    perfil_actor_id uuid,
    entidad_tipo text,
    entidad_id uuid,
    clave_grupo text,
    datos jsonb NOT NULL DEFAULT ('{}'::jsonb),
    leido_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_notificaciones PRIMARY KEY (id),
    CONSTRAINT ck_notificaciones_clave_grupo_longitud CHECK (char_length("clave_grupo") <= 1024),
    CONSTRAINT ck_notificaciones_entidad_tipo_longitud CHECK (char_length("entidad_tipo") <= 64),
    CONSTRAINT ck_notificaciones_tipo_enum CHECK ("tipo" IN ('seguimiento', 'solicitud_seguimiento', 'seguimiento_aceptado', 'reaccion', 'comentario', 'respuesta', 'mencion', 'compartido', 'mensaje', 'cotizacion_actualizada', 'postulacion_actualizada', 'mentoria_actualizada', 'resena', 'invitacion_emprendimiento', 'respuesta_aceptada', 'sistema')),
    CONSTRAINT fk_notificaciones_perfil_actor_id_perfiles FOREIGN KEY (perfil_actor_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_notificaciones_usuario_destinatario_id_usuarios FOREIGN KEY (usuario_destinatario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE ofertas_mentoria (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_mentor_id uuid NOT NULL,
    titulo text NOT NULL,
    descripcion text NOT NULL,
    categoria_id integer,
    modalidad text NOT NULL DEFAULT 'virtual',
    duracion_minutos smallint NOT NULL DEFAULT 60,
    es_gratis boolean NOT NULL DEFAULT TRUE,
    precio numeric(14,2),
    moneda_codigo character(3) NOT NULL DEFAULT 'COP',
    activo boolean NOT NULL DEFAULT TRUE,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_ofertas_mentoria PRIMARY KEY (id),
    CONSTRAINT ck_ofertas_mentoria_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_ofertas_mentoria_modalidad_enum CHECK ("modalidad" IN ('virtual', 'presencial', 'ambas')),
    CONSTRAINT ck_ofertas_mentoria_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_ofertas_mentoria_categoria_id_categorias FOREIGN KEY (categoria_id) REFERENCES categorias (id) ON DELETE RESTRICT,
    CONSTRAINT fk_ofertas_mentoria_moneda_codigo_monedas FOREIGN KEY (moneda_codigo) REFERENCES monedas (codigo) ON DELETE RESTRICT,
    CONSTRAINT fk_ofertas_mentoria_usuario_mentor_id_usuarios FOREIGN KEY (usuario_mentor_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE oportunidades (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    perfil_autor_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    tipo_codigo text NOT NULL,
    titulo text NOT NULL,
    descripcion text NOT NULL,
    categoria_id integer,
    ciudad_id integer,
    es_remoto boolean NOT NULL DEFAULT FALSE,
    estado text NOT NULL DEFAULT 'abierto',
    expira_en timestamptz,
    total_postulaciones integer NOT NULL DEFAULT 0,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_oportunidades PRIMARY KEY (id),
    CONSTRAINT ck_oportunidades_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_oportunidades_estado_enum CHECK ("estado" IN ('abierto', 'cerrado', 'cubierto')),
    CONSTRAINT ck_oportunidades_tipo_codigo_longitud CHECK (char_length("tipo_codigo") <= 64),
    CONSTRAINT ck_oportunidades_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_oportunidades_categoria_id_categorias FOREIGN KEY (categoria_id) REFERENCES categorias (id) ON DELETE RESTRICT,
    CONSTRAINT fk_oportunidades_ciudad_id_ciudades FOREIGN KEY (ciudad_id) REFERENCES ciudades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_oportunidades_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_oportunidades_perfil_autor_id_perfiles FOREIGN KEY (perfil_autor_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_oportunidades_tipo_codigo_tipos_oportunidad FOREIGN KEY (tipo_codigo) REFERENCES tipos_oportunidad (codigo) ON DELETE RESTRICT
);


CREATE TABLE reportes (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_reportante_id uuid NOT NULL,
    objetivo_tipo text NOT NULL,
    objetivo_id uuid NOT NULL,
    motivo_codigo text NOT NULL,
    detalles text,
    estado text NOT NULL DEFAULT 'abierto',
    resuelto_por_usuario_id uuid,
    nota_resolucion text,
    resuelto_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_reportes PRIMARY KEY (id),
    CONSTRAINT ck_reportes_detalles_longitud CHECK (char_length("detalles") <= 4000),
    CONSTRAINT ck_reportes_estado_enum CHECK ("estado" IN ('abierto', 'en_revision', 'resuelto', 'descartado')),
    CONSTRAINT ck_reportes_motivo_codigo_longitud CHECK (char_length("motivo_codigo") <= 64),
    CONSTRAINT ck_reportes_nota_resolucion_longitud CHECK (char_length("nota_resolucion") <= 4000),
    CONSTRAINT ck_reportes_objetivo_tipo_enum CHECK ("objetivo_tipo" IN ('perfil', 'publicacion', 'comentario', 'producto', 'mensaje', 'resena', 'oportunidad')),
    CONSTRAINT fk_reportes_motivo_codigo_motivos_reporte FOREIGN KEY (motivo_codigo) REFERENCES motivos_reporte (codigo) ON DELETE RESTRICT,
    CONSTRAINT fk_reportes_resuelto_por_usuario_id_usuarios FOREIGN KEY (resuelto_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_reportes_usuario_reportante_id_usuarios FOREIGN KEY (usuario_reportante_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE seguimientos (
    usuario_seguidor_id uuid NOT NULL,
    perfil_seguido_id uuid NOT NULL,
    estado text NOT NULL DEFAULT 'activo',
    creado_en timestamptz NOT NULL DEFAULT (now()),
    aceptado_en timestamptz,
    CONSTRAINT pk_seguimientos PRIMARY KEY (usuario_seguidor_id, perfil_seguido_id),
    CONSTRAINT ck_seguimientos_estado_enum CHECK ("estado" IN ('pendiente', 'activo')),
    CONSTRAINT fk_seguimientos_perfil_seguido_id_perfiles FOREIGN KEY (perfil_seguido_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_seguimientos_usuario_seguidor_id_usuarios FOREIGN KEY (usuario_seguidor_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE sesiones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_id uuid NOT NULL,
    hash_token_refresco text NOT NULL,
    familia_id uuid NOT NULL,
    reemplazado_por_id uuid,
    agente_usuario text,
    direccion_ip inet,
    ultimo_uso_en timestamptz NOT NULL DEFAULT (now()),
    expira_en timestamptz NOT NULL,
    revocado_en timestamptz,
    motivo_revocacion text,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_sesiones PRIMARY KEY (id),
    CONSTRAINT ck_sesiones_agente_usuario_longitud CHECK (char_length("agente_usuario") <= 255),
    CONSTRAINT ck_sesiones_hash_token_refresco_longitud CHECK (char_length("hash_token_refresco") <= 256),
    CONSTRAINT ck_sesiones_motivo_revocacion_longitud CHECK (char_length("motivo_revocacion") <= 255),
    CONSTRAINT fk_sesiones_reemplazado_por_id_sesiones FOREIGN KEY (reemplazado_por_id) REFERENCES sesiones (id) ON DELETE RESTRICT,
    CONSTRAINT fk_sesiones_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE tokens_recuperacion (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_id uuid NOT NULL,
    hash_token text NOT NULL,
    ip_solicitud inet,
    expira_en timestamptz NOT NULL,
    usado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_tokens_recuperacion PRIMARY KEY (id),
    CONSTRAINT ck_tokens_recuperacion_hash_token_longitud CHECK (char_length("hash_token") <= 256),
    CONSTRAINT fk_tokens_recuperacion_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE usuario_tipos_persona (
    usuario_id uuid NOT NULL,
    tipo_persona_codigo text NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_usuario_tipos_persona PRIMARY KEY (usuario_id, tipo_persona_codigo),
    CONSTRAINT ck_usuario_tipos_persona_tipo_persona_codigo_longitud CHECK (char_length("tipo_persona_codigo") <= 64),
    CONSTRAINT fk_usuario_tipos_persona_tipo_persona_codigo_tipos_persona FOREIGN KEY (tipo_persona_codigo) REFERENCES tipos_persona (codigo) ON DELETE RESTRICT,
    CONSTRAINT fk_usuario_tipos_persona_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE CASCADE
);


CREATE TABLE enlaces_emprendimiento (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    emprendimiento_id uuid NOT NULL,
    plataforma text NOT NULL,
    url text NOT NULL,
    orden smallint NOT NULL DEFAULT 0,
    CONSTRAINT pk_enlaces_emprendimiento PRIMARY KEY (id),
    CONSTRAINT ck_enlaces_emprendimiento_plataforma_longitud CHECK (char_length("plataforma") <= 64),
    CONSTRAINT ck_enlaces_emprendimiento_url_longitud CHECK (char_length("url") <= 2048),
    CONSTRAINT fk_enlaces_emprendimiento_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE CASCADE
);


CREATE TABLE miembros_emprendimiento (
    emprendimiento_id uuid NOT NULL,
    usuario_id uuid NOT NULL,
    rol text NOT NULL DEFAULT 'miembro',
    titulo text,
    estado text NOT NULL DEFAULT 'invitado',
    invitado_por_usuario_id uuid,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    unido_en timestamptz,
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_miembros_emprendimiento PRIMARY KEY (emprendimiento_id, usuario_id),
    CONSTRAINT ck_miembros_emprendimiento_estado_enum CHECK ("estado" IN ('invitado', 'activo', 'retirado', 'expulsado')),
    CONSTRAINT ck_miembros_emprendimiento_rol_enum CHECK ("rol" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro')),
    CONSTRAINT ck_miembros_emprendimiento_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_miembros_emprendimiento_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_miembros_emprendimiento_invitado_por_usuario_id_usuarios FOREIGN KEY (invitado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_miembros_emprendimiento_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE productos (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    emprendimiento_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    tipo text NOT NULL,
    titulo text NOT NULL,
    slug text NOT NULL,
    descripcion text,
    categoria_id integer NOT NULL,
    tipo_precio text NOT NULL DEFAULT 'fijo',
    precio numeric(14,2),
    moneda_codigo character(3) NOT NULL DEFAULT 'COP',
    estado_inventario text NOT NULL DEFAULT 'disponible',
    cantidad_inventario integer,
    ciudad_id integer,
    estado text NOT NULL DEFAULT 'borrador',
    calificacion_promedio numeric(3,2),
    total_calificaciones integer NOT NULL DEFAULT 0,
    total_guardados integer NOT NULL DEFAULT 0,
    publicado_en timestamptz,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_productos PRIMARY KEY (id),
    CONSTRAINT "AK_productos_id_emprendimiento_id" UNIQUE (id, emprendimiento_id),
    CONSTRAINT ck_productos_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_productos_estado_enum CHECK ("estado" IN ('borrador', 'activo', 'pausado', 'archivado')),
    CONSTRAINT ck_productos_estado_inventario_enum CHECK ("estado_inventario" IN ('disponible', 'agotado', 'bajo_pedido', 'no_aplica')),
    CONSTRAINT ck_productos_slug_longitud CHECK (char_length("slug") <= 255),
    CONSTRAINT ck_productos_tipo_enum CHECK ("tipo" IN ('producto', 'servicio')),
    CONSTRAINT ck_productos_tipo_precio_enum CHECK ("tipo_precio" IN ('fijo', 'desde', 'a_convenir', 'gratis')),
    CONSTRAINT ck_productos_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_productos_categoria_id_categorias FOREIGN KEY (categoria_id) REFERENCES categorias (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_ciudad_id_ciudades FOREIGN KEY (ciudad_id) REFERENCES ciudades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_moneda_codigo_monedas FOREIGN KEY (moneda_codigo) REFERENCES monedas (codigo) ON DELETE RESTRICT
);


CREATE TABLE verificaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    emprendimiento_id uuid NOT NULL,
    estado text NOT NULL DEFAULT 'pendiente',
    metodo text NOT NULL DEFAULT 'documentos',
    enviado_por_usuario_id uuid NOT NULL,
    revisado_por_usuario_id uuid,
    notas_revisor text,
    enviado_en timestamptz NOT NULL DEFAULT (now()),
    revisado_en timestamptz,
    CONSTRAINT pk_verificaciones PRIMARY KEY (id),
    CONSTRAINT ck_verificaciones_estado_enum CHECK ("estado" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')),
    CONSTRAINT ck_verificaciones_metodo_longitud CHECK (char_length("metodo") <= 64),
    CONSTRAINT ck_verificaciones_notas_revisor_longitud CHECK (char_length("notas_revisor") <= 4000),
    CONSTRAINT fk_verificaciones_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_verificaciones_enviado_por_usuario_id_usuarios FOREIGN KEY (enviado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_verificaciones_revisado_por_usuario_id_usuarios FOREIGN KEY (revisado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE solicitudes_mentoria (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    oferta_id uuid NOT NULL,
    perfil_mentoreado_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    mensaje text NOT NULL,
    estado text NOT NULL DEFAULT 'solicitado',
    agendado_en timestamptz,
    url_reunion text,
    calificacion_mentoreado smallint,
    comentario_mentoreado text,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_solicitudes_mentoria PRIMARY KEY (id),
    CONSTRAINT ck_solicitudes_mentoria_calificacion_mentoreado CHECK ("calificacion_mentoreado" BETWEEN 1 AND 5),
    CONSTRAINT ck_solicitudes_mentoria_comentario_mentoreado_longitud CHECK (char_length("comentario_mentoreado") <= 4000),
    CONSTRAINT ck_solicitudes_mentoria_estado_enum CHECK ("estado" IN ('solicitado', 'aceptado', 'rechazado', 'agendado', 'completado', 'cancelado')),
    CONSTRAINT ck_solicitudes_mentoria_mensaje_longitud CHECK (char_length("mensaje") <= 10000),
    CONSTRAINT ck_solicitudes_mentoria_url_reunion_longitud CHECK (char_length("url_reunion") <= 2048),
    CONSTRAINT fk_solicitudes_mentoria_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_mentoria_oferta_id_ofertas_mentoria FOREIGN KEY (oferta_id) REFERENCES ofertas_mentoria (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_mentoria_perfil_mentoreado_id_perfiles FOREIGN KEY (perfil_mentoreado_id) REFERENCES perfiles (id) ON DELETE RESTRICT
);


CREATE TABLE postulaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    oportunidad_id uuid NOT NULL,
    perfil_postulante_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    mensaje text NOT NULL,
    estado text NOT NULL DEFAULT 'pendiente',
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_postulaciones PRIMARY KEY (id),
    CONSTRAINT ck_postulaciones_estado_enum CHECK ("estado" IN ('pendiente', 'aceptado', 'rechazado', 'retirado')),
    CONSTRAINT ck_postulaciones_mensaje_longitud CHECK (char_length("mensaje") <= 10000),
    CONSTRAINT fk_postulaciones_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_postulaciones_oportunidad_id_oportunidades FOREIGN KEY (oportunidad_id) REFERENCES oportunidades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_postulaciones_perfil_postulante_id_perfiles FOREIGN KEY (perfil_postulante_id) REFERENCES perfiles (id) ON DELETE RESTRICT
);


CREATE TABLE productos_archivos (
    producto_id uuid NOT NULL,
    archivo_id uuid NOT NULL,
    posicion smallint NOT NULL,
    texto_alternativo text,
    CONSTRAINT pk_productos_archivos PRIMARY KEY (producto_id, archivo_id),
    CONSTRAINT ck_productos_archivos_texto_alternativo_longitud CHECK (char_length("texto_alternativo") <= 255),
    CONSTRAINT fk_productos_archivos_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_archivos_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE CASCADE
);


CREATE TABLE productos_etiquetas (
    producto_id uuid NOT NULL,
    etiqueta_id integer NOT NULL,
    CONSTRAINT pk_productos_etiquetas PRIMARY KEY (producto_id, etiqueta_id),
    CONSTRAINT fk_productos_etiquetas_etiqueta_id_etiquetas FOREIGN KEY (etiqueta_id) REFERENCES etiquetas (id) ON DELETE RESTRICT,
    CONSTRAINT fk_productos_etiquetas_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE CASCADE
);


CREATE TABLE documentos_verificacion (
    verificacion_id uuid NOT NULL,
    archivo_id uuid NOT NULL,
    tipo_documento text NOT NULL,
    CONSTRAINT pk_documentos_verificacion PRIMARY KEY (verificacion_id, archivo_id),
    CONSTRAINT ck_documentos_verificacion_tipo_documento_longitud CHECK (char_length("tipo_documento") <= 64),
    CONSTRAINT fk_documentos_verificacion_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_documentos_verificacion_verificacion_id_verificaciones FOREIGN KEY (verificacion_id) REFERENCES verificaciones (id) ON DELETE CASCADE
);


CREATE TABLE comentarios (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    publicacion_id uuid NOT NULL,
    comentario_padre_id uuid,
    perfil_autor_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    responde_a_perfil_id uuid,
    contenido text NOT NULL,
    total_reacciones integer NOT NULL DEFAULT 0,
    total_respuestas integer NOT NULL DEFAULT 0,
    editado_en timestamptz,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_comentarios PRIMARY KEY (id),
    CONSTRAINT "AK_comentarios_id_publicacion_id" UNIQUE (id, publicacion_id),
    CONSTRAINT ck_comentarios_contenido_longitud CHECK (char_length("contenido") <= 10000),
    CONSTRAINT fk_comentarios_comentario_padre_id_publicacion_id_comentarios FOREIGN KEY (comentario_padre_id, publicacion_id) REFERENCES comentarios (id, publicacion_id) ON DELETE RESTRICT,
    CONSTRAINT fk_comentarios_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_comentarios_perfil_autor_id_perfiles FOREIGN KEY (perfil_autor_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_comentarios_responde_a_perfil_id_perfiles FOREIGN KEY (responde_a_perfil_id) REFERENCES perfiles (id) ON DELETE RESTRICT
);


CREATE TABLE publicaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    perfil_autor_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    tipo_publicacion_codigo text NOT NULL DEFAULT 'general',
    contenido text NOT NULL DEFAULT '',
    visibilidad text NOT NULL DEFAULT 'publico',
    producto_id uuid,
    oportunidad_id uuid,
    publicacion_compartida_id uuid,
    comentario_aceptado_id uuid,
    comentarios_habilitados boolean NOT NULL DEFAULT TRUE,
    fijado boolean NOT NULL DEFAULT FALSE,
    total_reacciones integer NOT NULL DEFAULT 0,
    total_comentarios integer NOT NULL DEFAULT 0,
    total_compartidos integer NOT NULL DEFAULT 0,
    total_guardados integer NOT NULL DEFAULT 0,
    puntaje_descubrimiento float8 NOT NULL DEFAULT 0.0,
    puntaje_actualizado_en timestamptz,
    editado_en timestamptz,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_publicaciones PRIMARY KEY (id),
    CONSTRAINT ck_publicaciones_contenido_longitud CHECK (char_length("contenido") <= 10000),
    CONSTRAINT ck_publicaciones_tipo_publicacion_codigo_longitud CHECK (char_length("tipo_publicacion_codigo") <= 64),
    CONSTRAINT ck_publicaciones_visibilidad_enum CHECK ("visibilidad" IN ('publico', 'seguidores')),
    CONSTRAINT fk_publicaciones_comentario_aceptado_id_id_comentarios FOREIGN KEY (comentario_aceptado_id, id) REFERENCES comentarios (id, publicacion_id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_oportunidad_id_oportunidades FOREIGN KEY (oportunidad_id) REFERENCES oportunidades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_perfil_autor_id_perfiles FOREIGN KEY (perfil_autor_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_publicacion_compartida_id_publicaciones FOREIGN KEY (publicacion_compartida_id) REFERENCES publicaciones (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_tipo_publicacion_codigo_tipos_publicacion FOREIGN KEY (tipo_publicacion_codigo) REFERENCES tipos_publicacion (codigo) ON DELETE RESTRICT
);


CREATE TABLE reacciones_comentario (
    comentario_id uuid NOT NULL,
    perfil_id uuid NOT NULL,
    tipo_reaccion_codigo text NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_reacciones_comentario PRIMARY KEY (comentario_id, perfil_id),
    CONSTRAINT ck_reacciones_comentario_tipo_reaccion_codigo_longitud CHECK (char_length("tipo_reaccion_codigo") <= 64),
    CONSTRAINT fk_reacciones_comentario_comentario_id_comentarios FOREIGN KEY (comentario_id) REFERENCES comentarios (id) ON DELETE CASCADE,
    CONSTRAINT fk_reacciones_comentario_perfil_id_perfiles FOREIGN KEY (perfil_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_reacciones_comentario_tipo_reaccion_codigo_tipos_reaccion FOREIGN KEY (tipo_reaccion_codigo) REFERENCES tipos_reaccion (codigo) ON DELETE RESTRICT
);


CREATE TABLE guardados (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_id uuid NOT NULL,
    publicacion_id uuid,
    producto_id uuid,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_guardados PRIMARY KEY (id),
    CONSTRAINT ck_guardados_un_objetivo CHECK (("publicacion_id" IS NOT NULL) <> ("producto_id" IS NOT NULL)),
    CONSTRAINT fk_guardados_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_guardados_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE RESTRICT,
    CONSTRAINT fk_guardados_usuario_id_usuarios FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE menciones (
    publicacion_id uuid NOT NULL,
    perfil_id uuid NOT NULL,
    CONSTRAINT pk_menciones PRIMARY KEY (publicacion_id, perfil_id),
    CONSTRAINT fk_menciones_perfil_id_perfiles FOREIGN KEY (perfil_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_menciones_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE CASCADE
);


CREATE TABLE publicaciones_archivos (
    publicacion_id uuid NOT NULL,
    archivo_id uuid NOT NULL,
    posicion smallint NOT NULL,
    texto_alternativo text,
    CONSTRAINT pk_publicaciones_archivos PRIMARY KEY (publicacion_id, archivo_id),
    CONSTRAINT ck_publicaciones_archivos_texto_alternativo_longitud CHECK (char_length("texto_alternativo") <= 255),
    CONSTRAINT fk_publicaciones_archivos_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_archivos_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE CASCADE
);


CREATE TABLE publicaciones_etiquetas (
    publicacion_id uuid NOT NULL,
    etiqueta_id integer NOT NULL,
    CONSTRAINT pk_publicaciones_etiquetas PRIMARY KEY (publicacion_id, etiqueta_id),
    CONSTRAINT fk_publicaciones_etiquetas_etiqueta_id_etiquetas FOREIGN KEY (etiqueta_id) REFERENCES etiquetas (id) ON DELETE RESTRICT,
    CONSTRAINT fk_publicaciones_etiquetas_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE CASCADE
);


CREATE TABLE reacciones_publicacion (
    publicacion_id uuid NOT NULL,
    perfil_id uuid NOT NULL,
    tipo_reaccion_codigo text NOT NULL,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_reacciones_publicacion PRIMARY KEY (publicacion_id, perfil_id),
    CONSTRAINT ck_reacciones_publicacion_tipo_reaccion_codigo_longitud CHECK (char_length("tipo_reaccion_codigo") <= 64),
    CONSTRAINT fk_reacciones_publicacion_perfil_id_perfiles FOREIGN KEY (perfil_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_reacciones_publicacion_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE CASCADE,
    CONSTRAINT fk_reacciones_publicacion_tipo_reaccion_codigo_tipos_reaccion FOREIGN KEY (tipo_reaccion_codigo) REFERENCES tipos_reaccion (codigo) ON DELETE RESTRICT
);


CREATE TABLE conversaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    tipo text NOT NULL DEFAULT 'directa',
    titulo text,
    clave_directa text,
    creado_por_usuario_id uuid NOT NULL,
    ultimo_mensaje_id uuid,
    ultimo_mensaje_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_conversaciones PRIMARY KEY (id),
    CONSTRAINT ck_conversaciones_clave_directa_longitud CHECK (char_length("clave_directa") <= 1024),
    CONSTRAINT ck_conversaciones_tipo_enum CHECK ("tipo" IN ('directa', 'grupo')),
    CONSTRAINT ck_conversaciones_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_conversaciones_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE cotizacion_items (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    cotizacion_id uuid NOT NULL,
    producto_id uuid,
    descripcion text NOT NULL,
    cantidad numeric(12,2) NOT NULL,
    precio_unitario numeric(14,2) NOT NULL,
    total_linea numeric(14,2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED,
    CONSTRAINT pk_cotizacion_items PRIMARY KEY (id),
    CONSTRAINT ck_cotizacion_items_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT fk_cotizacion_items_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE RESTRICT
);


CREATE TABLE cotizaciones (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    solicitud_cotizacion_id uuid NOT NULL,
    version smallint NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    moneda_codigo character(3) NOT NULL DEFAULT 'COP',
    subtotal numeric(14,2) NOT NULL DEFAULT 0.0,
    descuento numeric(14,2) NOT NULL DEFAULT 0.0,
    impuesto numeric(14,2) NOT NULL DEFAULT 0.0,
    total numeric(14,2) GENERATED ALWAYS AS (subtotal - descuento + impuesto) STORED,
    valido_hasta date,
    notas text,
    estado text NOT NULL DEFAULT 'enviado',
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_cotizaciones PRIMARY KEY (id),
    CONSTRAINT "AK_cotizaciones_id_solicitud_cotizacion_id" UNIQUE (id, solicitud_cotizacion_id),
    CONSTRAINT ck_cotizaciones_estado_enum CHECK ("estado" IN ('enviado', 'reemplazado', 'aceptado', 'rechazado', 'vencido')),
    CONSTRAINT ck_cotizaciones_notas_longitud CHECK (char_length("notas") <= 4000),
    CONSTRAINT fk_cotizaciones_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_cotizaciones_moneda_codigo_monedas FOREIGN KEY (moneda_codigo) REFERENCES monedas (codigo) ON DELETE RESTRICT
);


CREATE TABLE solicitudes_cotizacion (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    numero bigint GENERATED ALWAYS AS IDENTITY,
    emprendimiento_id uuid NOT NULL,
    perfil_solicitante_id uuid NOT NULL,
    creado_por_usuario_id uuid NOT NULL,
    conversacion_id uuid,
    titulo text NOT NULL,
    mensaje text,
    fecha_deseada date,
    ciudad_entrega_id integer,
    estado text NOT NULL DEFAULT 'pendiente',
    cotizacion_aceptada_id uuid,
    cerrado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_solicitudes_cotizacion PRIMARY KEY (id),
    CONSTRAINT ck_solicitudes_cotizacion_estado_enum CHECK ("estado" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')),
    CONSTRAINT ck_solicitudes_cotizacion_mensaje_longitud CHECK (char_length("mensaje") <= 10000),
    CONSTRAINT ck_solicitudes_cotizacion_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_solicitudes_cotizacion_ciudad_entrega_id_ciudades FOREIGN KEY (ciudad_entrega_id) REFERENCES ciudades (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_cotizacion_conversacion_id_conversaciones FOREIGN KEY (conversacion_id) REFERENCES conversaciones (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_cotizacion_cotizacion_aceptada_id_id_co_e86cb7ca FOREIGN KEY (cotizacion_aceptada_id, id) REFERENCES cotizaciones (id, solicitud_cotizacion_id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_cotizacion_creado_por_usuario_id_usuarios FOREIGN KEY (creado_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_cotizacion_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitudes_cotizacion_perfil_solicitante_id_perfiles FOREIGN KEY (perfil_solicitante_id) REFERENCES perfiles (id) ON DELETE RESTRICT
);


CREATE TABLE historial_solicitudes (
    id bigint GENERATED ALWAYS AS IDENTITY,
    solicitud_cotizacion_id uuid NOT NULL,
    estado_anterior text,
    estado_nuevo text NOT NULL,
    usuario_actor_id uuid,
    nota text,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_historial_solicitudes PRIMARY KEY (id),
    CONSTRAINT ck_historial_solicitudes_estado_anterior_enum CHECK ("estado_anterior" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')),
    CONSTRAINT ck_historial_solicitudes_estado_nuevo_enum CHECK ("estado_nuevo" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')),
    CONSTRAINT ck_historial_solicitudes_nota_longitud CHECK (char_length("nota") <= 4000),
    CONSTRAINT fk_historial_solicitudes_solicitud_cotizacion_id_solic_3cc7a2f1 FOREIGN KEY (solicitud_cotizacion_id) REFERENCES solicitudes_cotizacion (id) ON DELETE RESTRICT,
    CONSTRAINT fk_historial_solicitudes_usuario_actor_id_usuarios FOREIGN KEY (usuario_actor_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE resenas (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    usuario_resenador_id uuid NOT NULL,
    emprendimiento_id uuid NOT NULL,
    producto_id uuid,
    solicitud_cotizacion_id uuid,
    calificacion smallint NOT NULL,
    titulo text,
    contenido text,
    estado text NOT NULL DEFAULT 'publicado',
    respuesta_emprendimiento text,
    respondido_por_usuario_id uuid,
    respondido_en timestamptz,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    actualizado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_resenas PRIMARY KEY (id),
    CONSTRAINT ck_resenas_calificacion CHECK ("calificacion" BETWEEN 1 AND 5),
    CONSTRAINT ck_resenas_contenido_longitud CHECK (char_length("contenido") <= 10000),
    CONSTRAINT ck_resenas_estado_enum CHECK ("estado" IN ('publicado', 'oculto')),
    CONSTRAINT ck_resenas_respuesta_emprendimiento_longitud CHECK (char_length("respuesta_emprendimiento") <= 10000),
    CONSTRAINT ck_resenas_titulo_longitud CHECK (char_length("titulo") <= 255),
    CONSTRAINT fk_resenas_emprendimiento_id_emprendimientos FOREIGN KEY (emprendimiento_id) REFERENCES emprendimientos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_resenas_producto_id_emprendimiento_id_productos FOREIGN KEY (producto_id, emprendimiento_id) REFERENCES productos (id, emprendimiento_id) ON DELETE RESTRICT,
    CONSTRAINT fk_resenas_respondido_por_usuario_id_usuarios FOREIGN KEY (respondido_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,
    CONSTRAINT fk_resenas_solicitud_cotizacion_id_solicitudes_cotizacion FOREIGN KEY (solicitud_cotizacion_id) REFERENCES solicitudes_cotizacion (id) ON DELETE RESTRICT,
    CONSTRAINT fk_resenas_usuario_resenador_id_usuarios FOREIGN KEY (usuario_resenador_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE solicitud_items (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    solicitud_cotizacion_id uuid NOT NULL,
    producto_id uuid,
    descripcion text NOT NULL,
    cantidad numeric(12,2) NOT NULL DEFAULT 1.0,
    notas text,
    CONSTRAINT pk_solicitud_items PRIMARY KEY (id),
    CONSTRAINT ck_solicitud_items_descripcion_longitud CHECK (char_length("descripcion") <= 10000),
    CONSTRAINT ck_solicitud_items_notas_longitud CHECK (char_length("notas") <= 4000),
    CONSTRAINT fk_solicitud_items_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_solicitud_items_solicitud_cotizacion_id_solicitudes_c716681e FOREIGN KEY (solicitud_cotizacion_id) REFERENCES solicitudes_cotizacion (id) ON DELETE CASCADE
);


CREATE TABLE resenas_archivos (
    resena_id uuid NOT NULL,
    archivo_id uuid NOT NULL,
    posicion smallint NOT NULL,
    CONSTRAINT pk_resenas_archivos PRIMARY KEY (resena_id, archivo_id),
    CONSTRAINT fk_resenas_archivos_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_resenas_archivos_resena_id_resenas FOREIGN KEY (resena_id) REFERENCES resenas (id) ON DELETE CASCADE
);


CREATE TABLE mensajes (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    conversacion_id uuid NOT NULL,
    perfil_remitente_id uuid NOT NULL,
    usuario_remitente_id uuid,
    tipo text NOT NULL DEFAULT 'texto',
    contenido text,
    producto_id uuid,
    solicitud_cotizacion_id uuid,
    responde_a_mensaje_id uuid,
    editado_en timestamptz,
    eliminado_en timestamptz,
    creado_en timestamptz NOT NULL DEFAULT (now()),
    CONSTRAINT pk_mensajes PRIMARY KEY (id),
    CONSTRAINT ck_mensajes_contenido_longitud CHECK (char_length("contenido") <= 10000),
    CONSTRAINT ck_mensajes_tipo_enum CHECK ("tipo" IN ('texto', 'archivo', 'producto', 'cotizacion', 'sistema')),
    CONSTRAINT fk_mensajes_producto_id_productos FOREIGN KEY (producto_id) REFERENCES productos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_mensajes_responde_a_mensaje_id_mensajes FOREIGN KEY (responde_a_mensaje_id) REFERENCES mensajes (id) ON DELETE RESTRICT,
    CONSTRAINT fk_mensajes_solicitud_cotizacion_id_solicitudes_cotizacion FOREIGN KEY (solicitud_cotizacion_id) REFERENCES solicitudes_cotizacion (id) ON DELETE RESTRICT,
    CONSTRAINT fk_mensajes_usuario_remitente_id_usuarios FOREIGN KEY (usuario_remitente_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE TABLE mensajes_adjuntos (
    mensaje_id uuid NOT NULL,
    archivo_id uuid NOT NULL,
    posicion smallint NOT NULL,
    CONSTRAINT pk_mensajes_adjuntos PRIMARY KEY (mensaje_id, archivo_id),
    CONSTRAINT fk_mensajes_adjuntos_archivo_id_archivos FOREIGN KEY (archivo_id) REFERENCES archivos (id) ON DELETE RESTRICT,
    CONSTRAINT fk_mensajes_adjuntos_mensaje_id_mensajes FOREIGN KEY (mensaje_id) REFERENCES mensajes (id) ON DELETE CASCADE
);


CREATE TABLE participantes (
    conversacion_id uuid NOT NULL,
    perfil_id uuid NOT NULL,
    rol text NOT NULL DEFAULT 'miembro',
    usuario_asignado_id uuid,
    unido_en timestamptz NOT NULL DEFAULT (now()),
    salio_en timestamptz,
    ultimo_mensaje_leido_id uuid,
    ultima_lectura_en timestamptz,
    silenciado_hasta timestamptz,
    archivado_en timestamptz,
    CONSTRAINT pk_participantes PRIMARY KEY (conversacion_id, perfil_id),
    CONSTRAINT ck_participantes_rol_enum CHECK ("rol" IN ('miembro', 'administrador')),
    CONSTRAINT fk_participantes_conversacion_id_conversaciones FOREIGN KEY (conversacion_id) REFERENCES conversaciones (id) ON DELETE RESTRICT,
    CONSTRAINT fk_participantes_perfil_id_perfiles FOREIGN KEY (perfil_id) REFERENCES perfiles (id) ON DELETE RESTRICT,
    CONSTRAINT fk_participantes_ultimo_mensaje_leido_id_mensajes FOREIGN KEY (ultimo_mensaje_leido_id) REFERENCES mensajes (id) ON DELETE RESTRICT,
    CONSTRAINT fk_participantes_usuario_asignado_id_usuarios FOREIGN KEY (usuario_asignado_id) REFERENCES usuarios (id) ON DELETE RESTRICT
);


CREATE INDEX ix_archivos_estado_creado_en ON archivos (estado, creado_en);


CREATE INDEX ix_archivos_subido_por_usuario_id ON archivos (subido_por_usuario_id);


CREATE UNIQUE INDEX ux_archivos_clave_objeto ON archivos (clave_objeto);


CREATE INDEX ix_bloqueos_perfil_bloqueado_id ON bloqueos (perfil_bloqueado_id);


CREATE INDEX ix_categorias_padre_id ON categorias (padre_id);


CREATE UNIQUE INDEX ux_categorias_slug ON categorias (slug);


CREATE INDEX ix_ciudades_departamento_id ON ciudades (departamento_id);


CREATE INDEX ix_comentarios_comentario_padre_id_publicacion_id ON comentarios (comentario_padre_id, publicacion_id);


CREATE INDEX ix_comentarios_creado_por_usuario_id ON comentarios (creado_por_usuario_id);


CREATE INDEX ix_comentarios_perfil_autor_id ON comentarios (perfil_autor_id);


CREATE INDEX ix_comentarios_publicacion_id ON comentarios (publicacion_id);


CREATE INDEX ix_comentarios_responde_a_perfil_id ON comentarios (responde_a_perfil_id);


CREATE INDEX ix_conversaciones_creado_por_usuario_id ON conversaciones (creado_por_usuario_id);


CREATE INDEX ix_conversaciones_ultimo_mensaje_id ON conversaciones (ultimo_mensaje_id);


CREATE UNIQUE INDEX ux_conversaciones_clave_directa ON conversaciones (clave_directa);


CREATE INDEX ix_cotizacion_items_cotizacion_id ON cotizacion_items (cotizacion_id);


CREATE INDEX ix_cotizacion_items_producto_id ON cotizacion_items (producto_id);


CREATE INDEX ix_cotizaciones_creado_por_usuario_id ON cotizaciones (creado_por_usuario_id);


CREATE INDEX ix_cotizaciones_estado_creado_en ON cotizaciones (estado, creado_en);


CREATE INDEX ix_cotizaciones_moneda_codigo ON cotizaciones (moneda_codigo);


CREATE INDEX ix_cotizaciones_solicitud_cotizacion_id ON cotizaciones (solicitud_cotizacion_id);


CREATE INDEX ix_departamentos_pais_codigo ON departamentos (pais_codigo);


CREATE INDEX ix_documentos_verificacion_archivo_id ON documentos_verificacion (archivo_id);


CREATE INDEX ix_emprendimientos_categoria_id ON emprendimientos (categoria_id);


CREATE INDEX ix_emprendimientos_creado_por_usuario_id ON emprendimientos (creado_por_usuario_id);


CREATE UNIQUE INDEX ix_emprendimientos_id_tipo_perfil ON emprendimientos (id, tipo_perfil);


CREATE INDEX ix_enlaces_emprendimiento_emprendimiento_id ON enlaces_emprendimiento (emprendimiento_id);


CREATE UNIQUE INDEX ux_etiquetas_nombre ON etiquetas (nombre);


CREATE INDEX ix_guardados_producto_id ON guardados (producto_id);


CREATE INDEX ix_guardados_publicacion_id ON guardados (publicacion_id);


CREATE INDEX ix_guardados_usuario_id ON guardados (usuario_id);


CREATE INDEX ix_historial_solicitudes_solicitud_cotizacion_id ON historial_solicitudes (solicitud_cotizacion_id);


CREATE INDEX ix_historial_solicitudes_usuario_actor_id ON historial_solicitudes (usuario_actor_id);


CREATE INDEX ix_intereses_usuario_categoria_id ON intereses_usuario (categoria_id);


CREATE INDEX ix_menciones_perfil_id ON menciones (perfil_id);


CREATE INDEX ix_mensajes_conversacion_id_perfil_remitente_id ON mensajes (conversacion_id, perfil_remitente_id);


CREATE INDEX ix_mensajes_producto_id ON mensajes (producto_id);


CREATE INDEX ix_mensajes_responde_a_mensaje_id ON mensajes (responde_a_mensaje_id);


CREATE INDEX ix_mensajes_solicitud_cotizacion_id ON mensajes (solicitud_cotizacion_id);


CREATE INDEX ix_mensajes_usuario_remitente_id ON mensajes (usuario_remitente_id);


CREATE INDEX ix_mensajes_adjuntos_archivo_id ON mensajes_adjuntos (archivo_id);


CREATE INDEX ix_miembros_emprendimiento_estado_creado_en ON miembros_emprendimiento (estado, creado_en);


CREATE INDEX ix_miembros_emprendimiento_invitado_por_usuario_id ON miembros_emprendimiento (invitado_por_usuario_id);


CREATE INDEX ix_miembros_emprendimiento_usuario_id ON miembros_emprendimiento (usuario_id);


CREATE INDEX ix_notificaciones_perfil_actor_id ON notificaciones (perfil_actor_id);


CREATE INDEX ix_notificaciones_usuario_destinatario_id ON notificaciones (usuario_destinatario_id);


CREATE INDEX ix_ofertas_mentoria_categoria_id ON ofertas_mentoria (categoria_id);


CREATE INDEX ix_ofertas_mentoria_moneda_codigo ON ofertas_mentoria (moneda_codigo);


CREATE INDEX ix_ofertas_mentoria_usuario_mentor_id ON ofertas_mentoria (usuario_mentor_id);


CREATE INDEX ix_oportunidades_categoria_id ON oportunidades (categoria_id);


CREATE INDEX ix_oportunidades_ciudad_id ON oportunidades (ciudad_id);


CREATE INDEX ix_oportunidades_creado_por_usuario_id ON oportunidades (creado_por_usuario_id);


CREATE INDEX ix_oportunidades_estado_creado_en ON oportunidades (estado, creado_en);


CREATE INDEX ix_oportunidades_perfil_autor_id ON oportunidades (perfil_autor_id);


CREATE INDEX ix_oportunidades_tipo_codigo ON oportunidades (tipo_codigo);


CREATE INDEX ix_paises_moneda_defecto_codigo ON paises (moneda_defecto_codigo);


CREATE INDEX ix_participantes_perfil_id ON participantes (perfil_id);


CREATE INDEX ix_participantes_ultimo_mensaje_leido_id ON participantes (ultimo_mensaje_leido_id);


CREATE INDEX ix_participantes_usuario_asignado_id ON participantes (usuario_asignado_id);


CREATE INDEX ix_perfiles_ciudad_id ON perfiles (ciudad_id);


CREATE INDEX ix_perfiles_estado_creado_en ON perfiles (estado, creado_en);


CREATE INDEX ix_perfiles_foto_archivo_id ON perfiles (foto_archivo_id);


CREATE INDEX ix_perfiles_portada_archivo_id ON perfiles (portada_archivo_id);


CREATE INDEX ix_postulaciones_creado_por_usuario_id ON postulaciones (creado_por_usuario_id);


CREATE INDEX ix_postulaciones_estado_creado_en ON postulaciones (estado, creado_en);


CREATE INDEX ix_postulaciones_oportunidad_id ON postulaciones (oportunidad_id);


CREATE INDEX ix_postulaciones_perfil_postulante_id ON postulaciones (perfil_postulante_id);


CREATE INDEX ix_productos_categoria_id ON productos (categoria_id);


CREATE INDEX ix_productos_ciudad_id ON productos (ciudad_id);


CREATE INDEX ix_productos_creado_por_usuario_id ON productos (creado_por_usuario_id);


CREATE INDEX ix_productos_emprendimiento_id ON productos (emprendimiento_id);


CREATE INDEX ix_productos_estado_creado_en ON productos (estado, creado_en);


CREATE INDEX ix_productos_moneda_codigo ON productos (moneda_codigo);


CREATE INDEX ix_productos_archivos_archivo_id ON productos_archivos (archivo_id);


CREATE INDEX ix_productos_etiquetas_etiqueta_id ON productos_etiquetas (etiqueta_id);


CREATE INDEX ix_publicaciones_comentario_aceptado_id_id ON publicaciones (comentario_aceptado_id, id);


CREATE INDEX ix_publicaciones_creado_por_usuario_id ON publicaciones (creado_por_usuario_id);


CREATE INDEX ix_publicaciones_oportunidad_id ON publicaciones (oportunidad_id);


CREATE INDEX ix_publicaciones_perfil_autor_id ON publicaciones (perfil_autor_id);


CREATE INDEX ix_publicaciones_producto_id ON publicaciones (producto_id);


CREATE INDEX ix_publicaciones_publicacion_compartida_id ON publicaciones (publicacion_compartida_id);


CREATE INDEX ix_publicaciones_tipo_publicacion_codigo ON publicaciones (tipo_publicacion_codigo);


CREATE INDEX ix_publicaciones_archivos_archivo_id ON publicaciones_archivos (archivo_id);


CREATE INDEX ix_publicaciones_etiquetas_etiqueta_id ON publicaciones_etiquetas (etiqueta_id);


CREATE INDEX ix_reacciones_comentario_perfil_id ON reacciones_comentario (perfil_id);


CREATE INDEX ix_reacciones_comentario_tipo_reaccion_codigo ON reacciones_comentario (tipo_reaccion_codigo);


CREATE INDEX ix_reacciones_publicacion_perfil_id ON reacciones_publicacion (perfil_id);


CREATE INDEX ix_reacciones_publicacion_tipo_reaccion_codigo ON reacciones_publicacion (tipo_reaccion_codigo);


CREATE INDEX ix_reportes_estado_creado_en ON reportes (estado, creado_en);


CREATE INDEX ix_reportes_motivo_codigo ON reportes (motivo_codigo);


CREATE INDEX ix_reportes_resuelto_por_usuario_id ON reportes (resuelto_por_usuario_id);


CREATE INDEX ix_reportes_usuario_reportante_id ON reportes (usuario_reportante_id);


CREATE INDEX ix_resenas_emprendimiento_id ON resenas (emprendimiento_id);


CREATE INDEX ix_resenas_estado_creado_en ON resenas (estado, creado_en);


CREATE INDEX ix_resenas_producto_id_emprendimiento_id ON resenas (producto_id, emprendimiento_id);


CREATE INDEX ix_resenas_respondido_por_usuario_id ON resenas (respondido_por_usuario_id);


CREATE INDEX ix_resenas_solicitud_cotizacion_id ON resenas (solicitud_cotizacion_id);


CREATE INDEX ix_resenas_usuario_resenador_id ON resenas (usuario_resenador_id);


CREATE INDEX ix_resenas_archivos_archivo_id ON resenas_archivos (archivo_id);


CREATE INDEX ix_seguimientos_estado_creado_en ON seguimientos (estado, creado_en);


CREATE INDEX ix_seguimientos_perfil_seguido_id ON seguimientos (perfil_seguido_id);


CREATE INDEX ix_sesiones_reemplazado_por_id ON sesiones (reemplazado_por_id);


CREATE INDEX ix_sesiones_usuario_id ON sesiones (usuario_id);


CREATE UNIQUE INDEX ux_sesiones_hash_token_refresco ON sesiones (hash_token_refresco);


CREATE INDEX ix_solicitud_items_producto_id ON solicitud_items (producto_id);


CREATE INDEX ix_solicitud_items_solicitud_cotizacion_id ON solicitud_items (solicitud_cotizacion_id);


CREATE INDEX ix_solicitudes_cotizacion_ciudad_entrega_id ON solicitudes_cotizacion (ciudad_entrega_id);


CREATE INDEX ix_solicitudes_cotizacion_conversacion_id ON solicitudes_cotizacion (conversacion_id);


CREATE INDEX ix_solicitudes_cotizacion_cotizacion_aceptada_id_id ON solicitudes_cotizacion (cotizacion_aceptada_id, id);


CREATE INDEX ix_solicitudes_cotizacion_creado_por_usuario_id ON solicitudes_cotizacion (creado_por_usuario_id);


CREATE INDEX ix_solicitudes_cotizacion_emprendimiento_id ON solicitudes_cotizacion (emprendimiento_id);


CREATE INDEX ix_solicitudes_cotizacion_estado_creado_en ON solicitudes_cotizacion (estado, creado_en);


CREATE INDEX ix_solicitudes_cotizacion_perfil_solicitante_id ON solicitudes_cotizacion (perfil_solicitante_id);


CREATE UNIQUE INDEX ux_solicitudes_cotizacion_numero ON solicitudes_cotizacion (numero);


CREATE INDEX ix_solicitudes_mentoria_creado_por_usuario_id ON solicitudes_mentoria (creado_por_usuario_id);


CREATE INDEX ix_solicitudes_mentoria_estado_creado_en ON solicitudes_mentoria (estado, creado_en);


CREATE INDEX ix_solicitudes_mentoria_oferta_id ON solicitudes_mentoria (oferta_id);


CREATE INDEX ix_solicitudes_mentoria_perfil_mentoreado_id ON solicitudes_mentoria (perfil_mentoreado_id);


CREATE INDEX ix_tokens_recuperacion_usuario_id ON tokens_recuperacion (usuario_id);


CREATE UNIQUE INDEX ux_tokens_recuperacion_hash_token ON tokens_recuperacion (hash_token);


CREATE INDEX ix_usuario_tipos_persona_tipo_persona_codigo ON usuario_tipos_persona (tipo_persona_codigo);


CREATE UNIQUE INDEX ix_usuarios_id_tipo_perfil ON usuarios (id, tipo_perfil);


CREATE UNIQUE INDEX ux_variantes_archivo_clave_objeto ON variantes_archivo (clave_objeto);


CREATE INDEX ix_verificaciones_emprendimiento_id ON verificaciones (emprendimiento_id);


CREATE INDEX ix_verificaciones_enviado_por_usuario_id ON verificaciones (enviado_por_usuario_id);


CREATE INDEX ix_verificaciones_revisado_por_usuario_id ON verificaciones (revisado_por_usuario_id);


ALTER TABLE archivos ADD CONSTRAINT fk_archivos_subido_por_usuario_id_usuarios FOREIGN KEY (subido_por_usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT;


ALTER TABLE comentarios ADD CONSTRAINT fk_comentarios_publicacion_id_publicaciones FOREIGN KEY (publicacion_id) REFERENCES publicaciones (id) ON DELETE RESTRICT;


ALTER TABLE conversaciones ADD CONSTRAINT fk_conversaciones_ultimo_mensaje_id_mensajes FOREIGN KEY (ultimo_mensaje_id) REFERENCES mensajes (id) ON DELETE RESTRICT;


ALTER TABLE cotizacion_items ADD CONSTRAINT fk_cotizacion_items_cotizacion_id_cotizaciones FOREIGN KEY (cotizacion_id) REFERENCES cotizaciones (id) ON DELETE CASCADE;


ALTER TABLE cotizaciones ADD CONSTRAINT fk_cotizaciones_solicitud_cotizacion_id_solicitudes_cotizacion FOREIGN KEY (solicitud_cotizacion_id) REFERENCES solicitudes_cotizacion (id) ON DELETE RESTRICT;


ALTER TABLE mensajes ADD CONSTRAINT fk_mensajes_conversacion_id_perfil_remitente_id_participantes FOREIGN KEY (conversacion_id, perfil_remitente_id) REFERENCES participantes (conversacion_id, perfil_id) ON DELETE RESTRICT;


