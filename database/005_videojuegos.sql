USE quiz_bd;
UPDATE categorias
SET nombre = 'Videojuegos'
WHERE id_categoria = 4;

-- PREGUNTA 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Qué compañía creó la consola PlayStation?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Nintendo',FALSE),
(LAST_INSERT_ID(),'texto','Sony',TRUE),
(LAST_INSERT_ID(),'texto','Microsoft',FALSE),
(LAST_INSERT_ID(),'texto','Sega',FALSE);


-- PREGUNTA 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cuál es el protagonista principal de la saga The Legend of Zelda?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Mario',FALSE),
(LAST_INSERT_ID(),'texto','Link',TRUE),
(LAST_INSERT_ID(),'texto','Zelda',FALSE),
(LAST_INSERT_ID(),'texto','Samus',FALSE);


-- PREGUNTA 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿En qué videojuego aparece el personaje Master Chief?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Halo',TRUE),
(LAST_INSERT_ID(),'texto','Gears of War',FALSE),
(LAST_INSERT_ID(),'texto','Call of Duty',FALSE),
(LAST_INSERT_ID(),'texto','Battlefield',FALSE);


-- PREGUNTA 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cuál de estos videojuegos es del género battle royale?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'texto','Minecraft',FALSE),
(LAST_INSERT_ID(),'texto','Fortnite',TRUE),
(LAST_INSERT_ID(),'texto','The Sims 4',FALSE),
(LAST_INSERT_ID(),'texto','FIFA 23',FALSE);

-- PREGUNTA 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es la forma de obtener un corazon mas en terraria?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/zelda.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/mario.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/terra.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/mine.jpg',FALSE);

-- PREGUNTA 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el nombre de la arma principal que tiene kratos en los god of war de la saga griega?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/pico.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/caos.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/llave.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/armzelda.jpg',FALSE);

-- PREGUNTA 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual juego gano como mejor juego del año en el 2016?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/forni.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/god.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/issac.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/over.jpg',TRUE);


-- PREGUNTA 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual juego fue el que impulso el exito de los battle royale?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'imagen','imagenes/apex.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/for.jpg',TRUE),
(LAST_INSERT_ID(),'imagen','imagenes/H1.jpg',FALSE),
(LAST_INSERT_ID(),'imagen','imagenes/pubg.jpg',FALSE);


-- PREGUNTA 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido que hace el cofre de fornite?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/cofreclash.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/cofrefornite.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/cofremine.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/cofrezelda.mp3',FALSE);

-- PREGUNTA 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido de muerte que hace los personajes de Roblox?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/muerte_amon.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/muerte_mario.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/muerte_mine.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/muerte_rob.mp3',TRUE);


-- PREGUNTA 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido que sale cuando pasas una noche en FNAF 1?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/victoria_fnaf.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/victoria_mario.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/victoria_mortal.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/victoria_plan.mp3',FALSE);


-- PREGUNTA 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (4,'¿Cual es el sonido que salirte una carta legendaria?');

INSERT INTO opciones (id_pregunta,tipo,contenido,es_correcta) VALUES
(LAST_INSERT_ID(),'audio','audios/clash_carta.mp3',TRUE),
(LAST_INSERT_ID(),'audio','audios/fnaf.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/min.mp3',FALSE),
(LAST_INSERT_ID(),'audio','audios/mortal.mp3',FALSE);