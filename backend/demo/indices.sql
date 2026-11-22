BEGIN;
CREATE INDEX IF NOT EXISTS ix_publicaciones_historial_autor
    ON publicaciones (creado_por_usuario_id, creado_en DESC, id DESC)
    WHERE eliminado_en IS NULL;
CREATE INDEX IF NOT EXISTS ix_publicaciones_feed_pagina
    ON publicaciones (visibilidad, creado_en DESC, id DESC)
    WHERE eliminado_en IS NULL;
CREATE INDEX IF NOT EXISTS ix_archivos_historial_usuario
    ON archivos (subido_por_usuario_id, id) WHERE eliminado_en IS NULL;
CREATE INDEX IF NOT EXISTS ix_emprendimientos_lista_usuario
    ON emprendimientos (creado_por_usuario_id, id);
COMMIT;
