import { classNames } from "../utils/classNames.js";
import styles from "./Avatar.module.css";

/**
 * Foto de perfil o, si no hay, las iniciales.
 * `ring` dibuja el aro con los colores de la marca (perfiles destacados y cabeceras).
 */
export default function Avatar({ name, src, size = "md", decorative = false, ring = false }) {
  const className = classNames(styles.avatar, styles[size]);

  const content = src ? (
    <img src={src} alt={decorative ? "" : name} className={className} loading="lazy" />
  ) : (
    <span
      className={className}
      {...(decorative ? { "aria-hidden": true } : { role: "img", "aria-label": name })}
    >
      {getInitials(name)}
    </span>
  );

  if (!ring) return content;
  return <span className={classNames(styles.ring, styles[`ring-${size}`])}>{content}</span>;
}

function getInitials(name = "") {
  return (
    name
      .trim()
      .split(/\s+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((word) => word[0].toUpperCase())
      .join("") || "?"
  );
}
