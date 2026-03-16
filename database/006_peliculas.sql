USE quiz_bd;
UPDATE categorias
SET nombre = 'Peliculas'
WHERE id_categoria = 6;

-- PREGUNTA 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Quién dirigió la película Titanic?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(1,'texto','James Cameron',TRUE),
(1,'texto','Steven Spielberg',FALSE),
(1,'texto','Christopher Nolan',FALSE),
(1,'texto','Martin Scorsese',FALSE);

-- PREGUNTA 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿En qué película aparece el personaje Jack Sparrow?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(2,'texto','Piratas del Caribe',TRUE),
(2,'texto','El Señor de los Anillos',FALSE),
(2,'texto','Harry Potter',FALSE),
(2,'texto','Star Wars',FALSE);

-- PREGUNTA 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál de estas películas trata sobre dinosaurios clonados?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(3,'texto','Jurassic Park',TRUE),
(3,'texto','Avatar',FALSE),
(3,'texto','Transformers',FALSE),
(3,'texto','Matrix',FALSE);

-- PREGUNTA 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Qué película ganó el Oscar a Mejor Película en 2020?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(4,'texto','Joker',FALSE),
(4,'texto','1917',FALSE),
(4,'texto','Parasite',TRUE),
(4,'texto','Ford v Ferrari',FALSE);

-- PREGUNTA 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál de las siguientes películas fue dirigida por Stanley Kubrick?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(5,'imagen','imagenes/padrino.jpg',FALSE),
(5,'imagen','imagenes/tiburon.jpg',FALSE),
(5,'imagen','imagenes/viento.jpg',FALSE),
(5,'imagen','imagenes/naranja.jpg',TRUE);

-- PREGUNTA 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el arma que usa Luke Skywalker?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(6,'imagen','imagenes/revolver.jpg',FALSE),
(6,'imagen','imagenes/sable.jpg',TRUE),
(6,'imagen','imagenes/pistola.jpg',FALSE),
(6,'imagen','imagenes/rambo.jpg',FALSE);

-- PREGUNTA 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el vehículo que permite viajar en el tiempo?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(7,'imagen','imagenes/alcon.jpg',FALSE),
(7,'imagen','imagenes/batimovil.jpg',FALSE),
(7,'imagen','imagenes/herbi.jpg',FALSE),
(7,'imagen','imagenes/volver.jpg',TRUE);

-- PREGUNTA 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es la película más taquillera de la historia?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(8,'imagen','imagenes/ava2.jpg',FALSE),
(8,'imagen','imagenes/tita.jpg',FALSE),
(8,'imagen','imagenes/venga.jpg',FALSE),
(8,'imagen','imagenes/ava.jpg',TRUE);

-- PREGUNTA 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el sonido que hace el icónico villano de Star Wars?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(9,'audio','audios/sonido_de_dar.mp3',TRUE),
(9,'audio','audios/sonido_delo.mp3',FALSE),
(9,'audio','audios/sonido_hombre.mp3',FALSE),
(9,'audio','audios/sonido_r-rex.mp3',FALSE);

-- PREGUNTA 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el sonido que hace el vehículo de Volver al Futuro?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(10,'audio','audios/sonido_del_hombre_famoso.mp3',FALSE),
(10,'audio','audios/sonido_sable.mp3',FALSE),
(10,'audio','audios/sonidodelo.mp3',TRUE),
(10,'audio','audios/sonidot-rex.mp3',FALSE);

-- PREGUNTA 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el sonido que ha aparecido en muchísimas películas de Hollywood?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(11,'audio','audios/dar.mp3',FALSE),
(11,'audio','audios/dolo.mp3',FALSE),
(11,'audio','audios/sonido_t.mp3',FALSE),
(11,'audio','audios/sonido_famoso.mp3',TRUE);

-- PREGUNTA 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál es el sonido que aparece en la famosa película Parque Jurásico?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(12,'audio','audios/delo.mp3',FALSE),
(12,'audio','audios/hombre.mp3',FALSE),
(12,'audio','audios/sabre.mp3',FALSE),
(12,'audio','audios/t-rex.mp3',TRUE);