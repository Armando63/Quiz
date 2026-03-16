USE quiz_bd;

-- PREGUNTA 1 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales NO pertenece al zodiaco chino?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/Caballo.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Gallo.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Perro.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Gato.jpg',TRUE);

-- PREGUNTA 2 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales NO es un mamífero?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/Leon.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Guepardo.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Koala.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Serpiente.jpg',TRUE);

-- PREGUNTA 3 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales NO puede encontrarse en América?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/Camello.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/Jaguar.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/OsoP.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/OsoN.jpg',FALSE);

-- PREGUNTA 4 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales NO es un depredador carnívoro?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/Pantera.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/OsoG.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Canguro.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/Pinwino.jpg',FALSE);

-- PREGUNTA 5 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales NO es un ave?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Aguila.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Colibri.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Golondrina.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Elefante.mp3',TRUE);

-- PREGUNTA 6 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos audios corresponde al aullido de un lobo?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Lobo.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Perro.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Oso.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Vaca.mp3',FALSE);

-- PREGUNTA 7 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos audios pertenece a un animal marino?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Caballo.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Delfin.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Jaguar.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Leon.mp3',FALSE);

-- PREGUNTA 8 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos audios pertenece a un felino salvaje?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Tigre.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Canguro.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Pinwino.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Serpiente.mp3',FALSE);

-- PREGUNTA 9 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál es el mamífero más grande del planeta?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Elefante africano',FALSE),
(LAST_INSERT_ID(),'texto','Ballena azul',TRUE),
(LAST_INSERT_ID(),'texto','Rinoceronte',FALSE),
(LAST_INSERT_ID(),'texto','Hipopótamo',FALSE);

-- PREGUNTA 10 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Qué animal es conocido como el animal terrestre más rápido del mundo?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Guepardo',TRUE),
(LAST_INSERT_ID(),'texto','Antílope',FALSE),
(LAST_INSERT_ID(),'texto','Caballo',FALSE),
(LAST_INSERT_ID(),'texto','Avestruz',FALSE);

-- PREGUNTA 11 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales pone huevos?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Murciélago',FALSE),
(LAST_INSERT_ID(),'texto','Ballena',FALSE),
(LAST_INSERT_ID(),'texto','Cocodrilo',TRUE),
(LAST_INSERT_ID(),'texto','Delfín',FALSE);

-- PREGUNTA 12 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (7,'¿Cuál de estos animales es capaz de volar hacia atrás?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Águila',FALSE),
(LAST_INSERT_ID(),'texto','Colibrí',TRUE),
(LAST_INSERT_ID(),'texto','Golondrina',FALSE),
(LAST_INSERT_ID(),'texto','Pingüino',FALSE);
