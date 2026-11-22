"""Datos de demostracion en una base de desarrollo, usando los contratos de la API."""
import getpass
import json
import os
import sys
from urllib.error import HTTPError
from urllib.parse import urlparse
from urllib.request import Request, urlopen

base = os.getenv("UPDATE_API", "http://localhost:5080").rstrip("/")
if urlparse(base).hostname not in {"localhost", "127.0.0.1", "::1"}:
    sys.exit("Este script solo trabaja con una API local. Usa una base de pruebas.")

def llamar(ruta, datos=None, token=None):
    cuerpo = json.dumps(datos).encode() if datos is not None else None
    headers = {"Content-Type": "application/json"}
    if token:
        headers["Authorization"] = "Bearer " + token
    with urlopen(Request(base + ruta, cuerpo, headers), timeout=30) as r:
        return json.load(r)

def paginas(ruta, token):
    pagina = 1
    while True:
        lote = llamar(f"{ruta}?pagina={pagina}&tamanoPagina=100", token=token)
        yield from lote
        if len(lote) < 100:
            break
        pagina += 1

def sembrar():
    for indice in (1, 2):
        handle = f"demo_backend2_{indice}"
        correo = f"{handle}@example.test"
        clave = os.getenv(f"UPDATE_DEMO_CLAVE_{indice}") or getpass.getpass(f"Clave de prueba de {handle}: ")
        if len(clave) < 8 or not any(c.isalpha() for c in clave) or not any(c.isdigit() for c in clave):
            sys.exit("Usa al menos ocho caracteres con letras y numeros.")
        try:
            sesion = llamar("/api/auth/login", {"identifier": correo, "password": clave})
        except HTTPError as error:
            if error.code != 401:
                raise
            sesion = llamar("/api/auth/register", {"handle": handle, "email": correo,
                "password": clave, "displayName": f"Demo Backend {indice}",
                "termsAccepted": True, "personas": ["entrepreneur"]})
        token = sesion["accessToken"]
        categoria = next((c for c in paginas("/api/mis-emprendimientos/categorias", token)
                          if c["slug"] == "demo-backend2"), None)
        if not categoria:
            sys.exit("Ejecuta primero backend/demo/categoria.sql.")
        contenido = f"Primera publicacion de {handle}"
        if not any(p["contenido"] == contenido for p in paginas("/api/feed/mias", token)):
            llamar("/api/feed", {"contenido": contenido}, token)
        nombre = f"Proyecto demo {indice}"
        if not any(e["nombre"] == nombre for e in paginas("/api/mis-emprendimientos", token)):
            llamar("/api/mis-emprendimientos", {"nombre": nombre, "categoriaId": categoria["id"],
                   "etapa": "idea"}, token)
        print(f"Lista la cuenta {correo}, con su publicacion y su emprendimiento.")

if __name__ == "__main__":
    try:
        sembrar()
    except HTTPError as error:
        sys.exit(f"Error HTTP {error.code} en {error.url}: {error.read().decode()}")
