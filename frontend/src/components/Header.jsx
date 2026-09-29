import { LANDING_SECTION_IDS, MAIN_CONTENT_ID } from "../constants/pageAnchors.js";
import Button from "./Button.jsx";
import styles from "./Header.module.css";
import Logo from "./Logo.jsx";
import ThemeToggle from "./ThemeToggle.jsx";

const SECTION_LINKS = [
  { href: `#${LANDING_SECTION_IDS.howItWorks}`, label: "Cómo funciona" },
  { href: `#${LANDING_SECTION_IDS.audiences}`, label: "Para quién es" },
];

export default function Header({ showSectionLinks = false }) {
  return (
    <header className={styles.header}>
      <a href={`#${MAIN_CONTENT_ID}`} className={styles.skipLink}>
        Saltar al contenido
      </a>

      <div className={`container ${styles.inner}`}>
        <Logo />
        {showSectionLinks && <SectionNav />}

        <div className={styles.actions}>
          <ThemeToggle />
          <Button to="/login" variant="ghost" size="sm">
            <span className="hide-from-sm">Entrar</span>
            <span className="hide-below-sm">Iniciar sesión</span>
          </Button>
          <div className="hide-below-sm">
            <Button to="/registro" size="sm">
              Crear cuenta
            </Button>
          </div>
        </div>
      </div>
    </header>
  );
}

function SectionNav() {
  return (
    <nav aria-label="Secciones" className={styles.nav}>
      <ul className={styles.navList}>
        {SECTION_LINKS.map((link) => (
          <li key={link.href}>
            <a href={link.href} className={styles.navLink}>
              {link.label}
            </a>
          </li>
        ))}
      </ul>
    </nav>
  );
}
