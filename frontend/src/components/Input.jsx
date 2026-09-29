import { useId } from "react";
import { CircleAlert } from "lucide-react";
import styles from "./Field.module.css";

/**
 * Campo de texto con su label, un texto de ayuda opcional y el mensaje de error.
 * Conecta la accesibilidad por sí solo:
 * - htmlFor / id: al hacer clic en el label se enfoca el campo
 * - aria-invalid: avisa a los lectores de pantalla que el campo tiene un error
 * - aria-describedby: hace que lean también la ayuda y el error
 *
 * `endSlot` permite poner algo a la derecha dentro del campo (el ojo de la contraseña).
 */
export default function Input({ label, hint, error, endSlot, required, ...props }) {
  const id = useId();
  const hintId = `${id}-hint`;
  const errorId = `${id}-error`;
  const describedBy = [error && errorId, hint && hintId].filter(Boolean).join(" ") || undefined;

  return (
    <div className={styles.field}>
      <label htmlFor={id} className={styles.label}>
        {label}
        {required && (
          <span aria-hidden="true" className={styles.required}>
            {" "}
            *
          </span>
        )}
      </label>

      <div className={styles.controlWrap}>
        <input
          id={id}
          required={required}
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy}
          className={endSlot ? `${styles.control} ${styles.withEnd}` : styles.control}
          {...props}
        />
        {endSlot && <div className={styles.end}>{endSlot}</div>}
      </div>

      {hint && (
        <p id={hintId} className={styles.hint}>
          {hint}
        </p>
      )}
      {error && (
        <p id={errorId} className={styles.error}>
          <CircleAlert aria-hidden="true" className={`icon icon--sm ${styles.errorIcon}`} />
          <span>{error}</span>
        </p>
      )}
    </div>
  );
}
