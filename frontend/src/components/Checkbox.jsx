import { useFieldIds } from "../hooks/useFieldIds.js";
import styles from "./Field.module.css";
import { FieldError } from "./FieldParts.jsx";

export default function Checkbox({ label, error, ...props }) {
  const { id, errorId, describedBy } = useFieldIds({ error });

  return (
    <div className={styles.field}>
      <div className={styles.checkRow}>
        <input
          id={id}
          type="checkbox"
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy}
          className={styles.checkbox}
          {...props}
        />
        <label htmlFor={id} className={styles.checkLabel}>
          {label}
        </label>
      </div>

      <FieldError id={errorId}>{error}</FieldError>
    </div>
  );
}
