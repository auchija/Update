import { classNames } from "../utils/classNames.js";
import styles from "./Card.module.css";

export default function Card({
  as: Tag = "div",
  elevated = false,
  compact = false,
  className,
  children,
  ...props
}) {
  return (
    <Tag
      className={classNames(
        styles.card,
        compact && styles.compact,
        elevated && styles.elevated,
        className,
      )}
      {...props}
    >
      {children}
    </Tag>
  );
}
