import { Link } from "react-router-dom";
import { SITE_NAME } from "../config.js";
import styles from "./Logo.module.css";

/** Marca de la app: dos nodos unidos por un trazo mostaza, más el nombre. Lleva a la portada. */
export default function Logo() {
  return (
    <Link to="/" className={styles.logo} aria-label={`${SITE_NAME}, ir al inicio`}>
      <svg viewBox="0 0 28 28" aria-hidden="true" className={styles.mark}>
        <circle cx="8" cy="20" r="5" className={styles.node} />
        <circle cx="20" cy="8" r="5" strokeWidth="2.5" className={styles.ring} />
        <path
          d="M11.5 16.5 16.5 11.5"
          strokeWidth="3"
          strokeLinecap="round"
          className={styles.link}
        />
      </svg>
      <span>{SITE_NAME}</span>
    </Link>
  );
}
