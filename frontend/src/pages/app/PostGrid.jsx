import { Link } from "react-router-dom";
import { FileText, Heart, MessageCircle } from "lucide-react";
import { postPath } from "../../constants/routes.js";
import { formatCount } from "../../utils/relativeTime.js";
import styles from "./PostGrid.module.css";

export default function PostGrid({ posts }) {
  return (
    <ul className={styles.grid}>
      {posts.map((post) => {
        const isPdf = post.adjuntoTipoMime === "application/pdf";
        const hasImage = post.imagenUrl && !isPdf;
        return (
          <li key={post.id}>
            <Link
              to={postPath(post.id)}
              className={styles.cell}
              aria-label={`Publicación de ${post.autor.nombreVisible}: ${post.contenido.slice(0, 80)}`}
            >
              {hasImage ? (
                <img
                  src={post.imagenUrl}
                  alt=""
                  loading="lazy"
                  decoding="async"
                  className={styles.image}
                />
              ) : (
                <span className={styles.textTile}>
                  {isPdf && <FileText aria-hidden="true" className={styles.tileIcon} />}
                  <span className={styles.tileText}>{post.contenido}</span>
                </span>
              )}
              <span className={styles.overlay} aria-hidden="true">
                <span className={styles.stat}>
                  <Heart className={styles.statIcon} /> {formatCount(post.totalReacciones)}
                </span>
                <span className={styles.stat}>
                  <MessageCircle className={styles.statIcon} /> {formatCount(post.totalComentarios)}
                </span>
              </span>
            </Link>
          </li>
        );
      })}
    </ul>
  );
}
