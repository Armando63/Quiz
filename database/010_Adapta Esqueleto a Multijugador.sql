USE quiz_bd;

-- ========================
--  Nueva Tabla JUGADORES
-- ========================
CREATE TABLE IF NOT EXISTS jugadores (
    id_jugador INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    socket_id VARCHAR(100),
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =========================
-- 2. Modificar PARTIDAS
-- =========================

ALTER TABLE partidas
DROP COLUMN aciertos,
DROP COLUMN errores;

ALTER TABLE partidas
ADD COLUMN estado ENUM('esperando','en_curso','finalizada') DEFAULT 'esperando',
ADD COLUMN max_jugadores INT DEFAULT 4,
ADD COLUMN codigo_sala VARCHAR(10) UNIQUE,
ADD COLUMN pregunta_actual INT DEFAULT NULL;

-- ==========================================
-- 3. Nueva Tabla Relación PARTIDA-JUGADORES
-- ==========================================
CREATE TABLE IF NOT EXISTS partida_jugadores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_partida INT,
    id_jugador INT,
    puntaje INT DEFAULT 0,
    aciertos INT DEFAULT 0,
    errores INT DEFAULT 0,
    listo BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (id_partida) REFERENCES partidas(id_partida),
    FOREIGN KEY (id_jugador) REFERENCES jugadores(id_jugador)
);

-- ================================
-- 4. Modificar RESPUESTAS_PARTIDA
-- ================================

ALTER TABLE respuestas_partida
ADD COLUMN id_jugador INT;

-- aqui solo se renombra un campo
ALTER TABLE respuestas_partida
CHANGE correcta es_correcta BOOLEAN;


ALTER TABLE respuestas_partida
ADD COLUMN tiempo_respuesta FLOAT;

ALTER TABLE respuestas_partida
ADD CONSTRAINT fk_respuesta_jugador
FOREIGN KEY (id_jugador) REFERENCES jugadores(id_jugador);

-- ====================================
-- 5. Índices para Mejorar Rendimiento
-- ====================================
CREATE INDEX idx_partida_jugador ON partida_jugadores(id_partida, id_jugador);
CREATE INDEX idx_respuestas_partida ON respuestas_partida(id_partida, id_jugador);
