import { LoaderCircle } from "lucide-react";
import { classNames } from "../utils/classNames.js";
import styles from "./Spinner.module.css";

export default function Spinner({ size = "md", label }) {
  const icon = (
    <LoaderCircle aria-hidden="true" className={classNames(styles.spinner, styles[size])} />
  );

  if (!label) return icon;

  return (
    <span role="status">
      {icon}
      <span className="sr-only">{label}</span>
    </span>
  );
}
