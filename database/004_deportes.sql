USE quiz_bd;

-- PREGUNTA 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cuantos balones de oro ha ganado Lionel Messi?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','10',FALSE),
(LAST_INSERT_ID(),'texto','6',FALSE),
(LAST_INSERT_ID(),'texto','8',TRUE),
(LAST_INSERT_ID(),'texto','9',FALSE);

-- PREGUNTA 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Qué superficie de juego se utiliza en Wimbledon?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','cesped',TRUE),
(LAST_INSERT_ID(),'texto','cancha dura',FALSE),
(LAST_INSERT_ID(),'texto','moqueta',FALSE),
(LAST_INSERT_ID(),'texto','asfalto',FALSE);

-- PREGUNTA 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿En que deporte incursiono Michael Jordan en su retiro?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','tenis',FALSE),
(LAST_INSERT_ID(),'texto','atletismo',FALSE),
(LAST_INSERT_ID(),'texto','beisbol',TRUE),
(LAST_INSERT_ID(),'texto','futbol',FALSE);

-- PREGUNTA 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Quien es el equipo con mas Champions?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','milan',FALSE),
(LAST_INSERT_ID(),'texto','liverpool',FALSE),
(LAST_INSERT_ID(),'texto','real madrid',TRUE),
(LAST_INSERT_ID(),'texto','barcelona',FALSE);

-- PREGUNTA 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Quien es Serena Williams?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/venusWilliams.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/cocoGauff.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/serenaWilliams.png',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/naomiOsaka.png',FALSE);

-- PREGUNTA 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cual es el equipo con mas campeonatos del Basquetball?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/bostonCeltics.png',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/Lakers.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/goldenState.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/miamiHeat.png',FALSE);

-- PREGUNTA 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Que equipo tiene mas series mundiales?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/Yankees.png',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/Cardinals.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Dodgers.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/Athletics.png',FALSE);

-- PREGUNTA 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Quien de las siguientes personas es Michael Jordan?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/lebron.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/shaq.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/kobe.png',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/jordan.png',TRUE);

-- PREGUNTA 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cual es el himno del FC Barcelona?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/Juventus.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Liverpool.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Barcelona.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Chivas.mp3',FALSE);

-- PREGUNTA 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cual es el sonido caracteristico del beisbol?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/baseball.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/Basquet.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Camion.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/barco.mp3',FALSE);

-- PREGUNTA 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cual es la cancion caracteristica del AC Milan?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/F.C.Bayern.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/champions.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/RealMadrid.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Acmilan.mp3',TRUE);

-- PREGUNTA 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (2,'¿Cual es la intro de John Cena?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/ReyMysterio.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Undertaker.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/Pimpinela.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/JohnCena.mp3',TRUE);