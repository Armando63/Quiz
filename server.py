import socket
import threading
import requests
import json

ACTIVO = True
PUERTO_UDP = 65534
PUERTO_TCP = 5000

def obtener_preguntas():
    try:
        resp = requests.get("http://127.0.0.1:8000/preguntas")
        return resp.json()
    except:
        print("Error al conectar con API")
        return []

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

IP_LOCAL = obtener_ip_local()

def escuchar_comandos():
    global ACTIVO
    while ACTIVO:
        cmd = input()
        if cmd.lower() == "exit":
            ACTIVO = False
            break

def servidor_tcp():
    global ACTIVO

    tcp = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp.bind(('', PUERTO_TCP))
    tcp.listen(5)
    tcp.settimeout(1)  # 👈 IMPORTANTE

    print(f"Servidor TCP escuchando en puerto {PUERTO_TCP}")

    while ACTIVO:
        try:
            cliente, addr = tcp.accept()
            print(f"Cliente TCP conectado: {addr}")

            if len(preguntas) > 0:
                pregunta = preguntas[0]

                data = json.dumps({
                    "tipo": "pregunta",
                    "texto": pregunta["texto"],
                    "opciones": pregunta["opciones"]
                })

                cliente.send(data.encode())

                respuesta_cliente = cliente.recv(1024).decode()

                try:
                    datos = json.loads(respuesta_cliente)

                    if datos["tipo"] == "respuesta":
                        respuesta = datos["respuesta"]

                        if respuesta == pregunta["correcta"]:
                            cliente.send("Correcto 🎉".encode())
                        else:
                            cliente.send("Incorrecto ❌".encode())

                except:
                    cliente.send("Error al procesar respuesta".encode())

            else:
                cliente.send("No hay preguntas disponibles".encode())

            cliente.close()

        except socket.timeout:
            continue
        except Exception as e:
            print("Error TCP:", e)

    tcp.close()
    print("Servidor TCP cerrado")

def servidor_udp():
    global ACTIVO

    udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    udp.bind(('', PUERTO_UDP))
    udp.settimeout(1)  # 👈 IMPORTANTE

    print(f"Servidor UDP activo en puerto {PUERTO_UDP}")
    print(f"IP del servidor: {IP_LOCAL}")
    print("Escribe 'exit' o presiona Ctrl+C para cerrar\n")

    while ACTIVO:
        try:
            data, addr = udp.recvfrom(1024)
            mensaje = data.decode().strip()

            if mensaje == "BUSCAR_SERVIDOR":
                respuesta = f"{IP_LOCAL}:{PUERTO_TCP}"
                udp.sendto(respuesta.encode(), addr)

        except socket.timeout:
            continue
        except Exception as e:
            print("Error UDP:", e)

    udp.close()
    print("Servidor UDP cerrado")

# 🔥 cargar preguntas
preguntas = obtener_preguntas()

print("\n📚 Preguntas cargadas:")
print(preguntas)
print("--------------------------------------------------\n")

# 🚀 MAIN
try:
    threading.Thread(target=escuchar_comandos, daemon=True).start()
    threading.Thread(target=servidor_tcp, daemon=True).start()

    servidor_udp()  # 👈 hilo principal

except KeyboardInterrupt:
    print("\n🛑 Cerrando servidor...")
    ACTIVO = False