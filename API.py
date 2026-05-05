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
        # Verificar si existe tabla categorias, si no, devolver categorías predefinidas
        try:
            cursor.execute("SELECT id_categoria as id, nombre FROM categorias ORDER BY nombre")
            categorias = cursor.fetchall()
            if categorias:
                return categorias
        except:
            pass
        
        # Si no hay tabla categorias, devolver categorías predefinidas
        return [
            {"id": 1, "nombre": "General"},
            {"id": 2, "nombre": "Geografía"},
            {"id": 3, "nombre": "Historia"},
            {"id": 4, "nombre": "Ciencia"},
            {"id": 5, "nombre": "Deportes"}
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
    """
    Endpoint para obtener preguntas.
    - Si se especifica categoria, devuelve solo las preguntas de esa categoría
    - Si no se especifica, devuelve todas las preguntas
    """
    conexion = obtener_conexion()
    cursor = conexion.cursor(dictionary=True)
    
    try:
        # Construir la consulta con o sin filtro
        # CORREGIDO: Usar la estructura de tu base de datos (quiz_bd)
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
            # Si no hay preguntas, devolver lista vacía
            print("⚠️ No se encontraron preguntas en la base de datos")
            return []
        
        print(f"📚 Se encontraron {len(filas)} opciones en la base de datos")
        
        # Agrupar por pregunta
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
        
        # Validar que todas las preguntas tengan respuesta correcta
        preguntas_validas = []
        for p_id, p_data in preguntas.items():
            if p_data["correcta"] is not None:
                preguntas_validas.append(p_data)
            else:
                print(f"⚠️ Advertencia: Pregunta {p_id} no tiene respuesta correcta definida")
        
        print(f"✅ Preguntas válidas: {len(preguntas_validas)}")
        
        # Convertir a lista y mezclar aleatoriamente
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
        return {
            "categoria_id": categoria_id,
            "tiene_preguntas": True,  # Asumir que sí tiene
            "total_preguntas": 0,
            "error": str(e)
        }
    finally:
        cursor.close()
        conexion.close()

@app.get("/health")
def health_check():
    """Endpoint para verificar que la API está funcionando"""
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
    print("\n" + "=" * 60)
    print("🎯 INICIANDO SERVIDOR API - QUIZ MULTIJUGADOR")
    print("=" * 60)
    print(f"📍 IP Local del servidor: {obtener_ip_local()}")
    print(f"🌐 API disponible en: http://{obtener_ip_local()}:8000")
    print(f"📚 Documentación Swagger: http://{obtener_ip_local()}:8000/docs")
    print("=" * 60)
    print("\n📋 Endpoints disponibles:")
    print("   GET  /categorias                - Lista todas las categorías")
    print("   GET  /preguntas                 - Todas las preguntas")
    print("   GET  /preguntas?categoria=1     - Preguntas por categoría")
    print("   GET  /preguntas/{id}            - Pregunta específica")
    print("   GET  /verificar-categoria/{id}  - Verifica si categoría tiene preguntas")
    print("   GET  /health                    - Verifica estado de la API")
    print("=" * 60)
    print("\n🎮 Para probar desde el navegador:")
    print(f"   http://{obtener_ip_local()}:8000/preguntas")
    print(f"   http://{obtener_ip_local()}:8000/preguntas?categoria=1")
    print("=" * 60 + "\n")
    
    # host="0.0.0.0" permite conexiones remotas
    uvicorn.run(app, host="0.0.0.0", port=8000)