USE quiz_bd;

UPDATE opciones
SET contenido = REPLACE(contenido, 'Quiz/', '')
WHERE contenido LIKE 'Quiz/%';

