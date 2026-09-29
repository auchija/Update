import { useId } from "react";
import { CircleAlert } from "lucide-react";
import styles from "./Field.module.css";

/** Casilla de verificación con su texto al lado (todo el texto es clicable). */
export default function Checkbox({ label, error, ...props }) {
  const id = useId();
  const errorId = `${id}-error`;

  return (
    <div className={styles.field}>
      <div className={styles.checkRow}>
        <input
          id={id}
          type="checkbox"
          aria-invalid={error ? true : undefined}
          aria-describedby={error ? errorId : undefined}
          className={styles.checkbox}
          {...props}
        />
        <label htmlFor={id} className={styles.checkLabel}>
          {label}
        </label>
      </div>
      {error && (
        <p id={errorId} className={styles.error}>
          <CircleAlert aria-hidden="true" className={`icon icon--sm ${styles.errorIcon}`} />
          <span>{error}</span>
        </p>
      )}
    </div>
  );
}
