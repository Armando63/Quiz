CREATE SCHEMA quiz_bd;
USE quiz_bd;

-- TABLA CATEGORIAS
CREATE TABLE categorias (
    id_categoria INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

-- TABLA PREGUNTAS
CREATE TABLE preguntas (
    id_pregunta INT AUTO_INCREMENT PRIMARY KEY,
    id_categoria INT,
    texto TEXT NOT NULL,
    FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria)
);

-- TABLA OPCIONES
CREATE TABLE opciones (
    id_opcion INT AUTO_INCREMENT PRIMARY KEY,
    id_pregunta INT,
    tipo ENUM('texto','imagen','audio') NOT NULL,
    contenido VARCHAR(255) NOT NULL,
    es_correcta BOOLEAN,
    FOREIGN KEY (id_pregunta) REFERENCES preguntas(id_pregunta)
);

-- TABLA PARTIDAS
CREATE TABLE partidas (
    id_partida INT AUTO_INCREMENT PRIMARY KEY,
    id_categoria INT,
    aciertos INT,
    errores INT,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria)
);

-- TABLA RESPUESTAS PARTIDA
CREATE TABLE respuestas_partida (
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_partida INT,
    id_pregunta INT,
    id_opcion INT,
    correcta BOOLEAN,
    FOREIGN KEY (id_partida) REFERENCES partidas(id_partida),
    FOREIGN KEY (id_pregunta) REFERENCES preguntas(id_pregunta),
    FOREIGN KEY (id_opcion) REFERENCES opciones(id_opcion)
);

-- INSERTAR CATEGORIAS
INSERT INTO categorias (nombre) VALUES
('Desastres Naturales'),
('Deportes'),
('Musica'),
('VideoJuegos'),
('Anime'),
('Peliculas'),
('Fauna');