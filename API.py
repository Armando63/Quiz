from fastapi import FastAPI

app = FastAPI()

@app.get("/conexion")
def conexion():
    return {
        "ip": "192.168.1.5",  # ⚠️ cambia por tu IP real
        "puerto": 5000
    }

@app.get("/preguntas")
def preguntas():
    return [
        {
            "texto": "¿Capital de México?",
            "opciones": ["CDMX", "Guadalajara", "Monterrey"],
            "correcta": 0
        },
        {
            "texto": "¿2 + 2?",
            "opciones": ["3", "4", "5"],
            "correcta": 1
        }
    ]