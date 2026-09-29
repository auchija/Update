import { useFieldIds } from "../hooks/useFieldIds.js";
import { classNames } from "../utils/classNames.js";
import styles from "./Field.module.css";
import { FieldError, FieldHint, RequiredMark } from "./FieldParts.jsx";

export default function Input({ label, hint, error, endSlot, required, ...props }) {
  const { id, hintId, errorId, describedBy } = useFieldIds({ hint, error });

  return (
    <div className={styles.field}>
      <label htmlFor={id} className={styles.label}>
        {label}
        {required && <RequiredMark />}
      </label>

      <div className={styles.controlWrap}>
        <input
          id={id}
          required={required}
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy}
          className={classNames(styles.control, endSlot && styles.withEnd)}
          {...props}
        />
        {endSlot && <div className={styles.end}>{endSlot}</div>}
      </div>

      <FieldHint id={hintId}>{hint}</FieldHint>
      <FieldError id={errorId}>{error}</FieldError>
    </div>
  );
}
