BEGIN;
INSERT INTO monedas (codigo, nombre, simbolo, decimales, activo)
VALUES ('COP', 'Peso colombiano', '$', 2, true)
ON CONFLICT (codigo) DO NOTHING;
INSERT INTO productos (emprendimiento_id, creado_por_usuario_id, tipo, titulo, slug, categoria_id, precio)
SELECT e.id, e.creado_por_usuario_id, 'producto', 'Producto de muestra', 'demo-backend2-producto', e.categoria_id, 10000
FROM emprendimientos e
JOIN perfiles p ON p.id = e.id AND p.eliminado_en IS NULL
JOIN usuarios u ON u.id = e.creado_por_usuario_id
WHERE u.correo = 'demo_backend2_1@example.test' AND p.nombre_visible = 'Proyecto demo 1'
AND NOT EXISTS (SELECT 1 FROM productos WHERE slug = 'demo-backend2-producto')
ORDER BY e.id LIMIT 1;
COMMIT;
SELECT id AS producto_id_de_prueba FROM productos WHERE slug = 'demo-backend2-producto' AND eliminado_en IS NULL;
