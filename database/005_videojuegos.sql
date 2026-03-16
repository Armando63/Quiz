USE quiz_bd;
UPDATE categorias
SET nombre = 'Videojuegos'
WHERE id_categoria = 4;

-- PREGUNTA 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Qué compañía creó la consola PlayStation?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(1,'texto','Nintendo',FALSE),
(1,'texto','Sony',TRUE),
(1,'texto','Microsoft',FALSE),
(1,'texto','Sega',FALSE);

-- PREGUNTA 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cuál es el protagonista principal de la saga The Legend of Zelda?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(2,'texto','Mario',FALSE),
(2,'texto','Link',TRUE),
(2,'texto','Zelda',FALSE),
(2,'texto','Samus',FALSE);

-- PREGUNTA 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿En qué videojuego aparece el personaje Master Chief?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(3,'texto','Halo',TRUE),
(3,'texto','Gears of War',FALSE),
(3,'texto','Call of Duty',FALSE),
(3,'texto','Battlefield',FALSE);

-- PREGUNTA 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cuál de estos videojuegos es del género battle royale?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(4,'texto','Minecraft',FALSE),
(4,'texto','Fortnite',TRUE),
(4,'texto','The Sims 4',FALSE),
(4,'texto','FIFA 23',FALSE);

-- PREGUNTA 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es la forma de obtener un corazon mas en Terraria?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(5,'imagen','imagenes/zelda.jpg',FALSE),
(5,'imagen','imagenes/mario.jpg',FALSE),
(5,'imagen','imagenes/terra.jpg',TRUE),
(5,'imagen','imagenes/mine.jpg',FALSE);

-- PREGUNTA 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el nombre del arma principal que tiene Kratos en los God of War de la saga griega?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(6,'imagen','imagenes/pico.jpg',FALSE),
(6,'imagen','imagenes/caos.jpg',TRUE),
(6,'imagen','imagenes/llave.jpg',FALSE),
(6,'imagen','imagenes/armzelda.jpg',FALSE);

-- PREGUNTA 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual juego gano como mejor juego del año en el 2016?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(7,'imagen','imagenes/forni.jpg',FALSE),
(7,'imagen','imagenes/god.jpg',FALSE),
(7,'imagen','imagenes/issac.jpg',FALSE),
(7,'imagen','imagenes/over.jpg',TRUE);

-- PREGUNTA 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual juego fue el que impulso el exito de los battle royale?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(8,'imagen','imagenes/apex.jpg',FALSE),
(8,'imagen','imagenes/for.jpg',TRUE),
(8,'imagen','imagenes/H1.jpg',FALSE),
(8,'imagen','imagenes/pubg.jpg',FALSE);

-- PREGUNTA 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido que hace el cofre de Fortnite?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(9,'audio','audios/cofreclash.mp3',FALSE),
(9,'audio','audios/cofrefornite.mp3',TRUE),
(9,'audio','audios/cofremine.mp3',FALSE),
(9,'audio','audios/cofrezelda.mp3',FALSE);

-- PREGUNTA 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido de muerte que hacen los personajes de Roblox?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(10,'audio','audios/muerte_amon.mp3',FALSE),
(10,'audio','audios/muerte_mario.mp3',FALSE),
(10,'audio','audios/muerte_mine.mp3',FALSE),
(10,'audio','audios/muerte_rob.mp3',TRUE);

-- PREGUNTA 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido que sale cuando pasas una noche en FNAF 1?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(11,'audio','audios/victoria_fnaf.mp3',TRUE),
(11,'audio','audios/victoria_mario.mp3',FALSE),
(11,'audio','audios/victoria_mortal.mp3',FALSE),
(11,'audio','audios/victoria_plan.mp3',FALSE);

-- PREGUNTA 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido cuando te sale una carta legendaria?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(12,'audio','audios/clash_carta.mp3',TRUE),
(12,'audio','audios/fnaf.mp3',FALSE),
(12,'audio','audios/min.mp3',FALSE),
(12,'audio','audios/mortal.mp3',FALSE);