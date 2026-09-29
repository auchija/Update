import { useEffect, useState } from "react";
import { Monitor, Moon, Sun } from "lucide-react";
import styles from "./ThemeToggle.module.css";

const OPTIONS = [
  { value: "light", label: "Tema claro", Icon: Sun },
  { value: "dark", label: "Tema oscuro", Icon: Moon },
  { value: "system", label: "Usar el tema del sistema", Icon: Monitor },
];

// Algunos navegadores bloquean localStorage (modo privado estricto) y leerlo lanza un error.
function readSavedTheme() {
  try {
    return localStorage.getItem("theme") || "system";
  } catch {
    return "system";
  }
}

/**
 * Selector de tema: claro, oscuro o el del sistema.
 * El modo oscuro se activa poniendo la clase "dark" en <html>; global.css
 * cambia el valor de las variables de color cuando esa clase está presente.
 * La elección se guarda en localStorage (index.html la lee al cargar).
 */
export default function ThemeToggle() {
  const [theme, setTheme] = useState(readSavedTheme);

  useEffect(() => {
    try {
      localStorage.setItem("theme", theme);
    } catch {
      // Si el navegador bloquea localStorage, el tema funciona pero no se recuerda.
    }

    const systemDark = window.matchMedia("(prefers-color-scheme: dark)");
    function applyTheme() {
      const isDark = theme === "dark" || (theme === "system" && systemDark.matches);
      document.documentElement.classList.toggle("dark", isDark);
    }
    applyTheme();

    // En modo "sistema", si el usuario cambia el tema de su computador, lo seguimos.
    systemDark.addEventListener("change", applyTheme);
    return () => systemDark.removeEventListener("change", applyTheme);
  }, [theme]);

  return (
    <div role="group" aria-label="Tema" className={styles.toggle}>
      {OPTIONS.map(({ value, label, Icon }) => (
        <button
          key={value}
          type="button"
          onClick={() => setTheme(value)}
          aria-pressed={theme === value}
          title={label}
          className={styles.option}
        >
          <Icon aria-hidden="true" className="icon icon--sm" />
          <span className="sr-only">{label}</span>
        </button>
      ))}
    </div>
  );
}
