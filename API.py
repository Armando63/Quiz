import mysql.connector
import socket
from fastapi import FastAPI, HTTPException, Query
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
from typing import Optional

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

def obtener_conexion():
    return mysql.connector.connect(
        host="localhost",
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

@app.get("/categorias")
def obtener_categorias():
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)
    try:
        try:
            cursor.execute("SELECT id_categoria as id, nombre FROM categorias ORDER BY nombre")
            categorias = cursor.fetchall()
            if categorias:
                return categorias
        except:
            pass
        return [
            {"id": 1, "nombre": "Desastres Naturales"},
            {"id": 2, "nombre": "Deportes"},
            {"id": 3, "nombre": "Musica"},
            {"id": 4, "nombre": "Videojuegos"},
            {"id": 5, "nombre": "Anima"},
            {"id": 6, "nombre": "Peliculas"},
            {"id": 7, "nombre": "Fauna"}
        ]
    except Exception as e:
        print(f"Error al obtener categorías: {e}")
        return []
    finally:
        cursor.close()
        conexion.close()

@app.get("/preguntas")
def obtener_preguntas(
    categoria: Optional[int] = Query(None, description="ID de la categoría para filtrar")
):
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)

    try:
        if categoria:
            query = """
            SELECT 
                p.id_pregunta,
                p.texto,
                o.id_opcion,
                o.contenido,
                o.es_correcta
            FROM preguntas p
            JOIN opciones o ON p.id_pregunta = o.id_pregunta
            WHERE p.id_categoria = %s
            ORDER BY p.id_pregunta;
            """
            cursor.execute(query, (categoria,))
        else:
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

        if not filas:
            print("⚠️ No se encontraron preguntas en la base de datos")
            return []

        print(f"📚 Se encontraron {len(filas)} opciones en la base de datos")

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

        preguntas_validas = []
        for p_id, p_data in preguntas.items():
            if p_data["correcta"] is not None:
                preguntas_validas.append(p_data)
            else:
                print(f"⚠️ Advertencia: Pregunta {p_id} no tiene respuesta correcta definida")

        print(f"✅ Preguntas válidas: {len(preguntas_validas)}")

        import random
        random.shuffle(preguntas_validas)

        return preguntas_validas

    except Exception as e:
        print(f"❌ Error al obtener preguntas: {e}")
        raise HTTPException(status_code=500, detail=f"Error al obtener preguntas: {str(e)}")
    finally:
        cursor.close()
        conexion.close()

@app.get("/preguntas/{pregunta_id}")
def obtener_pregunta_por_id(pregunta_id: int):
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)

    try:
        query = """
        SELECT 
            p.id_pregunta,
            p.texto,
            o.id_opcion,
            o.contenido,
            o.es_correcta
        FROM preguntas p
        JOIN opciones o ON p.id_pregunta = o.id_pregunta
        WHERE p.id_pregunta = %s
        ORDER BY o.id_opcion;
        """

        cursor.execute(query, (pregunta_id,))
        filas = cursor.fetchall()

        if not filas:
            raise HTTPException(status_code=404, detail=f"Pregunta {pregunta_id} no encontrada")

        pregunta = {
            "id": filas[0]["id_pregunta"],
            "texto": filas[0]["texto"],
            "opciones": [],
            "correcta": None
        }

        for i, fila in enumerate(filas):
            pregunta["opciones"].append(fila["contenido"])
            if fila["es_correcta"]:
                pregunta["correcta"] = i

        return pregunta

    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error: {str(e)}")
    finally:
        cursor.close()
        conexion.close()

@app.get("/verificar-categoria/{categoria_id}")
def verificar_categoria(categoria_id: int):
    conexion = obtener_conexion()
    cursor = conexion.cursor()

    try:
        cursor.execute("SELECT COUNT(*) FROM preguntas WHERE id_categoria = %s", (categoria_id,))
        count = cursor.fetchone()[0]

        return {
            "categoria_id": categoria_id,
            "tiene_preguntas": count > 0,
            "total_preguntas": count
        }
    except Exception as e:
        return {
            "categoria_id": categoria_id,
            "tiene_preguntas": True,
            "total_preguntas": 0,
            "error": str(e)
        }
    finally:
        cursor.close()
        conexion.close()

@app.get("/health")
def health_check():
    try:
        conexion = obtener_conexion()
        cursor = conexion.cursor()
        cursor.execute("SELECT 1")
        cursor.fetchone()
        cursor.close()
        conexion.close()

        return {
            "status": "ok",
            "message": "API funcionando correctamente",
            "ip": obtener_ip_local()
        }
    except Exception as e:
        return {
            "status": "error",
            "message": f"Error de conexión a MySQL: {e}"
        }

if __name__ == "__main__":
    ip = obtener_ip_local()
    print("\n" + "=" * 60)
    print("🎯 INICIANDO SERVIDOR API - QUIZ MULTIJUGADOR")
    print("=" * 60)
    print(f"📍 IP Local del servidor: {ip}")
    print(f"🌐 API disponible en: http://{ip}:8000")
    print(f"📚 Documentación Swagger: http://{ip}:8000/docs")
    print("=" * 60)
    print("\n📋 Endpoints disponibles:")
    print("   GET  /categorias                - Lista todas las categorías")
    print("   GET  /preguntas                 - Todas las preguntas")
    print("   GET  /preguntas?categoria=1     - Preguntas por categoría")
    print("   GET  /preguntas/{id}            - Pregunta específica")
    print("   GET  /verificar-categoria/{id}  - Verifica si categoría tiene preguntas")
    print("   GET  /health                    - Verifica estado de la API")
    print("=" * 60)
    print(f"\n⚠️  IMPORTANTE: Los clientes deben apuntar a http://{ip}:8000")
    print("    No usar 127.0.0.1 si hay jugadores en otras máquinas")
    print("=" * 60 + "\n")

    # host="0.0.0.0" permite conexiones remotas
    uvicorn.run(app, host="0.0.0.0", port=8000)