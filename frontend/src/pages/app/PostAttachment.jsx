import { FileText } from "lucide-react";
import styles from "./FeedPage.module.css";

/** Muestra el adjunto de una publicación: la imagen, o un enlace si es PDF. */
export default function PostAttachment({ url, mimeType, alt }) {
  if (!url) return null;

  if (mimeType === "application/pdf") {
    return (
      <a href={url} target="_blank" rel="noopener noreferrer" className={styles.document}>
        <FileText aria-hidden="true" className="icon" />
        <span>Ver documento PDF</span>
      </a>
    );
  }

  return <img src={url} alt={alt} loading="lazy" className={styles.image} />;
}
