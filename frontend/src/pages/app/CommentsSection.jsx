import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Trash2 } from "lucide-react";
import Avatar from "../../components/Avatar.jsx";
import Spinner from "../../components/Spinner.jsx";
import { userProfilePath } from "../../constants/routes.js";
import { comentar, eliminarComentario, getComentarios } from "../../services/feedService.js";
import { formatFullDate, timeAgo } from "../../utils/relativeTime.js";
import styles from "./Post.module.css";

const COMMENT_MAX_LENGTH = 1000;

export default function CommentsSection({ postId, total, defaultOpen, inputRef, onCountChange }) {
  const [isOpen, setIsOpen] = useState(defaultOpen);
  const [comments, setComments] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [text, setText] = useState("");
  const [isSending, setIsSending] = useState(false);
  const [sendError, setSendError] = useState("");

  useEffect(() => {
    if (!isOpen) return undefined;
    let cancelled = false;
    getComentarios(postId)
      .then((data) => {
        if (!cancelled) setComments(data);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });
    return () => {
      cancelled = true;
    };
  }, [isOpen, postId]);

  async function handleSubmit(event) {
    event.preventDefault();
    const contenido = text.trim();
    if (!contenido || isSending) return;

    setIsSending(true);
    setSendError("");
    try {
      const created = await comentar(postId, contenido);
      setText("");
      setComments((current) => [...(current ?? []), created]);
      onCountChange(1);
    } catch (error) {
      setSendError(error.message);
    } finally {
      setIsSending(false);
    }
  }

  async function handleDelete(comment) {
    if (!window.confirm("¿Quieres eliminar este comentario?")) return;
    try {
      await eliminarComentario(postId, comment.id);
      setComments((current) => current.filter((item) => item.id !== comment.id));
      onCountChange(-1);
    } catch (error) {
      setSendError(error.message);
    }
  }

  const shownComments = comments ?? [];
  const hiddenCount = isOpen ? 0 : total - shownComments.length;

  return (
    <div className={styles.comments}>
      {hiddenCount > 0 && (
        <button type="button" className={styles.viewComments} onClick={() => setIsOpen(true)}>
          {total === 1 ? "Ver 1 comentario" : `Ver los ${total} comentarios`}
        </button>
      )}

      {isOpen && !comments && !loadError && <Spinner size="sm" label="Cargando comentarios" />}
      {loadError && <p className={styles.inlineError}>{loadError}</p>}

      {shownComments.length > 0 && (
        <ul className={styles.commentList}>
          {shownComments.map((comment) => (
            <li key={comment.id} className={styles.comment}>
              <Link to={userProfilePath(comment.autor.nombreUsuario)} tabIndex={-1}>
                <Avatar
                  name={comment.autor.nombreVisible}
                  src={comment.autor.fotoUrl}
                  size="xs"
                  decorative
                />
              </Link>
              <div className={styles.commentBody}>
                <p className={styles.commentText}>
                  <Link
                    to={userProfilePath(comment.autor.nombreUsuario)}
                    className={styles.captionHandle}
                  >
                    {comment.autor.nombreUsuario}
                  </Link>{" "}
                  {comment.contenido}
                </p>
                <time
                  dateTime={comment.creadoEn}
                  title={formatFullDate(comment.creadoEn)}
                  className={styles.commentTime}
                >
                  {timeAgo(comment.creadoEn)}
                </time>
              </div>
              {comment.puedeEliminar && (
                <button
                  type="button"
                  className={styles.commentDelete}
                  onClick={() => handleDelete(comment)}
                  title="Eliminar comentario"
                >
                  <Trash2 aria-hidden="true" className="icon icon--sm" />
                  <span className="sr-only">Eliminar comentario</span>
                </button>
              )}
            </li>
          ))}
        </ul>
      )}

      <form onSubmit={handleSubmit} className={styles.commentForm}>
        <label htmlFor={`comentario-${postId}`} className="sr-only">
          Añade un comentario
        </label>
        <input
          ref={inputRef}
          id={`comentario-${postId}`}
          type="text"
          value={text}
          onChange={(event) => setText(event.target.value)}
          placeholder="Añade un comentario…"
          maxLength={COMMENT_MAX_LENGTH}
          autoComplete="off"
          className={styles.commentInput}
        />
        {text.trim() && (
          <button type="submit" disabled={isSending} className={styles.commentSubmit}>
            {isSending ? "Publicando…" : "Publicar"}
          </button>
        )}
      </form>
      {sendError && <p className={styles.inlineError}>{sendError}</p>}
    </div>
  );
}
