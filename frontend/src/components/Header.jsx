import Button from "./Button.jsx";
import styles from "./Header.module.css";
import Logo from "./Logo.jsx";
import ThemeToggle from "./ThemeToggle.jsx";

const SECTION_LINKS = [
  { href: "#como-funciona", label: "Cómo funciona" },
  { href: "#para-quien", label: "Para quién es" },
];

/**
 * Cabecera fija de las páginas públicas.
 * `showSectionLinks` muestra los enlaces a las secciones de la portada (solo en la landing).
 * En móvil se ocultan esos enlaces y el botón "Crear cuenta" para que todo quepa en 360 px.
 */
export default function Header({ showSectionLinks = false }) {
  return (
    <header className={styles.header}>
      {/* Primer elemento al pulsar Tab: permite saltar la cabecera e ir directo al contenido.
          Cada página pone id="contenido" en su <main>. */}
      <a href="#contenido" className={styles.skipLink}>
        Saltar al contenido
      </a>
      <div className={`container ${styles.inner}`}>
        <Logo />

        {showSectionLinks && (
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
        )}

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
