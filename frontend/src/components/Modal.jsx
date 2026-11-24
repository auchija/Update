import { useEffect, useId, useRef } from "react";
import { X } from "lucide-react";
import styles from "./Modal.module.css";

export default function Modal({ open, onClose, title, children }) {
  const dialogRef = useRef(null);
  const titleId = useId();

  useEffect(() => {
    const dialog = dialogRef.current;
    if (!dialog) return;
    if (open && !dialog.open) dialog.showModal();
    if (!open && dialog.open) dialog.close();
  }, [open]);

  function handleClick(event) {
    if (event.target === dialogRef.current) onClose();
  }

  return (
    <dialog
      ref={dialogRef}
      className={styles.dialog}
      onClose={onClose}
      onClick={handleClick}
      aria-labelledby={titleId}
    >
      <div className={styles.panel}>
        <header className={styles.header}>
          <h2 id={titleId} className={styles.title}>
            {title}
          </h2>
          <button type="button" onClick={onClose} className={styles.close} title="Cerrar">
            <X aria-hidden="true" className="icon" />
            <span className="sr-only">Cerrar</span>
          </button>
        </header>
        <div className={styles.body}>{open && children}</div>
      </div>
    </dialog>
  );
}
