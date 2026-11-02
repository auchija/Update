import { useEffect, useRef, useState } from "react";
import { USE_MOCKS, API_URL } from "../config.js";
import {
  listarPublicaciones, crearPublicacion, actualizarPublicacion, borrarPublicacion,
} from "../services/publicacionesService.js";
import "./Feed.css";

const LIMITE_CONTENIDO = 5000;

export default function Feed() {
  const campoContenido = useRef(null);
  const operacionEnCurso = useRef(false);
  const [publicaciones, setPublicaciones] = useState([]);
  const [contenido, setContenido] = useState("");
  const [publicacionEditada, setPublicacionEditada] = useState(null);
  const [error, setError] = useState("");
  const [errorCarga, setErrorCarga] = useState("");
  const [aviso, setAviso] = useState("");
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);
  const [pagina, setPagina] = useState(1);
  const [hayMas, setHayMas] = useState(false);
  const [intento, setIntento] = useState(0);
  const configurado = !USE_MOCKS && Boolean(API_URL);
  const ocupado = cargando || guardando;

  useEffect(() => {
    let vigente = true;
    async function cargar() {
      setCargando(true);
      setErrorCarga("");
      try {
        if (!configurado) throw new Error("Configura VITE_USE_MOCKS=false y VITE_API_URL para usar el backend real.");
        const lista = await listarPublicaciones(pagina);
        if (!Array.isArray(lista)) throw new Error("El servidor no devolvio una lista de publicaciones. Revisa VITE_API_URL.");
        if (!vigente) return;
        setPublicaciones((actuales) => {
          if (pagina === 1) return lista;
          const ids = new Set(actuales.map((p) => p.id));
          return [...actuales, ...lista.filter((p) => !ids.has(p.id))];
        });
        setHayMas(lista.length === 20);
      } catch (fallo) {
        if (vigente) setErrorCarga(fallo.message || "No se pudieron cargar las publicaciones.");
      } finally {
        if (vigente) setCargando(false);
      }
    }
    cargar();
    return () => { vigente = false; };
  }, [pagina, intento, configurado]);

  function limpiarFormulario() {
    setContenido("");
    setPublicacionEditada(null);
    setError("");
  }

  async function guardarPublicacion(evento) {
    evento.preventDefault();
    if (operacionEnCurso.current || ocupado || !configurado) return;
    const texto = contenido.trim();
    if (!texto || texto.length > LIMITE_CONTENIDO) {
      setError(!texto ? "Escribe algo antes de publicar." : `El limite es de ${LIMITE_CONTENIDO} caracteres.`);
      campoContenido.current?.focus();
      return;
    }
    operacionEnCurso.current = true;
    setGuardando(true);
    setError("");
    setAviso("");
    try {
      if (publicacionEditada !== null) {
        const actualizada = await actualizarPublicacion(publicacionEditada, texto);
        setPublicaciones((actuales) => actuales.map((p) => p.id === actualizada.id ? actualizada : p));
        setAviso("Cambios guardados.");
      } else {
        const nueva = await crearPublicacion(texto);
        setPublicaciones((actuales) => [nueva, ...actuales]);
        setAviso("Publicacion guardada.");
      }
      limpiarFormulario();
    } catch (fallo) {
      setError(fallo.message || "No se pudo guardar. Tu texto sigue en el formulario.");
    } finally {
      operacionEnCurso.current = false;
      setGuardando(false);
    }
  }

  function editarPublicacion(publicacion) {
    if (ocupado || !publicacion.esPropia) return;
    setContenido(publicacion.contenido);
    setPublicacionEditada(publicacion.id);
    setError("");
    setAviso("");
    campoContenido.current?.focus();
  }

  function cancelarEdicion() {
    limpiarFormulario();
    setAviso("Edicion cancelada.");
    campoContenido.current?.focus();
  }

  async function eliminarPublicacion(id) {
    if (operacionEnCurso.current || ocupado) return;
    if (!window.confirm("Quieres eliminar esta publicacion?")) return;
    operacionEnCurso.current = true;
    setGuardando(true);
    setError("");
    setAviso("");
    try {
      await borrarPublicacion(id);
      setPublicaciones((actuales) => actuales.filter((p) => p.id !== id));
      if (publicacionEditada === id) limpiarFormulario();
      setAviso("Publicacion eliminada.");
    } catch (fallo) {
      setError(fallo.message || "No se pudo eliminar la publicacion.");
    } finally {
      operacionEnCurso.current = false;
      setGuardando(false);
    }
  }

  function mostrarFecha(fecha) {
    return new Intl.DateTimeFormat("es-CO", {
      dateStyle: "short",
      timeStyle: "short",
    }).format(new Date(fecha));
  }

  return (
    <div className="feed">
      <header className="feed-header">
        <h1>Publicaciones</h1>
        <p>Comparte tus ideas, proyectos y avances.</p>
      </header>
      <form className="feed-panel" onSubmit={guardarPublicacion}>
        <h2>{publicacionEditada !== null ? "Editar publicacion" : "Crear publicacion"}</h2>
        <label htmlFor="contenido-publicacion">Que quieres compartir?</label>
        <textarea
          disabled={guardando || !configurado}
          ref={campoContenido}
          id="contenido-publicacion"
          value={contenido}
          maxLength={LIMITE_CONTENIDO}
          placeholder="Escribe tu publicacion..."
          aria-invalid={Boolean(error)}
          aria-describedby={error ? "feed-error feed-contador" : "feed-contador"}
          onChange={(evento) => {
            setContenido(evento.target.value);
            setError("");
            setAviso("");
          }}
        />
        {error && <p id="feed-error" className="feed-error" role="alert">{error}</p>}
        <div className="feed-actions">
          <small id="feed-contador">{contenido.length}/{LIMITE_CONTENIDO}</small>
          <div className="feed-buttons">
            {publicacionEditada !== null && <button type="button" disabled={ocupado} onClick={cancelarEdicion}>Cancelar</button>}
            <button type="submit" className="feed-primary" disabled={ocupado || !configurado}>
              {guardando ? "Guardando..." : publicacionEditada !== null ? "Guardar cambios" : "Publicar"}
            </button>
          </div>
        </div>
      </form>
      <p className="feed-status" role="status" aria-live="polite">{aviso}</p>
      {cargando && <p role="status">Cargando publicaciones...</p>}
      {errorCarga && (
        <div className="feed-error" role="alert">
          <p>{errorCarga}</p>
          <button type="button" disabled={ocupado} onClick={() => setIntento((valor) => valor + 1)}>Reintentar</button>
        </div>
      )}
      <section aria-label="Lista de publicaciones" aria-busy={cargando}>
        {!cargando && !errorCarga && publicaciones.length === 0 ? (
          <div className="feed-panel feed-empty">
            <h2>Todavia no hay publicaciones</h2>
            <p>Comparte tu primera idea para comenzar.</p>
          </div>
        ) : publicaciones.map((publicacion) => (
          <article className="feed-panel" key={publicacion.id}>
            <header className="feed-meta">
              <strong>{publicacion.autor}</strong>
              <small>
                <time dateTime={publicacion.creadoEn}>{mostrarFecha(publicacion.creadoEn)}</time>
                {publicacion.actualizadoEn && " - Editado"}
              </small>
            </header>
            <p className="feed-content">{publicacion.contenido}</p>
            {publicacion.esPropia && <footer className="feed-buttons">
              <button type="button" disabled={ocupado} onClick={() => editarPublicacion(publicacion)}>Editar</button>
              <button type="button" disabled={ocupado} className="feed-delete" onClick={() => eliminarPublicacion(publicacion.id)}>Eliminar</button>
            </footer>}
          </article>
        ))}
      </section>
      {hayMas && !errorCarga && (
        <button type="button" disabled={ocupado} onClick={() => setPagina((valor) => valor + 1)}>Cargar mas</button>
      )}
    </div>
  );
}
