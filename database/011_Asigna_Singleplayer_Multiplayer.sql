USE quiz_bd;

ALTER TABLE partidas
ADD COLUMN modo ENUM('singleplayer','multiplayer') DEFAULT 'singleplayer';