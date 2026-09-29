import { CircleAlert } from "lucide-react";
import styles from "./Field.module.css";

export function RequiredMark() {
  return (
    <span aria-hidden="true" className={styles.required}>
      {" "}
      *
    </span>
  );
}

export function FieldHint({ id, children }) {
  if (!children) return null;

  return (
    <p id={id} className={styles.hint}>
      {children}
    </p>
  );
}

export function FieldError({ id, children }) {
  if (!children) return null;

  return (
    <p id={id} className={styles.error}>
      <CircleAlert aria-hidden="true" className={`icon icon--sm ${styles.errorIcon}`} />
      <span>{children}</span>
    </p>
  );
}
