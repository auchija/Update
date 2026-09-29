import { MAIN_CONTENT_ID } from "../constants/pageAnchors.js";
import Footer from "./Footer.jsx";
import Header from "./Header.jsx";
import styles from "./PageLayout.module.css";

export default function PageLayout({ title, showSectionLinks = false, mainClassName, children }) {
  return (
    <div className={styles.page}>
      <title>{title}</title>
      <Header showSectionLinks={showSectionLinks} />
      <main id={MAIN_CONTENT_ID} className={mainClassName}>
        {children}
      </main>
      <Footer />
    </div>
  );
}
