import { useEffect, useState } from "react";

const THEME_STORAGE_KEY = "theme";
const DEFAULT_THEME = "system";

export function useTheme() {
  const [theme, setTheme] = useState(readSavedTheme);

  useEffect(() => {
    saveThemeIfPossible(theme);

    const systemPrefersDark = window.matchMedia("(prefers-color-scheme: dark)");

    function applyTheme() {
      const isDark = theme === "dark" || (theme === "system" && systemPrefersDark.matches);
      document.documentElement.classList.toggle("dark", isDark);
    }

    applyTheme();
    systemPrefersDark.addEventListener("change", applyTheme);
    return () => systemPrefersDark.removeEventListener("change", applyTheme);
  }, [theme]);

  return [theme, setTheme];
}

function readSavedTheme() {
  try {
    return localStorage.getItem(THEME_STORAGE_KEY) || DEFAULT_THEME;
  } catch {
    return DEFAULT_THEME;
  }
}

function saveThemeIfPossible(theme) {
  try {
    localStorage.setItem(THEME_STORAGE_KEY, theme);
  } catch {
    return;
  }
}
