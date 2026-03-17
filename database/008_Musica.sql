USE quiz_bd;

-- PREGUNTA 1 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estos artistas es conocido como el "Rey del Pop"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/heehee.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/elvis.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/alfredo.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/bruno.jpg',FALSE);

-- PREGUNTA 2 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estos artistas es un rapero famoso por "Lose Yourself"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/eminem.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/dreik.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/travieso.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/posmalon.jpg',FALSE);

-- PREGUNTA 3 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estas bandas interpreta "Radioactive"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/imayin.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/coldplay.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/marun.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/maneskin.jpg',FALSE);

-- PREGUNTA 4 (Imagen)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estos artistas es parte del grupo BTS?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/junkok.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/badbuni.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/joji.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/raw.jpg',FALSE);

-- PREGUNTA 5 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estas canciones de Eminem inicia con la frase "Look, if you had one shot, one opportunity..."?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/LoseYourself.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Godzilla.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/WithoutMe.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Mockingbird.mp3',FALSE);

-- PREGUNTA 6 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estas bandas interpreta la canción "Natural"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Natural.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Coldplay.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Muse.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/ArticMonkeys.mp3',FALSE);

-- PREGUNTA 7 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Cuál de estas canciones es interpretada por Paulo Londra?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/PauloLondra.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/MiloJ.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Trueno.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/LitKillah.mp3',FALSE);

-- PREGUNTA 8 (Audio)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Qué artista interpreta la canción "Cradles"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/SubUrban.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Billie.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Apt.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Melanie.mp3',FALSE);

-- PREGUNTA 9 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Qué grupo surcoreano popularizó el K-pop a nivel mundial en la última década?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','BTS',TRUE),
(LAST_INSERT_ID(),'texto','EXO',FALSE),
(LAST_INSERT_ID(),'texto','Blackpink',FALSE),
(LAST_INSERT_ID(),'texto','Super Junior',FALSE);

-- PREGUNTA 10 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Qué artista es conocido por romper récords de velocidad al rapear?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Eminem',TRUE),
(LAST_INSERT_ID(),'texto','Kendrick Lamar',FALSE),
(LAST_INSERT_ID(),'texto','J. Cole',FALSE),
(LAST_INSERT_ID(),'texto','Drake',FALSE);

-- PREGUNTA 11 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Qué instrumento musical tiene teclas, pedales y cuerdas?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Piano',TRUE),
(LAST_INSERT_ID(),'texto','Guitarra',FALSE),
(LAST_INSERT_ID(),'texto','Violín',FALSE),
(LAST_INSERT_ID(),'texto','Batería',FALSE);

-- PREGUNTA 12 (Texto)
INSERT INTO preguntas (id_categoria, texto)
VALUES (3,'¿Qué artista argentino se hizo famoso con "Adán y Eva"?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Paulo Londra',TRUE),
(LAST_INSERT_ID(),'texto','Duki',FALSE),
(LAST_INSERT_ID(),'texto','Trueno',FALSE),
(LAST_INSERT_ID(),'texto','Nicki Nicole',FALSE);