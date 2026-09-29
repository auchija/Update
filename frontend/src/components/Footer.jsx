import { SITE_DESCRIPTION, SITE_NAME } from "../config.js";
import styles from "./Footer.module.css";
import Logo from "./Logo.jsx";

export default function Footer() {
  const year = new Date().getFullYear();

  return (
    <footer className={styles.footer}>
      <div className={`container ${styles.inner}`}>
        <div className={styles.brand}>
          <Logo />
          <p className="text-sm text-muted">{SITE_DESCRIPTION}</p>
        </div>
        <p className="text-sm text-muted">
          © {year} {SITE_NAME}. Proyecto académico de la Universidad Autónoma de Occidente, Cali.
        </p>
      </div>
    </footer>
  );
}
