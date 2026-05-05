import mysql.connector
import socket
from fastapi import FastAPI, HTTPException, Query
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
from typing import Optional

app = FastAPI()

# Permitir CORS para conexiones desde C#
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
    """Endpoint para obtener todas las categorías disponibles"""
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)
    
    try:
        cursor.execute("SELECT id_categoria as id, nombre FROM categorias ORDER BY nombre")
        categorias = cursor.fetchall()
        return categorias
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error al obtener categorías: {str(e)}")
    finally:
        cursor.close()
        conexion.close()

@app.get("/preguntas")
def obtener_preguntas(
    categoria: Optional[int] = Query(None, description="ID de la categoría para filtrar")
):
    """
    Endpoint para obtener preguntas.
    - Si se especifica categoria, devuelve solo las preguntas de esa categoría
    - Si no se especifica, devuelve todas las preguntas
    """
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)
    
    try:
        # Construir la consulta con o sin filtro
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
            # Si no hay preguntas en esa categoría
            if categoria:
                raise HTTPException(status_code=404, detail=f"No hay preguntas en la categoría {categoria}")
            else:
                return []  # Lista vacía si no hay preguntas en general
        
        # Agrupar por pregunta
        preguntas = {}
        
        for fila in filas:
            id_p = fila["id_pregunta"]
            
            if id_p not in preguntas:
                preguntas[id_p] = {
                    "texto": fila["texto"],
                    "opciones": [],
                    "correcta": None,
                    "tipo_opciones": []  # Para saber si es texto, imagen o audio
                }
            
            # Detectar tipo de opción (por extensión de archivo)
            contenido = fila["contenido"]
            if contenido.endswith(('.jpg', '.jpeg', '.png', '.gif', '.bmp')):
                tipo = "imagen"
            elif contenido.endswith(('.mp3', '.wav', '.ogg')):
                tipo = "audio"
            else:
                tipo = "texto"
            
            preguntas[id_p]["opciones"].append(contenido)
            preguntas[id_p]["tipo_opciones"].append(tipo)
            
            if fila["es_correcta"]:
                preguntas[id_p]["correcta"] = len(preguntas[id_p]["opciones"]) - 1
        
        # Convertir a lista y mezclar aleatoriamente
        lista_preguntas = list(preguntas.values())
        
        # Mezclar preguntas (opcional, para que no siempre salgan en el mismo orden)
        import random
        random.shuffle(lista_preguntas)
        
        return lista_preguntas
        
    except HTTPException:
        raise
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error al obtener preguntas: {str(e)}")
    finally:
        cursor.close()
        conexion.close()

@app.get("/preguntas/{pregunta_id}")
def obtener_pregunta_por_id(pregunta_id: int):
    """Endpoint para obtener una pregunta específica por su ID"""
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
    """Endpoint para verificar si una categoría tiene preguntas"""
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
        raise HTTPException(status_code=500, detail=str(e))
    finally:
        cursor.close()
        conexion.close()

if __name__ == "__main__":
    print("=" * 50)
    print("🎯 INICIANDO SERVIDOR API - QUIZ MULTIJUGADOR")
    print("=" * 50)
    print(f"📍 IP Local del servidor: {obtener_ip_local()}")
    print(f"🌐 API disponible en: http://{obtener_ip_local()}:8000")
    print(f"🔗 Documentación automática: http://{obtener_ip_local()}:8000/docs")
    print("=" * 50)
    print("📋 Endpoints disponibles:")
    print("   GET  /categorias                    - Lista todas las categorías")
    print("   GET  /preguntas?categoria={id}      - Preguntas por categoría")
    print("   GET  /preguntas                     - Todas las preguntas")
    print("   GET  /preguntas/{id}                - Pregunta específica")
    print("   GET  /verificar-categoria/{id}      - Verifica si categoría tiene preguntas")
    print("=" * 50)
    
    # host="0.0.0.0" permite conexiones remotas
    uvicorn.run(app, host="0.0.0.0", port=8000)