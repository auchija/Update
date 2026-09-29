import styles from "./Card.module.css";

/**
 * Tarjeta: fondo, borde y esquinas redondeadas.
 * - as: etiqueta HTML a usar ("div", "article", "section"...)
 * - elevated: sombra más marcada
 * - compact: menos espacio interior
 * - className: clase extra para colocarla o darle layout (p. ej. "stack")
 */
export default function Card({
  as: Tag = "div",
  elevated = false,
  compact = false,
  className,
  children,
  ...props
}) {
  const classes = [styles.card, compact && styles.compact, elevated && styles.elevated, className]
    .filter(Boolean)
    .join(" ");

  return (
    <Tag className={classes} {...props}>
      {children}
    </Tag>
  );
}
