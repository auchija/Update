import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Menu, X } from "lucide-react";
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
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  return (
    <header className={styles.header}>
      <a href={`#${MAIN_CONTENT_ID}`} className={styles.skipLink}>
        Saltar al contenido
      </a>

      <div className={styles.inner}>
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
          
          <button
            className={styles.mobileMenuButton}
            onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
            aria-label="Menú"
          >
            <AnimatePresence mode="wait">
              {mobileMenuOpen ? (
                <motion.div
                  key="close"
                  initial={{ rotate: -90, opacity: 0 }}
                  animate={{ rotate: 0, opacity: 1 }}
                  exit={{ rotate: 90, opacity: 0 }}
                  transition={{ duration: 0.2 }}
                >
                  <X size={24} />
                </motion.div>
              ) : (
                <motion.div
                  key="open"
                  initial={{ rotate: 90, opacity: 0 }}
                  animate={{ rotate: 0, opacity: 1 }}
                  exit={{ rotate: -90, opacity: 0 }}
                  transition={{ duration: 0.2 }}
                >
                  <Menu size={24} />
                </motion.div>
              )}
            </AnimatePresence>
          </button>
        </div>
      </div>

      <AnimatePresence>
        {mobileMenuOpen && showSectionLinks && (
          <motion.nav
            initial={{ opacity: 0, height: 0 }}
            animate={{ opacity: 1, height: "auto" }}
            exit={{ opacity: 0, height: 0 }}
            transition={{ duration: 0.3 }}
            className={styles.mobileMenu}
          >
            <ul className={styles.mobileMenuList}>
              {SECTION_LINKS.map((link) => (
                <motion.li
                  key={link.href}
                  initial={{ opacity: 0, x: -10 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.2 }}
                >
                  <a
                    href={link.href}
                    className={styles.mobileMenuLink}
                    onClick={() => setMobileMenuOpen(false)}
                  >
                    {link.label}
                  </a>
                </motion.li>
              ))}
            </ul>
          </motion.nav>
        )}
      </AnimatePresence>
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