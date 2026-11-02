import { MAIN_CONTENT_ID } from "../../constants/pageAnchors.js";
import Logo from "../../components/Logo.jsx";
import { SITE_NAME } from "../../config.js";
import styles from "./Auth.module.css";

export default function AuthLayout({ pageTitle, children }) {
  return (
    <div className={styles.page}>
      <title>{`${pageTitle} | ${SITE_NAME}`}</title>

      <aside className={styles.panelSide}>
        <div className={styles.panel}>
          <div className={styles.panelContent}>
            <div className={styles.panelBrand}>
              <Logo />
            </div>
            <h2 className={styles.panelTitle}>
              Construye tu proyecto
              <br />
              con las personas correctas
            </h2>
          </div>
        </div>
      </aside>

      <main id={MAIN_CONTENT_ID} className={styles.formSide}>
        <div className={styles.mobileBrand}>
          <Logo />
        </div>
        <div className={styles.card}>{children}</div>
      </main>
    </div>
  );
}