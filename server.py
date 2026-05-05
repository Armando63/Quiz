import socket
import threading
import json
import random
import time

ACTIVO = True
PUERTO_UDP = 65534
PUERTO_TCP = 5000

class SalaJuego:
    def __init__(self):
        self.jugadores = {}  # {id: {"nombre": str, "listo": bool, "socket": socket, "addr": tuple}}
        self.categoria_seleccionada = None
        self.partida_iniciada = False
        self.partida_en_curso = False
        self.lock = threading.Lock()
        self.jugador_id_counter = 0
        self.respuestas_recibidas = set()
        self.pregunta_actual = 0
        self.total_preguntas = 5  # Número total de preguntas
        self.puntajes = {}
        self.orden_respuestas = {}  # Para guardar el orden de respuestas
        self.tiempo_espera = 30  # Segundos para responder
        self.temporizador = None
    
    def agregar_jugador(self, nombre, cliente_socket, addr):
        with self.lock:
            self.jugador_id_counter += 1
            jugador_id = self.jugador_id_counter
            
            # Verificar si el nombre ya existe
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
            
            print(f"✅ Jugador agregado: {nombre} (ID: {jugador_id}) - Total: {len(self.jugadores)}")
            
            return jugador_id, "OK"
    
    def quitar_jugador(self, jugador_id):
        with self.lock:
            if jugador_id in self.jugadores:
                nombre = self.jugadores[jugador_id]["nombre"]
                del self.jugadores[jugador_id]
                del self.puntajes[nombre]
                print(f"❌ Jugador eliminado: {nombre} - Restantes: {len(self.jugadores)}")
                return True
            return False
    
    def cambiar_estado(self, jugador_id, listo):
        with self.lock:
            if jugador_id in self.jugadores:
                self.jugadores[jugador_id]["listo"] = listo
                nombre = self.jugadores[jugador_id]["nombre"]
                print(f"📌 Jugador {nombre} está {'LISTO' if listo else 'NO LISTO'}")
                return True
            return False
    
    def todos_listos(self):
        with self.lock:
            if len(self.jugadores) < 2:
                print(f"⚠️ No hay suficientes jugadores: {len(self.jugadores)}/2")
                return False
            todos = all(jugador["listo"] for jugador in self.jugadores.values())
            if todos:
                print(f"✅ ¡TODOS LOS JUGADORES ESTÁN LISTOS! ({len(self.jugadores)} jugadores)")
            return todos
    
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
            print(f"📤 Enviando mensaje a todos: {mensaje.get('tipo', 'desconocido')}")
            for jugador_id, jugador in self.jugadores.items():
                if jugador_id != excluir:
                    try:
                        jugador["socket"].send(data)
                        print(f"   → Enviado a {jugador['nombre']}")
                    except Exception as e:
                        print(f"   ✗ Error enviando a {jugador['nombre']}: {e}")
    
    def iniciar_partida(self):
        if self.partida_iniciada:
            print("⚠️ La partida ya fue iniciada")
            return False
        
        with self.lock:
            self.partida_iniciada = True
            self.partida_en_curso = True
            self.pregunta_actual = 0
            self.respuestas_recibidas.clear()
            
            # Reiniciar puntajes
            for nombre in self.puntajes:
                self.puntajes[nombre] = 0
            
            self.categoria_seleccionada = self.elegir_categoria()
            self.total_preguntas = 5  # Número de preguntas
            
            print(f"\n{'='*50}")
            print(f"🎮 ¡PARTIDA INICIADA!")
            print(f"📚 Categoría: {self.categoria_seleccionada}")
            print(f"👥 Jugadores: {', '.join([j['nombre'] for j in self.jugadores.values()])}")
            print(f"📊 Total preguntas: {self.total_preguntas}")
            print(f"{'='*50}\n")
            
            # Enviar orden de inicio a todos
            self.enviar_a_todos({
                "tipo": "iniciar_partida",
                "categoria": self.categoria_seleccionada,
                "total_preguntas": self.total_preguntas
            })
            return True
    
    def elegir_categoria(self):
        categorias = ["General", "Geografía", "Historia", "Ciencia", "Deportes"]
        return random.choice(categorias)
    
    def registrar_respuesta(self, nombre, pregunta_index, es_correcta, puntaje_actual):
        with self.lock:
            if not self.partida_en_curso:
                return False
            
            # Actualizar puntaje si es correcto
            if es_correcta:
                self.puntajes[nombre] = puntaje_actual
                print(f"✓ {nombre} respondió CORRECTAMENTE - Puntaje: {puntaje_actual}")
            else:
                print(f"✗ {nombre} respondió INCORRECTAMENTE")
            
            # Registrar que este jugador ya respondió
            nombre_hash = f"{nombre}_{pregunta_index}"
            if nombre_hash not in self.respuestas_recibidas:
                self.respuestas_recibidas.add(nombre)
                print(f"📝 Respuestas recibidas: {len(self.respuestas_recibidas)}/{len(self.jugadores)}")
            
            # Verificar si todos respondieron
            if len(self.respuestas_recibidas) >= len(self.jugadores):
                print(f"🎉 ¡Todos los jugadores respondieron la pregunta {pregunta_index + 1}!")
                self.respuestas_recibidas.clear()
                
                # Avanzar a la siguiente pregunta
                siguiente_pregunta = pregunta_index + 1
                
                if siguiente_pregunta >= self.total_preguntas:
                    # Finalizar partida
                    print(f"🏆 ¡PARTIDA FINALIZADA!")
                    self.finalizar_partida()
                else:
                    # Enviar señal para siguiente pregunta
                    print(f"➡️ Enviando señal para siguiente pregunta ({siguiente_pregunta + 1}/{self.total_preguntas})")
                    self.enviar_siguiente_pregunta()
                
                return True
            else:
                print(f"⏳ Esperando a {len(self.jugadores) - len(self.respuestas_recibidas)} jugadores más...")
            
            return False
    
    def enviar_siguiente_pregunta(self):
        self.pregunta_actual += 1
        self.respuestas_recibidas.clear()
        
        mensaje = {
            "tipo": "siguiente_pregunta",
            "pregunta_numero": self.pregunta_actual,
            "total_preguntas": self.total_preguntas
        }
        
        print(f"\n📢 Enviando SIGUIENTE PREGUNTA #{self.pregunta_actual}")
        self.enviar_a_todos(mensaje)
    
    def finalizar_partida(self):
        self.partida_en_curso = False
        
        # Crear lista de resultados ordenados
        resultados = []
        for nombre, puntaje in self.puntajes.items():
            resultados.append({
                "nombre": nombre,
                "puntaje": puntaje,
                "total_preguntas": self.total_preguntas
            })
        
        # Ordenar por puntaje (mayor a menor)
        resultados.sort(key=lambda x: x["puntaje"], reverse=True)
        
        # Mostrar resultados en consola
        print(f"\n{'='*50}")
        print("🏆 RESULTADOS FINALES 🏆")
        print("="*50)
        for i, r in enumerate(resultados, 1):
            print(f"{i}. {r['nombre']} - {r['puntaje']} puntos")
        print("="*50)
        
        # Enviar resultados a todos
        mensaje = {
            "tipo": "fin_partida",
            "puntajes": resultados
        }
        
        self.enviar_a_todos(mensaje)
        print("✅ Resultados enviados a todos los jugadores")
        
        # Resetear para nueva partida
        self.partida_iniciada = False
        for jugador in self.jugadores.values():
            jugador["listo"] = False

