import { useRef, useState } from "react";
import { Link } from "react-router-dom";
import { Heart, MessageCircle, MoreHorizontal, Pencil, Send, Trash2 } from "lucide-react";
import Avatar from "../../components/Avatar.jsx";
import { postPath, userProfilePath } from "../../constants/routes.js";
import { marcarMeGusta } from "../../services/feedService.js";
import { classNames } from "../../utils/classNames.js";
import { formatCount, formatFullDate, timeAgo } from "../../utils/relativeTime.js";
import CommentsSection from "./CommentsSection.jsx";
import PostAttachment from "./PostAttachment.jsx";
import styles from "./Post.module.css";

const LONG_CAPTION = 140;

/**
 * Publicación al estilo de una red social: autor, foto, acciones ("me gusta", comentar,
 * compartir), contador, texto y comentarios. Doble clic en la foto también da "me gusta".
 * `onChange(post)` avisa a la lista cuando cambian los contadores.
 */
export default function PostCard({
  post,
  onChange,
  onEdit,
  onDelete,
  isDeleting = false,
  commentsOpen = false,
}) {
  const { autor } = post;
  const [likeError, setLikeError] = useState("");
  const [isBursting, setIsBursting] = useState(false);
  const [isCaptionExpanded, setIsCaptionExpanded] = useState(false);
  const [shareStatus, setShareStatus] = useState("");
  const commentInputRef = useRef(null);
  const likeRequest = useRef(0);

  const hasMedia = Boolean(post.imagenUrl);
  const isImage = hasMedia && post.adjuntoTipoMime !== "application/pdf";
  const isLongCaption = post.contenido.length > LONG_CAPTION || post.contenido.includes("\n");

  async function toggleLike(nextValue = !post.meGusta) {
    if (nextValue === post.meGusta) return;
    setLikeError("");
    const previous = post;
    // Se muestra el cambio al instante y se corrige con lo que responda el servidor.
    onChange({
      ...post,
      meGusta: nextValue,
      totalReacciones: Math.max(0, post.totalReacciones + (nextValue ? 1 : -1)),
    });

    const request = ++likeRequest.current;
    try {
      const result = await marcarMeGusta(post.id, nextValue);
      if (request === likeRequest.current) onChange({ ...previous, ...result });
    } catch (error) {
      if (request === likeRequest.current) {
        onChange(previous);
        setLikeError(error.message);
      }
    }
  }

  function handleDoubleClick() {
    setIsBursting(true);
    toggleLike(true);
  }

  async function handleShare() {
    const url = new URL(postPath(post.id), window.location.origin).href;
    try {
      if (navigator.share) {
        await navigator.share({ title: `Publicación de ${autor.nombreVisible}`, url });
        return;
      }
      await navigator.clipboard.writeText(url);
      setShareStatus("Enlace copiado");
    } catch (error) {
      // Cancelar el menú de compartir no es un error que haya que mostrar.
      if (error?.name !== "AbortError") setShareStatus("No pudimos copiar el enlace");
    }
    window.setTimeout(() => setShareStatus(""), 2500);
  }

  function changeCommentCount(delta) {
    onChange({ ...post, totalComentarios: Math.max(0, post.totalComentarios + delta) });
  }

  return (
    <article className={styles.post} aria-label={`Publicación de ${autor.nombreVisible}`}>
      <header className={styles.header}>
        <Link to={userProfilePath(autor.nombreUsuario)} className={styles.author}>
          <Avatar name={autor.nombreVisible} src={autor.fotoUrl} size="sm" ring decorative />
          <span className={styles.authorText}>
            <span className={styles.handle}>{autor.nombreUsuario}</span>
            <span className={styles.authorName}>{autor.nombreVisible}</span>
          </span>
        </Link>
        <span className={styles.dot} aria-hidden="true">
          •
        </span>
        <Link to={postPath(post.id)} className={styles.time}>
          <time dateTime={post.creadoEn} title={formatFullDate(post.creadoEn)}>
            {timeAgo(post.creadoEn)}
          </time>
          {post.editadoEn && <span> · editada</span>}
        </Link>

        {post.esMia && <PostMenu onEdit={onEdit} onDelete={onDelete} isDeleting={isDeleting} />}
      </header>

      {isImage && (
        <div className={styles.media} onDoubleClick={handleDoubleClick}>
          <PostAttachment
            url={post.imagenUrl}
            mimeType={post.adjuntoTipoMime}
            alt={`Foto de la publicación de ${autor.nombreVisible}`}
          />
          {isBursting && (
            <Heart
              aria-hidden="true"
              className={styles.burst}
              onAnimationEnd={() => setIsBursting(false)}
            />
          )}
        </div>
      )}

      {!isImage && (
        <div className={styles.textBody}>
          <p
            className={classNames(styles.bigText, post.contenido.length > 280 && styles.mediumText)}
          >
            {post.contenido}
          </p>
          {hasMedia && (
            <PostAttachment url={post.imagenUrl} mimeType={post.adjuntoTipoMime} alt="" />
          )}
        </div>
      )}

      <div className={styles.actions}>
        <button
          type="button"
          onClick={() => toggleLike()}
          aria-pressed={post.meGusta}
          className={classNames(styles.action, post.meGusta && styles.liked)}
          title={post.meGusta ? "Quitar me gusta" : "Me gusta"}
        >
          <Heart aria-hidden="true" className={styles.actionIcon} />
          <span className="sr-only">Me gusta</span>
        </button>
        <button
          type="button"
          onClick={() => commentInputRef.current?.focus()}
          className={styles.action}
          title="Comentar"
        >
          <MessageCircle aria-hidden="true" className={styles.actionIcon} />
          <span className="sr-only">Comentar</span>
        </button>
        <button type="button" onClick={handleShare} className={styles.action} title="Compartir">
          <Send aria-hidden="true" className={styles.actionIcon} />
          <span className="sr-only">Compartir</span>
        </button>
        <span role="status" className={styles.shareStatus}>
          {shareStatus}
        </span>
      </div>

      <div className={styles.details}>
        {likeError && <p className={styles.inlineError}>{likeError}</p>}

        <p className={styles.likes}>
          {post.totalReacciones === 0
            ? "Sé el primero en darle me gusta"
            : `${formatCount(post.totalReacciones)} me gusta`}
        </p>

        {isImage && post.contenido && (
          <p
            className={classNames(
              styles.caption,
              !isCaptionExpanded && isLongCaption && styles.clamped,
            )}
          >
            <Link to={userProfilePath(autor.nombreUsuario)} className={styles.captionHandle}>
              {autor.nombreUsuario}
            </Link>{" "}
            {post.contenido}
          </p>
        )}
        {isImage && isLongCaption && !isCaptionExpanded && (
          <button type="button" className={styles.more} onClick={() => setIsCaptionExpanded(true)}>
            más
          </button>
        )}

        <CommentsSection
          postId={post.id}
          total={post.totalComentarios}
          defaultOpen={commentsOpen}
          inputRef={commentInputRef}
          onCountChange={changeCommentCount}
        />
      </div>
    </article>
  );
}

/** Menú "⋯" con Editar y Eliminar, solo en las publicaciones propias. */
function PostMenu({ onEdit, onDelete, isDeleting }) {
  const menuRef = useRef(null);

  function choose(action) {
    menuRef.current.open = false;
    action();
  }

  return (
    <details ref={menuRef} className={styles.menu}>
      <summary className={styles.menuButton} title="Opciones">
        <MoreHorizontal aria-hidden="true" className="icon" />
        <span className="sr-only">Opciones de la publicación</span>
      </summary>
      <div className={styles.menuList}>
        <button type="button" onClick={() => choose(onEdit)} className={styles.menuItem}>
          <Pencil aria-hidden="true" className="icon icon--sm" />
          Editar
        </button>
        <button
          type="button"
          onClick={() => choose(onDelete)}
          disabled={isDeleting}
          className={classNames(styles.menuItem, styles.menuDanger)}
        >
          <Trash2 aria-hidden="true" className="icon icon--sm" />
          {isDeleting ? "Eliminando…" : "Eliminar"}
        </button>
      </div>
    </details>
  );
}
