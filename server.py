import socket
import threading
import json
import random

ACTIVO = True
PUERTO_UDP = 65534
PUERTO_TCP = 5000

class SalaJuego:
    def __init__(self):
        self.jugadores = {}
        self.categoria_seleccionada = None
        self.partida_iniciada = False
        self.partida_en_curso = False
        self.lock = threading.Lock()
        self.jugador_id_counter = 0
        self.respuestas_recibidas = set()
        self.pregunta_actual = 0
        self.total_preguntas = 5
        self.puntajes = {}

    def agregar_jugador(self, nombre, cliente_socket, addr):
        with self.lock:
            for jugador in self.jugadores.values():
                if jugador["nombre"] == nombre:
                    return None, "El nombre ya está en uso"

            self.jugador_id_counter += 1
            jugador_id = self.jugador_id_counter

            self.jugadores[jugador_id] = {
                "nombre": nombre,
                "listo": False,
                "socket": cliente_socket,
                "addr": addr
            }

            self.puntajes[nombre] = 0
            print(f"✅ Jugador agregado: {nombre}")
            return jugador_id, "OK"

    def quitar_jugador(self, jugador_id):
        with self.lock:
            if jugador_id in self.jugadores:
                nombre = self.jugadores[jugador_id]["nombre"]
                del self.jugadores[jugador_id]
                if nombre in self.puntajes:
                    del self.puntajes[nombre]
                print(f"❌ Jugador eliminado: {nombre}")
                return True
            return False

    def cambiar_estado(self, jugador_id, listo):
        with self.lock:
            if jugador_id in self.jugadores:
                self.jugadores[jugador_id]["listo"] = listo
                return True
            return False

    def todos_listos(self):
        with self.lock:
            return len(self.jugadores) >= 2 and all(j["listo"] for j in self.jugadores.values())

    def obtener_info_jugadores(self):
        with self.lock:
            return [{"nombre": j["nombre"], "listo": j["listo"]} for j in self.jugadores.values()]

    def enviar_a_todos(self, mensaje):
        data = (json.dumps(mensaje) + "\n").encode()
        with self.lock:
            for jugador in self.jugadores.values():
                try:
                    jugador["socket"].sendall(data)
                except:
                    pass

    def iniciar_partida(self):
        with self.lock:
            if self.partida_iniciada:
                return

            self.partida_iniciada = True
            self.partida_en_curso = True
            self.respuestas_recibidas.clear()
            self.pregunta_actual = 0

            for j in self.puntajes:
                self.puntajes[j] = 0

            self.categoria_seleccionada = random.choice(
                ["General", "Geografía", "Historia", "Ciencia", "Deportes"]
            )

            print("🎮 PARTIDA INICIADA")

            self.enviar_a_todos({
                "tipo": "iniciar_partida",
                "categoria": self.categoria_seleccionada
            })

    def registrar_respuesta(self, nombre, pregunta_index, es_correcta, puntaje_actual):
        with self.lock:
            if not self.partida_en_curso:
                return

            if es_correcta:
                self.puntajes[nombre] = puntaje_actual

            clave = f"{nombre}_{pregunta_index}"
            if clave not in self.respuestas_recibidas:
                self.respuestas_recibidas.add(clave)

            if len(self.respuestas_recibidas) >= len(self.jugadores):
                self.respuestas_recibidas.clear()

                if pregunta_index + 1 >= self.total_preguntas:
                    self.finalizar_partida()
                else:
                    self.enviar_siguiente_pregunta()

    def enviar_siguiente_pregunta(self):
        self.pregunta_actual += 1

        self.enviar_a_todos({
            "tipo": "siguiente_pregunta",
            "pregunta_numero": self.pregunta_actual
        })

    def finalizar_partida(self):
        self.partida_en_curso = False

        resultados = [
            {"nombre": n, "puntaje": p, "total_preguntas": self.total_preguntas}
            for n, p in self.puntajes.items()
        ]

        resultados.sort(key=lambda x: x["puntaje"], reverse=True)

        self.enviar_a_todos({
            "tipo": "fin_partida",
            "puntajes": resultados
        })

        self.partida_iniciada = False
        for j in self.jugadores.values():
            j["listo"] = False


sala = SalaJuego()

def manejar_cliente(cliente_socket, addr):
    print(f"🔌 Conectado: {addr}")
    buffer = ""
    jugador_id = None
    nombre = None

    try:
        while True:
            data = cliente_socket.recv(4096)
            if not data:
                break

            buffer += data.decode()

            while "\n" in buffer:
                linea, buffer = buffer.split("\n", 1)
                if not linea.strip():
                    continue

                mensaje = json.loads(linea)

                if mensaje["tipo"] == "join":
                    jugador_id, _ = sala.agregar_jugador(mensaje["nombre"], cliente_socket, addr)
                    nombre = mensaje["nombre"]

                    sala.enviar_a_todos({
                        "tipo": "lobby_update",
                        "jugadores": sala.obtener_info_jugadores()
                    })

                elif mensaje["tipo"] == "ready":
                    sala.cambiar_estado(jugador_id, mensaje["listo"])

                    sala.enviar_a_todos({
                        "tipo": "lobby_update",
                        "jugadores": sala.obtener_info_jugadores()
                    })

                    if sala.todos_listos():
                        sala.iniciar_partida()

                elif mensaje["tipo"] == "respuesta":
                    sala.registrar_respuesta(
                        mensaje["jugador"],
                        mensaje.get("pregunta_index", 0),
                        mensaje["correcta"],
                        mensaje["puntaje_actual"]
                    )

                elif mensaje["tipo"] == "leave":
                    break

    finally:
        if jugador_id:
            sala.quitar_jugador(jugador_id)

        cliente_socket.close()
        print(f"🔌 Desconectado: {addr}")


def servidor_tcp():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind(("0.0.0.0", PUERTO_TCP))
    s.listen()

    print(f"TCP activo en puerto {PUERTO_TCP}")

    while ACTIVO:
        c, addr = s.accept()
        threading.Thread(target=manejar_cliente, args=(c, addr), daemon=True).start()


def servidor_udp():
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
    s.bind(("0.0.0.0", PUERTO_UDP))

    while ACTIVO:
        data, addr = s.recvfrom(1024)
        if data.decode() == "BUSCAR_SERVIDOR":
            ip = socket.gethostbyname(socket.gethostname())
            s.sendto(f"{ip}:{PUERTO_TCP}".encode(), addr)


if __name__ == "__main__":
    print("🎮 Servidor iniciado")

    threading.Thread(target=servidor_tcp, daemon=True).start()
    servidor_udp()