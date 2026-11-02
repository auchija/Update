import { Link } from "react-router-dom";
import { SITE_DESCRIPTION, SITE_NAME } from "../config.js";
import { ROUTES } from "../constants/routes.js";
import styles from "./Footer.module.css";
import Logo from "./Logo.jsx";

const BARS = [0, 1, 2, 3, 4, 5, 6, 7, 8];

export default function Footer() {
  const year = new Date().getFullYear();

  return (
    <footer className={styles.footer}>
      <div className="container">
        <div className={styles.card}>
          <div className={styles.bars} aria-hidden="true">
            {BARS.map((bar) => {
              const distance = Math.abs(bar - 4);
              return (
                <div
                  key={bar}
                  className={styles.bar}
                  style={{
                    height: `${Math.max(20, 90 - distance * 15)}%`,
                    opacity: Math.max(0.1, 1 - distance * 0.2),
                  }}
                />
              );
            })}
          </div>

          <div className={styles.content}>
            <div className={styles.brand}>
              <Logo />
              <p className={styles.description}>{SITE_DESCRIPTION}</p>
            </div>

            <div className={styles.columns}>
              <nav aria-label="Explorar">
                <h2 className={styles.columnTitle}>Explorar</h2>
                <ul className={styles.list}>
                  <li>
                    <a href="/#como-funciona">Cómo funciona</a>
                  </li>
                  <li>
                    <a href="/#para-quien">Para quién es</a>
                  </li>
                </ul>
              </nav>

              <nav aria-label="Tu cuenta">
                <h2 className={styles.columnTitle}>Tu cuenta</h2>
                <ul className={styles.list}>
                  <li>
                    <Link to={ROUTES.login}>Iniciar sesión</Link>
                  </li>
                  <li>
                    <Link to={ROUTES.register}>Crear cuenta</Link>
                  </li>
                </ul>
              </nav>
            </div>
          </div>
        </div>

        <p className={styles.copy}>
          © {year} {SITE_NAME}. Proyecto académico de la Universidad Autónoma de Occidente, Cali.
        </p>
      </div>
    </footer>
  );
}
