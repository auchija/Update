import { useEffect, useState } from "react";
import { FileText, Upload, X } from "lucide-react";
import { useFieldIds } from "../hooks/useFieldIds.js";
import {
  FILE_ACCEPT,
  MAX_FILE_SIZE_MB,
  formatFileSize,
  isImage,
  validateFile,
} from "../utils/fileValidation.js";
import { classNames } from "../utils/classNames.js";
import fieldStyles from "./Field.module.css";
import { FieldError, FieldHint, RequiredMark } from "./FieldParts.jsx";
import styles from "./FileUpload.module.css";

/**
 * Selector de archivo con validación previa (tipo y peso) y vista previa.
 * Es controlado: el padre guarda el archivo y lo recibe en onChange(file | null).
 */
export default function FileUpload({
  label = "Archivo adjunto",
  hint = `JPG, PNG, WEBP o PDF. Máximo ${MAX_FILE_SIZE_MB} MB.`,
  file,
  onChange,
  error,
  required,
  disabled,
}) {
  const [localError, setLocalError] = useState("");
  const [isDragging, setIsDragging] = useState(false);
  const [preview, setPreview] = useState(null);
  const shownError = localError || error;
  const { id, hintId, errorId, describedBy } = useFieldIds({ hint, error: shownError });

  // Lee la imagen para la vista previa. El setState va dentro del callback
  // (no directo en el efecto) y se guarda junto al archivo para no mostrar uno viejo.
  useEffect(() => {
    if (!file || !isImage(file.type)) return undefined;
    const reader = new FileReader();
    reader.onload = () => setPreview({ file, url: reader.result });
    reader.readAsDataURL(file);
    return () => reader.abort();
  }, [file]);

  const previewUrl = preview?.file === file ? preview.url : null;

  function selectFile(selected) {
    if (!selected) return;
    const validationError = validateFile(selected);
    setLocalError(validationError);
    if (!validationError) onChange(selected);
  }

  function handleInputChange(event) {
    selectFile(event.target.files?.[0]);
    // Permite volver a elegir el mismo archivo después de quitarlo.
    event.target.value = "";
  }

  function handleDrop(event) {
    event.preventDefault();
    setIsDragging(false);
    if (!disabled) selectFile(event.dataTransfer.files?.[0]);
  }

  function handleRemove() {
    setLocalError("");
    onChange(null);
  }

  return (
    <div className={fieldStyles.field}>
      <span className={fieldStyles.label}>
        {label}
        {required && <RequiredMark />}
      </span>

      {file ? (
        <div className={styles.preview}>
          {previewUrl ? (
            <img src={previewUrl} alt={`Vista previa de ${file.name}`} className={styles.thumb} />
          ) : (
            <div className={styles.fileIcon}>
              <FileText aria-hidden="true" className="icon" />
            </div>
          )}
          <div className={styles.fileInfo}>
            <p className={styles.fileName}>{file.name}</p>
            <p className="text-muted">{formatFileSize(file.size)}</p>
          </div>
          <button
            type="button"
            className={styles.remove}
            onClick={handleRemove}
            disabled={disabled}
            aria-label={`Quitar ${file.name}`}
          >
            <X aria-hidden="true" className="icon icon--sm" />
          </button>
        </div>
      ) : (
        <label
          htmlFor={id}
          className={classNames(
            styles.dropzone,
            isDragging && styles.dragging,
            shownError && styles.invalid,
            disabled && styles.disabled,
          )}
          onDragOver={(event) => {
            event.preventDefault();
            setIsDragging(true);
          }}
          onDragLeave={() => setIsDragging(false)}
          onDrop={handleDrop}
        >
          <Upload aria-hidden="true" className="icon" />
          <span>
            <strong>Elige un archivo</strong> o arrástralo aquí
          </span>
          <input
            id={id}
            type="file"
            accept={FILE_ACCEPT}
            className="sr-only"
            onChange={handleInputChange}
            disabled={disabled}
            required={required}
            aria-invalid={shownError ? true : undefined}
            aria-describedby={describedBy}
          />
        </label>
      )}

      <FieldHint id={hintId}>{hint}</FieldHint>
      <FieldError id={errorId}>{shownError}</FieldError>
    </div>
  );
}
