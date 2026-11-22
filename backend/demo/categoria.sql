INSERT INTO categorias (slug, nombre, activo)
VALUES ('demo-backend2', 'Emprendimientos de prueba', true)
ON CONFLICT (slug) DO NOTHING;
