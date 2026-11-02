import { useFieldIds } from "../hooks/useFieldIds.js";
import { classNames } from "../utils/classNames.js";
import styles from "./Field.module.css";
import { FieldError, FieldHint, RequiredMark } from "./FieldParts.jsx";

export default function Textarea({ label, hint, error, required, ...props }) {
  const { id, hintId, errorId, describedBy } = useFieldIds({ hint, error });

  return (
    <div className={styles.field}>
      <label htmlFor={id} className={styles.label}>
        {label}
        {required && <RequiredMark />}
      </label>

      <textarea
        id={id}
        required={required}
        aria-invalid={error ? true : undefined}
        aria-describedby={describedBy}
        className={classNames(styles.control, styles.textarea)}
        {...props}
      />

      <FieldHint id={hintId}>{hint}</FieldHint>
      <FieldError id={errorId}>{error}</FieldError>
    </div>
  );
}
