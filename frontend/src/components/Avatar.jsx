import { classNames } from "../utils/classNames.js";
import styles from "./Avatar.module.css";

export default function Avatar({ name, src, size = "md", decorative = false }) {
  const className = classNames(styles.avatar, styles[size]);

  if (src) {
    return <img src={src} alt={decorative ? "" : name} className={className} />;
  }

  const accessibilityProps = decorative
    ? { "aria-hidden": true }
    : { role: "img", "aria-label": name };

  return (
    <span className={className} {...accessibilityProps}>
      {getInitials(name)}
    </span>
  );
}

function getInitials(name) {
  return name
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((word) => word[0].toUpperCase())
    .join("");
}
