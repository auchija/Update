import styles from "./Avatar.module.css";

// "Valentina Ospina" → "VO"
function getInitials(name) {
  return name
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((word) => word[0].toUpperCase())
    .join("");
}

/**
 * Foto de perfil circular. Sin `src` muestra las iniciales del nombre.
 * Usa `decorative` cuando el nombre ya aparece escrito al lado, para que
 * los lectores de pantalla no lo lean dos veces.
 */
export default function Avatar({ name, src, size = "md", decorative = false }) {
  const classes = `${styles.avatar} ${styles[size]}`;

  if (src) {
    return <img src={src} alt={decorative ? "" : name} className={classes} />;
  }

  if (decorative) {
    return (
      <span aria-hidden="true" className={classes}>
        {getInitials(name)}
      </span>
    );
  }

  return (
    <span role="img" aria-label={name} className={classes}>
      {getInitials(name)}
    </span>
  );
}
