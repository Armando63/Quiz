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
        self.lock = threading.Lock()
        self.jugador_id_counter = 0
        self.respuestas_recibidas = set()
        self.pregunta_actual = 0
        self.puntajes = {}
        self.total_preguntas = 0
    
    def agregar_jugador(self, nombre, cliente_socket, addr):
        with self.lock:
            self.jugador_id_counter += 1
            jugador_id = self.jugador_id_counter
            
            for jugador in self.jugadores.values():
                if jugador["nombre"] == nombre:
                    return None, "El nombre ya está en uso"
            
            self.jugadores[jugador_id] = {
                "nombre": nombre,
                "listo": False,
                "socket": cliente_socket,
                "addr": addr
            }
            
            self.puntajes[nombre] = 0
            
            return jugador_id, "OK"
    
    def quitar_jugador(self, jugador_id):
        with self.lock:
            if jugador_id in self.jugadores:
                nombre = self.jugadores[jugador_id]["nombre"]
                del self.jugadores[jugador_id]
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
            if len(self.jugadores) < 2:
                return False
            return all(jugador["listo"] for jugador in self.jugadores.values())
    
    def obtener_info_jugadores(self):
        with self.lock:
            return [
                {
                    "nombre": j["nombre"],
                    "listo": j["listo"]
                }
                for j in self.jugadores.values()
            ]
    
    def enviar_a_todos(self, mensaje, excluir=None):
        with self.lock:
            data = json.dumps(mensaje).encode()
            for jugador_id, jugador in self.jugadores.items():
                if jugador_id != excluir:
                    try:
                        jugador["socket"].send(data)
                    except:
                        pass
    
    def registrar_respuesta(self, nombre, pregunta_index, es_correcta, puntaje_actual):
        with self.lock:
            if es_correcta:
                self.puntajes[nombre] = puntaje_actual
            self.respuestas_recibidas.add(nombre)
            
            if len(self.respuestas_recibidas) == len(self.jugadores):
                self.respuestas_recibidas.clear()
                return True
        return False
    
    def iniciar_partida(self):
        if self.partida_iniciada:
            return False
        
        with self.lock:
            self.partida_iniciada = True
            self.categoria_seleccionada = self.elegir_categoria()
            
            self.enviar_a_todos({
                "tipo": "iniciar_partida",
                "categoria": self.categoria_seleccionada
            })
            return True
    
    def elegir_categoria(self):
        categorias = ["General", "Geografía", "Historia", "Ciencia", "Deportes"]
        return random.choice(categorias)
    
    def enviar_siguiente_pregunta(self):
        self.enviar_a_todos({"tipo": "siguiente_pregunta"})
    
    def finalizar_partida(self):
        resultados = {
            "tipo": "fin_partida",
            "puntajes": [
                {"nombre": nombre, "puntaje": self.puntajes[nombre], "total_preguntas": self.total_preguntas}
                for nombre in self.jugadores.values()
            ]
        }
        self.enviar_a_todos(resultados)

sala = SalaJuego()

def manejar_cliente(cliente_socket, addr):
    print(f"Nueva conexión desde {addr}")
    jugador_id = None
    nombre_jugador = None
    
    try:
        while True:
            data = cliente_socket.recv(4096)
            if not data:
                break
            
            mensaje = json.loads(data.decode())
            
            if mensaje["tipo"] == "join":
                resultado = sala.agregar_jugador(mensaje["nombre"], cliente_socket, addr)
                if resultado[0] is None:
                    respuesta = {"tipo": "error", "mensaje": resultado[1]}
                    cliente_socket.send(json.dumps(respuesta).encode())
                    break
                
                jugador_id = resultado[0]
                nombre_jugador = mensaje["nombre"]
                print(f"Jugador {nombre_jugador} (ID:{jugador_id}) conectado")
                
                sala.enviar_a_todos({
                    "tipo": "lobby_update",
                    "jugadores": sala.obtener_info_jugadores()
                })
                
            elif mensaje["tipo"] == "ready":
                if jugador_id is not None:
                    sala.cambiar_estado(jugador_id, mensaje["listo"])
                    print(f"Jugador {nombre_jugador} listo: {mensaje['listo']}")
                    
                    sala.enviar_a_todos({
                        "tipo": "lobby_update",
                        "jugadores": sala.obtener_info_jugadores()
                    })
                    
                    if sala.todos_listos():
                        print("¡Todos listos! Iniciando partida...")
                        sala.iniciar_partida()
            
            elif mensaje["tipo"] == "respuesta":
                todos_respondieron = sala.registrar_respuesta(
                    mensaje["jugador"],
                    mensaje["pregunta_index"],
                    mensaje["correcta"],
                    mensaje["puntaje_actual"]
                )
                
                if todos_respondieron:
                    sala.enviar_siguiente_pregunta()
            
            elif mensaje["tipo"] == "leave":
                break
                
    except Exception as e:
        print(f"Error con cliente {addr}: {e}")
    finally:
        if jugador_id is not None:
            sala.quitar_jugador(jugador_id)
            print(f"Jugador {nombre_jugador} desconectado")
            
            sala.enviar_a_todos({
                "tipo": "lobby_update",
                "jugadores": sala.obtener_info_jugadores()
            })
        
        cliente_socket.close()

def servidor_tcp():
    tcp = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    tcp.bind(('0.0.0.0', PUERTO_TCP))
    tcp.listen(10)
    tcp.settimeout(1)
    
    print(f"Servidor TCP escuchando en puerto {PUERTO_TCP}")
    
    while ACTIVO:
        try:
            cliente_socket, addr = tcp.accept()
            threading.Thread(target=manejar_cliente, args=(cliente_socket, addr)).start()
        except socket.timeout:
            continue
        except Exception as e:
            print(f"Error TCP: {e}")
    
    tcp.close()

def servidor_udp():
    udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    udp.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
    udp.bind(('0.0.0.0', PUERTO_UDP))
    udp.settimeout(1)
    
    print(f"Servidor UDP escuchando en puerto {PUERTO_UDP}")
    
    while ACTIVO:
        try:
            data, addr = udp.recvfrom(1024)
            mensaje = data.decode().strip()
            
            if mensaje == "BUSCAR_SERVIDOR":
                respuesta = f"{obtener_ip_local()}:{PUERTO_TCP}"
                udp.sendto(respuesta.encode(), addr)
                
        except socket.timeout:
            continue
        except Exception as e:
            print(f"Error UDP: {e}")
    
    udp.close()

def obtener_ip_local():
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    try:
        s.connect(("8.8.8.8", 80))
        ip = s.getsockname()[0]
    except:
        ip = "127.0.0.1"
    finally:
        s.close()
    return ip

if __name__ == "__main__":
    IP_LOCAL = obtener_ip_local()
    
    print("=" * 50)
    print("SERVIDOR QUIZ MULTIJUGADOR")
    print("=" * 50)
    print(f"IP del servidor: {IP_LOCAL}")
    print(f"Puerto TCP: {PUERTO_TCP}")
    print(f"Puerto UDP: {PUERTO_UDP}")
    print("=" * 50)
    
    try:
        threading.Thread(target=servidor_tcp, daemon=True).start()
        servidor_udp()
    except KeyboardInterrupt:
        print("\nCerrando servidor...")
        ACTIVO = False