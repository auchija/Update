import { Link } from "react-router-dom";
import { classNames } from "../utils/classNames.js";
import styles from "./Button.module.css";
import Spinner from "./Spinner.jsx";

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
  const className = classNames(
    styles.button,
    styles[variant],
    styles[size],
    fullWidth && styles.fullWidth,
  );

  if (to) {
    return (
      <Link to={to} className={className}>
        {children}
      </Link>
    );
  }

  if (href) {
    return (
      <a href={href} className={className}>
        {children}
      </a>
    );
  }

  return (
    <button
      type={type}
      className={className}
      disabled={disabled || isLoading}
      aria-busy={isLoading || undefined}
      {...props}
    >
      {isLoading && <Spinner size="sm" />}
      {children}
    </button>
  );
}
