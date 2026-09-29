import styles from "./Auth.module.css";

export default function AuthHeading({ title, subtitle }) {
  return (
    <div className={styles.heading}>
      <h1 className={styles.title}>{title}</h1>
      <p className="text-muted">{subtitle}</p>
    </div>
  );
}
