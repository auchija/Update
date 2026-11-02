INSERT INTO tipos_publicacion (codigo, nombre, activo)
VALUES ('general', 'General', true)
ON CONFLICT (codigo) DO NOTHING;
