SELECT * FROM quiz_bd.categorias;
USE quiz_bd;

-- =========================
-- PREGUNTA 1
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (4, '¿En qué año se lanzó Minecraft?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, '2010', 0),
(@id_pregunta, '2013', 0),
(@id_pregunta, '2014', 0),
(@id_pregunta, '2009', 1);


-- =========================
-- PREGUNTA 2
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (4, '¿Qué empresa creó Minecraft?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, 'Nintendo', 0),
(@id_pregunta, 'Mojang', 1),
(@id_pregunta, 'Ubisoft', 0),
(@id_pregunta, 'Electronic Arts', 0);


-- =========================
-- PREGUNTA 3
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (4, '¿Cuál es el protagonista de Super Mario Bros?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, 'Link', 0),
(@id_pregunta, 'Mario', 1),
(@id_pregunta, 'Sonic', 0),
(@id_pregunta, 'Luigi', 0);


-- =========================
-- PREGUNTA 4
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (4, '¿En qué año salió Fortnite Battle Royale?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, '2015', 0),
(@id_pregunta, '2016', 0),
(@id_pregunta, '2017', 1),
(@id_pregunta, '2019', 0);

-- =========================
-- PREGUNTA 5
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (6, '¿En qué año se estrenó Titanic?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, '1995', 0),
(@id_pregunta, '1997', 1),
(@id_pregunta, '2000', 0),
(@id_pregunta, '2003', 0);


-- =========================
-- PREGUNTA 6
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (6, '¿Quién dirigió Avatar?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, 'Steven Spielberg', 0),
(@id_pregunta, 'Christopher Nolan', 0),
(@id_pregunta, 'James Cameron', 1),
(@id_pregunta, 'Ridley Scott', 0);


-- =========================
-- PREGUNTA 7
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (6, '¿Qué actor interpreta a Iron Man en el MCU?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, 'Chris Evans', 0),
(@id_pregunta, 'Robert Downey Jr.', 1),
(@id_pregunta, 'Chris Hemsworth', 0),
(@id_pregunta, 'Mark Ruffalo', 0);


-- =========================
-- PREGUNTA 8
-- =========================

INSERT INTO preguntas (id_categoria, texto, tipo, recurso)
VALUES (6, '¿En qué año se estrenó Jurassic Park?', 'texto', NULL);

SET @id_pregunta = LAST_INSERT_ID();

INSERT INTO opciones (id_pregunta, opcion, es_correcta) VALUES
(@id_pregunta, '1993', 1),
(@id_pregunta, '1998', 0),
(@id_pregunta, '2001', 0),
(@id_pregunta, '2005', 0);