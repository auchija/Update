import { useEffect, useRef } from "react";
import { X } from "lucide-react";
import styles from "./Modal.module.css";

/**
 * Ventana modal con <dialog>: el navegador se encarga del foco, de Escape y del fondo inerte.
 * Se cierra con la X, con Escape o haciendo clic fuera del contenido.
 */
export default function Modal({ open, onClose, title, children }) {
  const dialogRef = useRef(null);

  useEffect(() => {
    const dialog = dialogRef.current;
    if (!dialog) return;
    if (open && !dialog.open) dialog.showModal();
    if (!open && dialog.open) dialog.close();
  }, [open]);

  function handleClick(event) {
    // El clic en el fondo llega al propio <dialog>; dentro del contenido llega a sus hijos.
    if (event.target === dialogRef.current) onClose();
  }

  return (
    <dialog
      ref={dialogRef}
      className={styles.dialog}
      onClose={onClose}
      onClick={handleClick}
      aria-labelledby="modal-titulo"
    >
      <div className={styles.panel}>
        <header className={styles.header}>
          <h2 id="modal-titulo" className={styles.title}>
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
