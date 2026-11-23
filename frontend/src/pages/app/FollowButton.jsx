import { useState } from "react";
import { seguirPerfil } from "../../services/comunidadService.js";
import { classNames } from "../../utils/classNames.js";
import styles from "./FollowButton.module.css";

/**
 * Botón Seguir / Siguiendo. Muestra el cambio al instante y lo deshace si el servidor falla.
 * `onChange({ loSigo, totalSeguidores })` avisa al padre.
 */
export default function FollowButton({ handle, following, onChange, variant = "solid", onError }) {
  const [isSending, setIsSending] = useState(false);

  async function toggle() {
    const next = !following;
    setIsSending(true);
    onChange?.({ loSigo: next });
    try {
      onChange?.(await seguirPerfil(handle, next));
    } catch (error) {
      onChange?.({ loSigo: !next });
      onError?.(error.message);
    } finally {
      setIsSending(false);
    }
  }

  return (
    <button
      type="button"
      onClick={toggle}
      disabled={isSending}
      aria-pressed={following}
      className={classNames(
        styles.button,
        variant === "link" ? styles.link : styles.solid,
        following && styles.following,
      )}
    >
      {following ? "Siguiendo" : "Seguir"}
    </button>
  );
}
