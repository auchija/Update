import { useState } from "react";
import { seguirPerfil } from "../../services/comunidadService.js";
import { classNames } from "../../utils/classNames.js";
import styles from "./FollowButton.module.css";

/**
 * Botón Seguir / Solicitado / Siguiendo. `person` es un perfil de la API.
 * Muestra el cambio al instante y lo deshace si el servidor falla.
 * `onChange({ loSigo, solicitudPendiente, totalSeguidores? })` avisa al padre.
 */
export default function FollowButton({ person, onChange, variant = "solid", onError }) {
  const [isSending, setIsSending] = useState(false);
  const { loSigo, solicitudPendiente, esPrivado, nombreUsuario } = person;
  const isActive = loSigo || solicitudPendiente;

  async function toggle() {
    // Dejar de seguir una cuenta privada quita el acceso: para volver hay que pedirlo otra vez.
    if (
      loSigo &&
      esPrivado &&
      !window.confirm(`¿Dejar de seguir a @${nombreUsuario}? Tendrás que enviar otra solicitud.`)
    ) {
      return;
    }

    const previous = { loSigo, solicitudPendiente };
    const follow = !isActive;
    setIsSending(true);
    onChange?.({
      loSigo: follow && !esPrivado,
      solicitudPendiente: follow && Boolean(esPrivado),
    });
    try {
      onChange?.(await seguirPerfil(nombreUsuario, follow));
    } catch (error) {
      onChange?.(previous);
      onError?.(error.message);
    } finally {
      setIsSending(false);
    }
  }

  let label = "Seguir";
  if (loSigo) label = "Siguiendo";
  else if (solicitudPendiente) label = "Solicitado";

  return (
    <button
      type="button"
      onClick={toggle}
      disabled={isSending}
      aria-pressed={isActive}
      aria-label={`${label}: @${nombreUsuario}`}
      className={classNames(
        styles.button,
        variant === "link" ? styles.link : styles.solid,
        isActive && styles.following,
      )}
    >
      {label}
    </button>
  );
}
