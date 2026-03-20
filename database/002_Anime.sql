USE quiz_bd;

-- Pregunta 1
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, '¿Que es lo que generalmente significa cuando empieza a llover en un anime?');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'texto', 'Que va a ser sol', false),
(last_insert_id(), 'texto', 'Que algo malo va a pasar', true),
(last_insert_id(), 'texto', 'Que algo bueno va a pasar', false),
(last_insert_id(), 'texto', 'No significa nada', false);

-- Pregunta 2
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'En japon, como se le conoce al anime de Pokemon');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'texto', 'Pocket Monsters', true),
(last_insert_id(), 'texto', 'Digimon', false),
(last_insert_id(), 'texto', 'Pokemon', false),
(last_insert_id(), 'texto', '日本語に翻訳されたテキスト', false);

-- Pregunta 3
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Como se le llama a la cancion que sale al principio de los animes y tiene una duracin aproximada de 1:30 minutos');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'texto', 'Intro', false),
(last_insert_id(), 'texto', 'Ending', false),
(last_insert_id(), 'texto', 'Outro', false),
(last_insert_id(), 'texto', 'Opening', true); 

-- Pregunta 4
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Como le llamaron en español latino al KameHameHa utilizado por el maestro Roshi');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'texto', 'Onda vital', false),
(last_insert_id(), 'texto', 'KameHameHa', false),
(last_insert_id(), 'texto', 'Onda Glaciar', true),
(last_insert_id(), 'texto', 'Onda Polar', false);

-- Pregunta 5
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, '¿Que imagen utilizaron para censurar el ultimo episodio de School Days debido a un asesinato ocurrido el dia anterior');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'imagen', 'Quiz/imagenes/niceboat.jpg', true),
(last_insert_id(), 'imagen', 'Quiz/imagenes/presa.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/negro.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/fondo.jpg', false);

-- Pregunta 6
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Que anime tuvo un episodio que fue transmitido en plazas y otros lugares publicos en Mexico');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'imagen', 'Quiz/imagenes/pingu.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/onepis.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/pokemon.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/dragonball.jpg', true);

-- Pregunta 7
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'En los arcos de viajes escolares, cual es el destino que comunmente suelen viajar en los animes');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'imagen', 'Quiz/imagenes/mejico.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/fukushima.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/saga.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/kioto.jpg', true);

-- Pregunta 8
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Anime que es famoso por tener un arco de 8 episodios repitiendo lo mismo con pocos cambios en los personajes');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'imagen', 'Quiz/imagenes/shinchan.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/haruhi.jpg', true),
(last_insert_id(), 'imagen', 'Quiz/imagenes/steinsgate.jpg', false),
(last_insert_id(), 'imagen', 'Quiz/imagenes/bahamut.jpg', false);

-- Pregunta 9
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Sonido que usan comunmente para interrumpir al protagonista cuando le va a decir algo importante a alguien');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'audio', 'Quiz/audios/fierro.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/camotes.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/gente.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/tren.mp3', true);

-- Pregunta 10
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Ending que se volvio popular en Japon en los 2007 por la coreografia que aparece en el');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'audio', 'Quiz/audios/luckystar.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/rubychan.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/harehareyukai.mp3', true),
(last_insert_id(), 'audio', 'Quiz/audios/umapyoi.mp3', false);

-- Pregunta 11
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Cual de las siguientes canciones es el ending de un anime de chicas que conducen tanques');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'audio', 'Quiz/audios/liella.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/gup.mp3', true),
(last_insert_id(), 'audio', 'Quiz/audios/senki.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/walkure.mp3', false);

-- Pregunta 12
INSERT INTO preguntas (id_categoria, texto)
VALUES (5, 'Es el opening de un anime de chicas caballo');

INSERT INTO opciones (id_pregunta, tipo, contenido, es_correcta) VALUES
(last_insert_id(), 'audio', 'Quiz/audios/ganso.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/colorful.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/bokurawa.mp3', false),
(last_insert_id(), 'audio', 'Quiz/audios/kitasan.mp3', true);