import styles from "./Badge.module.css";

/**
 * Etiqueta corta: etapa de un proyecto, sector, estado.
 * variant: "neutral" | "primary" | "accent" | "success" | "danger"
 */
export default function Badge({ variant = "neutral", children }) {
  return <span className={`${styles.badge} ${styles[variant]}`}>{children}</span>;
}
