
import socket

ACTIVO = True

udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
udp.bind(('', 65534))  # Escucha en todas las interfaces

print("Servidor UDP activo en puerto 65534")
print("Presiona Ctrl+C para detener\n")

while ACTIVO:
    try:
        data, addr = udp.recvfrom(1024)
        mensaje = data.decode()

        print(f"Mensaje recibido de {addr[0]}:{addr[1]} -> {mensaje}")

        if mensaje == "BUSCAR_SERVIDOR":
            respuesta = "SERVIDOR_ACTIVO:5000"
            udp.sendto(respuesta.encode(), addr)

            print(f"Respuesta enviada a {addr[0]}:{addr[1]}")

    except Exception as e:
        print("Error:", e)

udp.close()