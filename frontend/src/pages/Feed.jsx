import { useRef, useState } from "react";
import "./Feed.css";

const LIMITE_CONTENIDO = 5000;

export default function Feed() {
  const campoContenido = useRef(null);
  const [publicaciones, setPublicaciones] = useState([]);
  const [contenido, setContenido] = useState("");
  const [publicacionEditada, setPublicacionEditada] = useState(null);
  const [error, setError] = useState("");
  const [aviso, setAviso] = useState("");

  function limpiarFormulario() {
    setContenido("");
    setPublicacionEditada(null);
    setError("");
  }

  function guardarPublicacion(evento) {
    evento.preventDefault();
    const texto = contenido.trim();
    if (!texto || texto.length > LIMITE_CONTENIDO) {
      setError(!texto ? "Escribe algo antes de publicar." : `El limite es de ${LIMITE_CONTENIDO} caracteres.`);
      campoContenido.current?.focus();
      return;
    }
    if (publicacionEditada !== null) {
      setPublicaciones((actuales) => actuales.map((publicacion) =>
        publicacion.id === publicacionEditada
          ? { ...publicacion, contenido: texto, actualizadoEn: new Date().toISOString() }
          : publicacion
      ));
      setAviso("Publicacion actualizada.");
    } else {
      const nuevaPublicacion = {
        id: crypto.randomUUID(),
        autor: "Tu perfil",
        contenido: texto,
        creadoEn: new Date().toISOString(),
        actualizadoEn: null,
      };
      setPublicaciones((actuales) => [nuevaPublicacion, ...actuales]);
      setAviso("Publicacion creada.");
    }
    limpiarFormulario();
    campoContenido.current?.focus();
  }

  function editarPublicacion(publicacion) {
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

  function eliminarPublicacion(id) {
    if (!window.confirm("Quieres eliminar esta publicacion?")) return;
    setPublicaciones((actuales) => actuales.filter((publicacion) => publicacion.id !== id));
    if (publicacionEditada === id) limpiarFormulario();
    setAviso("Publicacion eliminada.");
    campoContenido.current?.focus();
  }

  function mostrarFecha(fecha) {
    return new Intl.DateTimeFormat("es-CO", {
      dateStyle: "short",
      timeStyle: "short",
    }).format(new Date(fecha));
  }

  return (
    <main className="feed">
      <header className="feed-header">
        <h1>Publicaciones</h1>
        <p>Comparte tus ideas, proyectos y avances.</p>
      </header>
      <form className="feed-panel" onSubmit={guardarPublicacion}>
        <h2>{publicacionEditada !== null ? "Editar publicacion" : "Crear publicacion"}</h2>
        <label htmlFor="contenido-publicacion">Que quieres compartir?</label>
        <textarea
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
            {publicacionEditada !== null && <button type="button" onClick={cancelarEdicion}>Cancelar</button>}
            <button type="submit" className="feed-primary">
              {publicacionEditada !== null ? "Guardar cambios" : "Publicar"}
            </button>
          </div>
        </div>
      </form>
      <p className="feed-status" role="status" aria-live="polite">{aviso}</p>
      <section aria-label="Lista de publicaciones">
        {publicaciones.length === 0 ? (
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
            <footer className="feed-buttons">
              <button type="button" onClick={() => editarPublicacion(publicacion)}>Editar</button>
              <button type="button" className="feed-delete" onClick={() => eliminarPublicacion(publicacion.id)}>Eliminar</button>
            </footer>
          </article>
        ))}
      </section>
    </main>
  );
}
