import socket
import threading
import json

ACTIVO = True
PUERTO_TCP = 5000

jugadores = []
clientes = []

lock = threading.Lock()

def enviar_lobby():
    data = json.dumps({
        "tipo": "lobby_update",
        "jugadores": jugadores
    })

    with lock:
        for c in clientes:
            try:
                c.sendall(data.encode())
            except:
                pass

def manejar_cliente(cliente, addr):
    global jugadores, clientes

    print(f"Cliente conectado: {addr}")

    with lock:
        clientes.append(cliente)

    try:
        while True:
            data = cliente.recv(1024)
            if not data:
                break

            mensaje = data.decode()
            datos = json.loads(mensaje)

            if datos["tipo"] == "join":
                with lock:
                    jugadores.append({
                        "nombre": datos["nombre"],
                        "listo": False
                    })
                enviar_lobby()

            elif datos["tipo"] == "ready":
                with lock:
                    for j in jugadores:
                        if j["nombre"] == datos["nombre"]:
                            j["listo"] = datos["listo"]
                enviar_lobby()

    except Exception as e:
        print("Error:", e)

    finally:
        print(f"Cliente desconectado: {addr}")

        with lock:
            clientes.remove(cliente)

        cliente.close()

def servidor_tcp():
    tcp = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp.bind(('', PUERTO_TCP))
    tcp.listen(5)

    print(f"Servidor TCP en puerto {PUERTO_TCP}")

    while ACTIVO:
        cliente, addr = tcp.accept()
        threading.Thread(target=manejar_cliente, args=(cliente, addr), daemon=True).start()

if __name__ == "__main__":
    print("Iniciando servidor...")

    hilo = threading.Thread(target=servidor_tcp)
    hilo.start()

    hilo.join()