# Instancia global de la sala
sala = SalaJuego()

def manejar_cliente(cliente_socket, addr):
    print(f"🔌 Nueva conexión desde {addr}")
    jugador_id = None
    nombre_jugador = None
    
    try:
        while True:
            try:
                # Recibir datos
                data = cliente_socket.recv(4096)
                if not data:
                    break
                
                # Decodificar mensaje
                mensaje = json.loads(data.decode())
                print(f"📨 Mensaje recibido de {addr}: {mensaje.get('tipo', 'desconocido')}")
                
                # Manejar según tipo de mensaje
                if mensaje["tipo"] == "join":
                    resultado = sala.agregar_jugador(mensaje["nombre"], cliente_socket, addr)
                    if resultado[0] is None:
                        respuesta = {"tipo": "error", "mensaje": resultado[1]}
                        cliente_socket.send(json.dumps(respuesta).encode())
                        break
                    
                    jugador_id = resultado[0]
                    nombre_jugador = mensaje["nombre"]
                    
                    # Notificar a todos el nuevo estado del lobby
                    sala.enviar_a_todos({
                        "tipo": "lobby_update",
                        "jugadores": sala.obtener_info_jugadores()
                    })
                    
                elif mensaje["tipo"] == "ready":
                    if jugador_id is not None:
                        sala.cambiar_estado(jugador_id, mensaje["listo"])
                        
                        # Actualizar lobby para todos
                        sala.enviar_a_todos({
                            "tipo": "lobby_update",
                            "jugadores": sala.obtener_info_jugadores()
                        })
                        
                        # Verificar si todos están listos para iniciar
                        if sala.todos_listos() and not sala.partida_iniciada:
                            print("🚀 ¡Todos listos! Iniciando partida...")
                            sala.iniciar_partida()
                
                elif mensaje["tipo"] == "respuesta":
                    # Registrar respuesta del jugador
                    todos_respondieron = sala.registrar_respuesta(
                        mensaje["jugador"],
                        mensaje.get("pregunta_index", 0),
                        mensaje["correcta"],
                        mensaje["puntaje_actual"]
                    )
                
                elif mensaje["tipo"] == "leave":
                    print(f"👋 Jugador {nombre_jugador} solicitó salir")
                    break
                
            except json.JSONDecodeError as e:
                print(f"⚠️ Error decodificando JSON: {e}")
                continue
            except Exception as e:
                print(f"⚠️ Error procesando mensaje: {e}")
                continue
                
    except Exception as e:
        print(f"❌ Error con cliente {addr}: {e}")
    finally:
        if jugador_id is not None:
            sala.quitar_jugador(jugador_id)
            print(f"👋 Jugador {nombre_jugador} desconectado")
            
            # Notificar cambio en el lobby
            sala.enviar_a_todos({
                "tipo": "lobby_update",
                "jugadores": sala.obtener_info_jugadores()
            })
        
        cliente_socket.close()
        print(f"🔌 Conexión cerrada con {addr}")

