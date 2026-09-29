import { CircleAlert, CircleCheck, Info } from "lucide-react";
import { classNames } from "../utils/classNames.js";
import styles from "./Alert.module.css";

const ICONS = { info: Info, success: CircleCheck, danger: CircleAlert };

export default function Alert({ variant = "info", title, children }) {
  const Icon = ICONS[variant];
  const role = variant === "danger" ? "alert" : "status";

  return (
    <div role={role} className={classNames(styles.alert, styles[variant])}>
      <Icon aria-hidden="true" className={classNames("icon", styles.icon)} />
      <div className={styles.body}>
        {title && <p className={styles.title}>{title}</p>}
        {children && <div>{children}</div>}
      </div>
    </div>
  );
}
