USE quiz_bd;

-- PREGUNTA 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Quién dirigió la película Titanic?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','James Cameron',TRUE),
(LAST_INSERT_ID(),'texto','Steven Spielberg',FALSE),
(LAST_INSERT_ID(),'texto','Christopher Nolan',FALSE),
(LAST_INSERT_ID(),'texto','Martin Scorsese',FALSE);


-- PREGUNTA 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿En qué película aparece el personaje Jack Sparrow?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Piratas del Caribe',TRUE),
(LAST_INSERT_ID(),'texto','El Señor de los Anillos',FALSE),
(LAST_INSERT_ID(),'texto','Harry Potter',FALSE),
(LAST_INSERT_ID(),'texto','Star Wars',FALSE);


-- PREGUNTA 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cuál de estas películas trata sobre dinosaurios clonados?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Jurassic Park',TRUE),
(LAST_INSERT_ID(),'texto','Avatar',FALSE),
(LAST_INSERT_ID(),'texto','Transformers',FALSE),
(LAST_INSERT_ID(),'texto','Matrix',FALSE);


-- PREGUNTA 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Qué película ganó el Oscar a Mejor Película en 2020?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Joker',FALSE),
(LAST_INSERT_ID(),'texto','1917',FALSE),
(LAST_INSERT_ID(),'texto','Parasite',TRUE),
(LAST_INSERT_ID(),'texto','Ford v Ferrari',FALSE);

-- PREGUNTA 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿¿Cuál de las siguientes películas fue dirigida por Stanley Kubrick?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/padrino.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/tiburon.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/viento.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/naranja.jpg',TRUE);

-- PREGUNTA 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es la rama que uso luke skywalker?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/revolver.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/sable.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/pistola.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/rambo.jpg',FALSE);

-- PREGUNTA 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es el veiculo que permite viajar en el tiempo?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/alcon.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/batimovil.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/herbi.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/volver.jpg',TRUE);


-- PREGUNTA 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es la pelicula mas taquillera de la historia?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/ava2.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/tita.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/venga.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/ava.jpg',TRUE);


-- PREGUNTA 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es el sonido hace el iconico villano de Star Wars?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/sonido_de_dar.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/sonido_delo.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonido_hombre.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonido_r-rex.mp3',FALSE);

-- PREGUNTA 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es el sonido que hace el veiculo de volver al futuro?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/sonido_del_hombre_famoso.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonido_sable.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonidodelo.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/sonidot-rex.mp3',FALSE);


-- PREGUNTA 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es el sonido que a aparecido en muchisimas peliculas de Hollywood?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/dar.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/dolo.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonido _t.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sonido_famoso.mp3',TRUE);


-- PREGUNTA 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (6,'¿Cual es el sonido que aparece en la famosa pelicula Parque jurásico?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/delo.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/hombre.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/sabre.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/t-rex.mp3',TRUE);
