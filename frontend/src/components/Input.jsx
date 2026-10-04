import { Check } from "lucide-react";
import { useFieldIds } from "../hooks/useFieldIds.js";
import { classNames } from "../utils/classNames.js";
import styles from "./Field.module.css";
import { FieldError, FieldHint, RequiredMark } from "./FieldParts.jsx";

export default function Input({ label, hint, error, endSlot, required, validator, value, ...props }) {
  const { id, hintId, errorId, describedBy } = useFieldIds({ hint, error });

  const isValid = value && !error && validator && validator(value);
  const checkmark = isValid && (
  <Check 
    size={22} 
    strokeWidth={3}
    strokeLinecap="butt"
    className={styles.checkmark} 
  />
);
  const finalEndSlot = checkmark || endSlot;

  return (
    <div className={styles.field}>
      <label htmlFor={id} className={styles.label}>
        {label}
        {required && <RequiredMark />}
      </label>

      <div className={styles.controlWrap}>
        <input
          id={id}
          value={value}
          required={required}
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy}
          className={classNames(styles.control, finalEndSlot && styles.withEnd)}
          {...props}
          
        />
        {finalEndSlot && <div className={styles.end}>{finalEndSlot}</div>}
      </div>

      <FieldHint id={hintId}>{hint}</FieldHint>
      <FieldError id={errorId}>{error}</FieldError>
    </div>
  );
}
