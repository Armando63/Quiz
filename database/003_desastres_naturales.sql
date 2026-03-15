USE quiz_bd;

UPDATE categorias
SET nombre = 'Desastres Naturales'
WHERE id_categoria = 1;

-- Pregunta 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿De que magnitud fue el terremoto más fuerte que ha ocurrido en Mexico?');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(1, 'texto', '9.0 Mw', false),
(1, 'texto', '8.6 Mw', true),
(1, 'texto', '8.2 Mw', false),
(1, 'texto', '8.4 Mw', false);

-- Pregunta 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿De donde proviene la palabra tsunami?');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(2, 'texto', 'Japones', true),
(2, 'texto', 'Ingles', false),
(2, 'texto', 'Frances', false),
(2, 'texto', 'Español', false);

-- Pregunta 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿Donde fue el epicentro del terremoto que sucedió el 19 de septiembre de 2017');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(3, 'texto', 'Oaxaca', false),
(3, 'texto', 'Ciudad de Mexico', false),
(3, 'texto', 'Michoacan', false),
(3, 'texto', 'Puebla', true); 

-- Pregunta 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿Donde fue el terremoto con mayor magnitud de los 2000 al presente?');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(4, 'texto', 'Mexico', false),
(4, 'texto', 'Chile', false),
(4, 'texto', 'Indonesia', true),
(4, 'texto', 'Japon', false);

-- Pregunta 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿Cual de las siguientes imagenes corresponde al volcán que destruyó Pompeya');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(5, 'imagen', 'Quiz/imagenes/vesubio.jpg', true),
(5, 'imagen', 'Quiz/imagenes/kilauea.jpg', false),
(5, 'imagen', 'Quiz/imagenes/popocatepetl.jpg', false),
(5, 'imagen', 'Quiz/imagenes/krakatau.jpg', false);

-- Pregunta 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'Desastre natural que se caracteriza por fuertes vientos capaces de destruir casas');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(6, 'imagen', 'Quiz/imagenes/ventisca.jpg', false),
(6, 'imagen', 'Quiz/imagenes/erupcion.jpg', false),
(6, 'imagen', 'Quiz/imagenes/tsunami.jpg', false),
(6, 'imagen', 'Quiz/imagenes/tornado.jpg', true);

-- Pregunta 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'A que pez suelen relacionar con los tsunamis');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(7, 'imagen', 'Quiz/imagenes/mamahuevo.jpg', false),
(7, 'imagen', 'Quiz/imagenes/pezdiablo.jpg', false),
(7, 'imagen', 'Quiz/imagenes/weon.jpg', false),
(7, 'imagen', 'Quiz/imagenes/pezremo.jpg', true);

-- Pregunta 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, '¿Cual de estos huracanes sucedió en 2023');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(8, 'imagen', 'Quiz/imagenes/alex.jpg', false),
(8, 'imagen', 'Quiz/imagenes/otis.jpg', true),
(8, 'imagen', 'Quiz/imagenes/patricia.jpg', false),
(8, 'imagen', 'Quiz/imagenes/wilma.jpg', false);

-- Pregunta 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'Que sirena corresponde a la de un tornado');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(9, 'audio', 'Quiz/audios/volcano.mp3', false),
(9, 'audio', 'Quiz/audios/terremoto.mp3', false),
(9, 'audio', 'Quiz/audios/tsunami.mp3', false),
(9, 'audio', 'Quiz/audios/tornado.mp3', true);

-- Pregunta 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'Cual audio corresponde al tsunami de indonesia');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(10, 'audio', 'Quiz/audios/chile.mp3', false),
(10, 'audio', 'Quiz/audios/japon2.mp3', false),
(10, 'audio', 'Quiz/audios/indonesia.mp3', true),
(10, 'audio', 'Quiz/audios/japon.mp3', false);

-- Pregunta 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'Cual audio corresponde a un huracan');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(11, 'audio', 'Quiz/audios/tsunami11.mp3', false),
(11, 'audio', 'Quiz/audios/huracan.mp3', true),
(11, 'audio', 'Quiz/audios/avalancha.mp3', false),
(11, 'audio', 'Quiz/audios/terremoto11.mp3', false);

-- Pregunta 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (1, 'Cual audio corresponde a un terremoto');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(12, 'audio', 'Quiz/audios/tsunami11.mp3', false),
(12, 'audio', 'Quiz/audios/huracan.mp3', false),
(12, 'audio', 'Quiz/audios/japon2.mp3', false),
(12, 'audio', 'Quiz/audios/terremoto11.mp3', true);