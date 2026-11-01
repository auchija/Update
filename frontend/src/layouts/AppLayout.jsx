import { Outlet } from "react-router-dom";
import AppHeader from "../components/AppHeader.jsx";
import { MAIN_CONTENT_ID } from "../constants/pageAnchors.js";
import { classNames } from "../utils/classNames.js";
import styles from "./AppLayout.module.css";

/**
 * Estructura común de todas las páginas privadas:
 * barra de la app arriba y el contenido de cada página en <Outlet />.
 */
export default function AppLayout() {
  return (
    <div className={styles.page}>
      <a href={`#${MAIN_CONTENT_ID}`} className={styles.skipLink}>
        Saltar al contenido
      </a>
      <AppHeader />
      <main id={MAIN_CONTENT_ID} className={classNames("container", styles.main)}>
        <Outlet />
      </main>
    </div>
  );
}
