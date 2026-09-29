import { CircleAlert, CircleCheck, Info } from "lucide-react";
import styles from "./Alert.module.css";

const ICONS = { info: Info, success: CircleCheck, danger: CircleAlert };

/**
 * Aviso dentro de la página: error al enviar un formulario, confirmación, información.
 * variant: "info" | "success" | "danger"
 * Los de error usan role="alert": los lectores de pantalla los leen al aparecer.
 */
export default function Alert({ variant = "info", title, children }) {
  const Icon = ICONS[variant];

  return (
    <div
      role={variant === "danger" ? "alert" : "status"}
      className={`${styles.alert} ${styles[variant]}`}
    >
      <Icon aria-hidden="true" className={`icon ${styles.icon}`} />
      <div className={styles.body}>
        {title && <p className={styles.title}>{title}</p>}
        {children && <div>{children}</div>}
      </div>
    </div>
  );
}
