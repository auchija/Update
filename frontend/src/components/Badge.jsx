import { classNames } from "../utils/classNames.js";
import styles from "./Badge.module.css";

export default function Badge({ variant = "neutral", children }) {
  return <span className={classNames(styles.badge, styles[variant])}>{children}</span>;
}
