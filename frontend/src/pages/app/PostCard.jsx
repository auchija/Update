import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import styles from "./FeedPage.module.css";

const dateFormat = new Intl.DateTimeFormat("es-CO", { dateStyle: "medium", timeStyle: "short" });

export default function PostCard({ post, onEdit, onDelete, isDeleting }) {
  const { autor } = post;

  return (
    <article className="stack">
      <header className="cluster">
        <Avatar name={autor.nombreVisible} src={autor.fotoUrl} />
        <div>
          <p className="text-strong">{autor.nombreVisible}</p>
          <p className="text-muted text-sm">
            @{autor.nombreUsuario} ·{" "}
            <time dateTime={post.creadoEn}>{dateFormat.format(new Date(post.creadoEn))}</time>
            {post.editadoEn && " · Editada"}
          </p>
        </div>
      </header>

      <p className={styles.content}>{post.contenido}</p>

      {post.imagenUrl && (
        <img
          src={post.imagenUrl}
          alt={`Imagen de la publicación de ${autor.nombreVisible}`}
          loading="lazy"
          className={styles.image}
        />
      )}

      {post.esMia && (
        <footer className="cluster cluster--xs">
          <Button variant="secondary" size="sm" onClick={onEdit} disabled={isDeleting}>
            Editar
          </Button>
          <Button variant="danger" size="sm" onClick={onDelete} isLoading={isDeleting}>
            {isDeleting ? "Eliminando" : "Eliminar"}
          </Button>
        </footer>
      )}
    </article>
  );
}