def servidor_tcp():
    tcp = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    tcp.bind(('0.0.0.0', PUERTO_TCP))
    tcp.listen(10)
    tcp.settimeout(1)
    
    print(f"\n🔌 Servidor TCP escuchando en puerto {PUERTO_TCP}")
    print(f"🌐 Esperando conexiones en 0.0.0.0:{PUERTO_TCP}\n")
    
    while ACTIVO:
        try:
            cliente_socket, addr = tcp.accept()
            print(f"\n✨ Nueva conexión aceptada desde {addr}")
            threading.Thread(target=manejar_cliente, args=(cliente_socket, addr), daemon=True).start()
        except socket.timeout:
            continue
        except Exception as e:
            print(f"❌ Error TCP: {e}")
    
    tcp.close()
    print("🔌 Servidor TCP cerrado")

def servidor_udp():
    udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    udp.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
    udp.bind(('0.0.0.0', PUERTO_UDP))
    udp.settimeout(1)
    
    print(f"🔍 Servidor UDP escuchando en puerto {PUERTO_UDP} para descubrimiento\n")
    
    while ACTIVO:
        try:
            data, addr = udp.recvfrom(1024)
            mensaje = data.decode().strip()
            
            if mensaje == "BUSCAR_SERVIDOR":
                ip_local = obtener_ip_local()
                respuesta = f"{ip_local}:{PUERTO_TCP}"
                udp.sendto(respuesta.encode(), addr)
                print(f"🔍 Respondiendo a descubrimiento desde {addr}: {respuesta}")
                
        except socket.timeout:
            continue
        except Exception as e:
            print(f"❌ Error UDP: {e}")
    
    udp.close()
    print("🔍 Servidor UDP cerrado")

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

def escuchar_comandos():
    global ACTIVO
    print("\n📝 Escribe 'exit' para cerrar el servidor\n")
    while ACTIVO:
        cmd = input()
        if cmd.lower() == "exit":
            print("\n🛑 Cerrando servidor...")
            ACTIVO = False
            break
        elif cmd.lower() == "estado":
            print(f"\n📊 Estado del servidor:")
            print(f"   Jugadores conectados: {len(sala.jugadores)}")
            print(f"   Partida activa: {sala.partida_en_curso}")
            print(f"   Pregunta actual: {sala.pregunta_actual}")
            print(f"   Puntajes: {sala.puntajes}\n")

if __name__ == "__main__":
    IP_LOCAL = obtener_ip_local()
    
    print("\n" + "="*60)
    print("🎮 SERVIDOR QUIZ MULTIJUGADOR 🎮")
    print("="*60)
    print(f"📡 IP del servidor: {IP_LOCAL}")
    print(f"🔌 Puerto TCP: {PUERTO_TCP}")
    print(f"🔍 Puerto UDP: {PUERTO_UDP}")
    print("="*60)
    print("📌 Los clientes deben conectarse a la IP de arriba")
    print("📌 Mínimo 2 jugadores para iniciar partida")
    print("="*60 + "\n")
    
    try:
        # Iniciar hilo para comandos
        threading.Thread(target=escuchar_comandos, daemon=True).start()
        
        # Iniciar servidores
        threading.Thread(target=servidor_tcp, daemon=True).start()
        servidor_udp()
        
    except KeyboardInterrupt:
        print("\n\n🛑 Servidor interrumpido por el usuario")
        ACTIVO = False
    
    print("👋 Servidor cerrado")