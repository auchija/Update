import { useId } from "react";
import { CircleAlert } from "lucide-react";
import styles from "./Field.module.css";

/**
 * Grupo de casillas para elegir VARIAS opciones (p. ej. las "personas" del registro).
 * Usa <fieldset> y <legend>: así los lectores de pantalla leen la pregunta del grupo.
 *
 * - options: lista de { value, label }
 * - value: lista con los valores marcados, p. ej. ["entrepreneur", "mentor"]
 * - onChange(nuevaLista): se llama con la lista actualizada
 */
export default function CheckboxGroup({
  legend,
  hint,
  name,
  options,
  value,
  onChange,
  error,
  required,
}) {
  const id = useId();
  const hintId = `${id}-hint`;
  const errorId = `${id}-error`;
  const describedBy = [error && errorId, hint && hintId].filter(Boolean).join(" ") || undefined;

  // Marca o desmarca una opción y devuelve la lista nueva.
  function toggle(optionValue) {
    if (value.includes(optionValue)) {
      onChange(value.filter((v) => v !== optionValue));
    } else {
      onChange([...value, optionValue]);
    }
  }

  return (
    <fieldset className={styles.fieldset} aria-describedby={describedBy}>
      <legend className={styles.label}>
        {legend}
        {required && (
          <span aria-hidden="true" className={styles.required}>
            {" "}
            *
          </span>
        )}
      </legend>

      {hint && (
        <p id={hintId} className={styles.hint}>
          {hint}
        </p>
      )}

      <div className={styles.options}>
        {options.map((option) => (
          <div key={option.value} className={styles.checkRow}>
            <input
              id={`${id}-${option.value}`}
              type="checkbox"
              name={name}
              value={option.value}
              checked={value.includes(option.value)}
              onChange={() => toggle(option.value)}
              aria-invalid={error ? true : undefined}
              className={styles.checkbox}
            />
            <label htmlFor={`${id}-${option.value}`} className={styles.checkLabel}>
              {option.label}
            </label>
          </div>
        ))}
      </div>

      {error && (
        <p id={errorId} className={styles.error}>
          <CircleAlert aria-hidden="true" className={`icon icon--sm ${styles.errorIcon}`} />
          <span>{error}</span>
        </p>
      )}
    </fieldset>
  );
}
