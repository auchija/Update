import { useId } from "react";
import { ChevronDown, CircleAlert } from "lucide-react";
import styles from "./Field.module.css";

/**
 * Lista desplegable (<select> nativo) con label, ayuda y error.
 * `options` es una lista de { value, label }.
 */
export default function Select({ label, options, placeholder, hint, error, required, ...props }) {
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
        <select
          id={id}
          required={required}
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy}
          className={`${styles.control} ${styles.select}`}
          {...props}
        >
          {placeholder && <option value="">{placeholder}</option>}
          {options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
        <ChevronDown aria-hidden="true" className={`icon ${styles.chevron}`} />
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
