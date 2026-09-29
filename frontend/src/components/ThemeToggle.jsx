import { Monitor, Moon, Sun } from "lucide-react";
import { useTheme } from "../hooks/useTheme.js";
import styles from "./ThemeToggle.module.css";

const THEME_OPTIONS = [
  { value: "light", label: "Tema claro", Icon: Sun },
  { value: "dark", label: "Tema oscuro", Icon: Moon },
  { value: "system", label: "Usar el tema del sistema", Icon: Monitor },
];

export default function ThemeToggle() {
  const [theme, setTheme] = useTheme();

  return (
    <div role="group" aria-label="Tema" className={styles.toggle}>
      {THEME_OPTIONS.map(({ value, label, Icon }) => (
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
