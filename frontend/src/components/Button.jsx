import { Link } from "react-router-dom";
import styles from "./Button.module.css";
import Spinner from "./Spinner.jsx";

/**
 * Botón reutilizable.
 * - variant: "primary" | "secondary" | "ghost" | "danger"
 * - size: "sm" | "md" | "lg"
 * - to: enlace a otra página de la app (p. ej. "/registro"), con React Router
 * - href: enlace normal, p. ej. a una sección de la misma página ("#como-funciona")
 * - isLoading: muestra un spinner y desactiva el botón
 */
export default function Button({
  variant = "primary",
  size = "md",
  to,
  href,
  isLoading = false,
  fullWidth = false,
  type = "button",
  disabled,
  children,
  ...props
}) {
  const classes = [styles.button, styles[variant], styles[size], fullWidth && styles.fullWidth]
    .filter(Boolean)
    .join(" ");

  if (to) {
    return (
      <Link to={to} className={classes}>
        {children}
      </Link>
    );
  }

  if (href) {
    return (
      <a href={href} className={classes}>
        {children}
      </a>
    );
  }

  return (
    <button
      type={type}
      className={classes}
      disabled={disabled || isLoading}
      aria-busy={isLoading || undefined}
      {...props}
    >
      {isLoading && <Spinner size="sm" />}
      {children}
    </button>
  );
}
