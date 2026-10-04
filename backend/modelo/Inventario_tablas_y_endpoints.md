# Inventario de tablas y endpoints

Las rutas conservan los nombres de tabla. Las claves compuestas se envían en el orden indicado. Todos los CRUD requieren JWT con `rol=administrador`.

| Tabla SQL | Entidad C# | Clave primaria | Ruta GET/PUT/DELETE | Borrado lógico |
|---|---|---|---|---|
| `archivos` | `Archivo` | `id` | `/api/archivos/{id}` | Sí |
| `bloqueos` | `Bloqueo` | `usuario_bloqueador_id, perfil_bloqueado_id` | `/api/bloqueos/{usuarioBloqueadorId}/{perfilBloqueadoId}` | No |
| `categorias` | `Categoria` | `id` | `/api/categorias/{id}` | No |
| `ciudades` | `Ciudad` | `id` | `/api/ciudades/{id}` | No |
| `comentarios` | `Comentario` | `id` | `/api/comentarios/{id}` | Sí |
| `conversaciones` | `Conversacion` | `id` | `/api/conversaciones/{id}` | No |
| `cotizacion_items` | `CotizacionItem` | `id` | `/api/cotizacion_items/{id}` | No |
| `cotizaciones` | `Cotizacion` | `id` | `/api/cotizaciones/{id}` | No |
| `departamentos` | `Departamento` | `id` | `/api/departamentos/{id}` | No |
| `documentos_verificacion` | `DocumentoVerificacion` | `verificacion_id, archivo_id` | `/api/documentos_verificacion/{verificacionId}/{archivoId}` | No |
| `emprendimientos` | `Emprendimiento` | `id` | `/api/emprendimientos/{id}` | No |
| `enlaces_emprendimiento` | `EnlaceEmprendimiento` | `id` | `/api/enlaces_emprendimiento/{id}` | No |
| `etiquetas` | `Etiqueta` | `id` | `/api/etiquetas/{id}` | No |
| `guardados` | `Guardado` | `id` | `/api/guardados/{id}` | No |
| `historial_solicitudes` | `HistorialSolicitud` | `id` | `/api/historial_solicitudes/{id}` | No |
| `intereses_usuario` | `InteresUsuario` | `usuario_id, categoria_id` | `/api/intereses_usuario/{usuarioId}/{categoriaId}` | No |
| `menciones` | `Mencion` | `publicacion_id, perfil_id` | `/api/menciones/{publicacionId}/{perfilId}` | No |
| `mensajes` | `Mensaje` | `id` | `/api/mensajes/{id}` | Sí |
| `mensajes_adjuntos` | `MensajeAdjunto` | `mensaje_id, archivo_id` | `/api/mensajes_adjuntos/{mensajeId}/{archivoId}` | No |
| `miembros_emprendimiento` | `MiembroEmprendimiento` | `emprendimiento_id, usuario_id` | `/api/miembros_emprendimiento/{emprendimientoId}/{usuarioId}` | No |
| `monedas` | `Moneda` | `codigo` | `/api/monedas/{codigo}` | No |
| `motivos_reporte` | `MotivoReporte` | `codigo` | `/api/motivos_reporte/{codigo}` | No |
| `notificaciones` | `Notificacion` | `id` | `/api/notificaciones/{id}` | No |
| `ofertas_mentoria` | `OfertaMentoria` | `id` | `/api/ofertas_mentoria/{id}` | No |
| `oportunidades` | `Oportunidad` | `id` | `/api/oportunidades/{id}` | Sí |
| `paises` | `Pais` | `codigo` | `/api/paises/{codigo}` | No |
| `participantes` | `Participante` | `conversacion_id, perfil_id` | `/api/participantes/{conversacionId}/{perfilId}` | No |
| `perfiles` | `Perfil` | `id` | `/api/perfiles/{id}` | Sí |
| `permisos_rol` | `PermisoRol` | `rol, permiso` | `/api/permisos_rol/{rol}/{permiso}` | No |
| `postulaciones` | `Postulacion` | `id` | `/api/postulaciones/{id}` | No |
| `productos` | `Producto` | `id` | `/api/productos/{id}` | Sí |
| `productos_archivos` | `ProductoArchivo` | `producto_id, archivo_id` | `/api/productos_archivos/{productoId}/{archivoId}` | No |
| `productos_etiquetas` | `ProductoEtiqueta` | `producto_id, etiqueta_id` | `/api/productos_etiquetas/{productoId}/{etiquetaId}` | No |
| `publicaciones` | `Publicacion` | `id` | `/api/publicaciones/{id}` | Sí |
| `publicaciones_archivos` | `PublicacionArchivo` | `publicacion_id, archivo_id` | `/api/publicaciones_archivos/{publicacionId}/{archivoId}` | No |
| `publicaciones_etiquetas` | `PublicacionEtiqueta` | `publicacion_id, etiqueta_id` | `/api/publicaciones_etiquetas/{publicacionId}/{etiquetaId}` | No |
| `reacciones_comentario` | `ReaccionComentario` | `comentario_id, perfil_id` | `/api/reacciones_comentario/{comentarioId}/{perfilId}` | No |
| `reacciones_publicacion` | `ReaccionPublicacion` | `publicacion_id, perfil_id` | `/api/reacciones_publicacion/{publicacionId}/{perfilId}` | No |
| `reportes` | `Reporte` | `id` | `/api/reportes/{id}` | No |
| `resenas` | `Resena` | `id` | `/api/resenas/{id}` | Sí |
| `resenas_archivos` | `ResenaArchivo` | `resena_id, archivo_id` | `/api/resenas_archivos/{resenaId}/{archivoId}` | No |
| `seguimientos` | `Seguimiento` | `usuario_seguidor_id, perfil_seguido_id` | `/api/seguimientos/{usuarioSeguidorId}/{perfilSeguidoId}` | No |
| `sesiones` | `Sesion` | `id` | `/api/sesiones/{id}` | No |
| `solicitud_items` | `SolicitudItem` | `id` | `/api/solicitud_items/{id}` | No |
| `solicitudes_cotizacion` | `SolicitudCotizacion` | `id` | `/api/solicitudes_cotizacion/{id}` | No |
| `solicitudes_mentoria` | `SolicitudMentoria` | `id` | `/api/solicitudes_mentoria/{id}` | No |
| `tipos_oportunidad` | `TipoOportunidad` | `codigo` | `/api/tipos_oportunidad/{codigo}` | No |
| `tipos_persona` | `TipoPersona` | `codigo` | `/api/tipos_persona/{codigo}` | No |
| `tipos_publicacion` | `TipoPublicacion` | `codigo` | `/api/tipos_publicacion/{codigo}` | No |
| `tipos_reaccion` | `TipoReaccion` | `codigo` | `/api/tipos_reaccion/{codigo}` | No |
| `tokens_recuperacion` | `TokenRecuperacion` | `id` | `/api/tokens_recuperacion/{id}` | No |
| `usuario_tipos_persona` | `UsuarioTipoPersona` | `usuario_id, tipo_persona_codigo` | `/api/usuario_tipos_persona/{usuarioId}/{tipoPersonaCodigo}` | No |
| `usuarios` | `Usuario` | `id` | `/api/usuarios/{id}` | No |
| `usuarios_reservados` | `UsuarioReservado` | `nombre_usuario` | `/api/usuarios_reservados/{nombreUsuario}` | No |
| `variantes_archivo` | `VarianteArchivo` | `archivo_id, variante` | `/api/variantes_archivo/{archivoId}/{variante}` | No |
| `verificaciones` | `Verificacion` | `id` | `/api/verificaciones/{id}` | No |
