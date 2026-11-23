import { motion, useReducedMotion } from "motion/react";
import { classNames } from "../../utils/classNames.js";
import styles from "./LandingSection.module.css";

const fadeUp = {
  hidden: { opacity: 0, filter: "blur(6px)", y: 20 },
  show: {
    opacity: 1,
    filter: "blur(0px)",
    y: 0,
    transition: { duration: 0.7, ease: [0.22, 1, 0.36, 1] },
  },
};

const stagger = {
  hidden: {},
  show: { transition: { staggerChildren: 0.12 } },
};

export default function LandingSection({ id, title, description, sunken = false, children }) {
  const titleId = `${id}-titulo`;
  const reduceMotion = useReducedMotion();

  return (
    <section
      id={id}
      aria-labelledby={titleId}
      className={classNames(styles.section, sunken && styles.sunken)}
    >
      <div className={`container ${styles.inner}`}>
        <motion.div
          className={styles.header}
          variants={stagger}
          initial={reduceMotion ? false : "hidden"}
          whileInView="show"
          viewport={{ once: true, margin: "-80px" }}
        >
          <motion.h2
            id={titleId}
            className={styles.title}
            variants={reduceMotion ? {} : fadeUp}
          >
            {title}
          </motion.h2>
          <motion.p
            className="text-muted"
            variants={reduceMotion ? {} : fadeUp}
          >
            {description}
          </motion.p>
        </motion.div>
        {children}
      </div>
    </section>
  );
}
