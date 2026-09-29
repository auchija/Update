import { LoaderCircle } from "lucide-react";
import styles from "./Spinner.module.css";

/**
 * Indicador de carga. Con `label`, los lectores de pantalla lo anuncian;
 * sin él es decorativo (por ejemplo, dentro de un botón).
 */
export default function Spinner({ size = "md", label }) {
  const icon = <LoaderCircle aria-hidden="true" className={`${styles.spinner} ${styles[size]}`} />;

  if (!label) return icon;

  return (
    <span role="status">
      {icon}
      <span className="sr-only">{label}</span>
    </span>
  );
}
