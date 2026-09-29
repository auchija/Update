import { useFieldIds } from "../hooks/useFieldIds.js";
import styles from "./Field.module.css";
import { FieldError, FieldHint, RequiredMark } from "./FieldParts.jsx";

export default function CheckboxGroup({
  legend,
  hint,
  name,
  options,
  value: selectedValues,
  onChange,
  error,
  required,
}) {
  const { id, hintId, errorId, describedBy } = useFieldIds({ hint, error });

  function toggleOption(optionValue) {
    const nextValues = selectedValues.includes(optionValue)
      ? selectedValues.filter((selected) => selected !== optionValue)
      : [...selectedValues, optionValue];
    onChange(nextValues);
  }

  return (
    <fieldset className={styles.fieldset} aria-describedby={describedBy}>
      <legend className={styles.label}>
        {legend}
        {required && <RequiredMark />}
      </legend>

      <FieldHint id={hintId}>{hint}</FieldHint>

      <div className={styles.options}>
        {options.map((option) => {
          const optionId = `${id}-${option.value}`;
          return (
            <div key={option.value} className={styles.checkRow}>
              <input
                id={optionId}
                type="checkbox"
                name={name}
                value={option.value}
                checked={selectedValues.includes(option.value)}
                onChange={() => toggleOption(option.value)}
                aria-invalid={error ? true : undefined}
                className={styles.checkbox}
              />
              <label htmlFor={optionId} className={styles.checkLabel}>
                {option.label}
              </label>
            </div>
          );
        })}
      </div>

      <FieldError id={errorId}>{error}</FieldError>
    </fieldset>
  );
}
