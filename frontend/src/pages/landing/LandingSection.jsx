import { classNames } from "../../utils/classNames.js";
import styles from "./LandingSection.module.css";

export default function LandingSection({ id, title, description, sunken = false, children }) {
  const titleId = `${id}-titulo`;

  return (
    <section
      id={id}
      aria-labelledby={titleId}
      className={classNames(styles.section, sunken && styles.sunken)}
    >
      <div className={`container ${styles.inner}`}>
        <div className={styles.header}>
          <h2 id={titleId} className={styles.title}>
            {title}
          </h2>
          <p className="text-muted">{description}</p>
        </div>
        {children}
      </div>
    </section>
  );
}
