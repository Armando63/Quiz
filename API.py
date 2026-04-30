import mysql.connector
import socket
from fastapi import FastAPI

app = FastAPI()


def obtener_conexion():
    return mysql.connector.connect(
        host="", #PONER TU IP 
        user="root",
        password="1234",
        database="quiz_bd"
    )

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

@app.get("/conexion")
def conexion():
    return {
        "ip": obtener_ip_local(),
        "puerto": 5000
    }


@app.get("/preguntas")
def obtener_preguntas():
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)

    query = """
    SELECT 
        p.id_pregunta,
        p.texto,
        o.id_opcion,
        o.contenido,
        o.es_correcta
    FROM preguntas p
    JOIN opciones o ON p.id_pregunta = o.id_pregunta
    ORDER BY p.id_pregunta;
    """

    cursor.execute(query)
    filas = cursor.fetchall()

    preguntas = {}
    
    for fila in filas:
        id_p = fila["id_pregunta"]

        if id_p not in preguntas:
            preguntas[id_p] = {
                "texto": fila["texto"],
                "opciones": [],
                "correcta": None
            }

        preguntas[id_p]["opciones"].append(fila["contenido"])

        if fila["es_correcta"]:
            preguntas[id_p]["correcta"] = len(preguntas[id_p]["opciones"]) - 1

    cursor.close()
    conexion.close()

    return list(preguntas.values())