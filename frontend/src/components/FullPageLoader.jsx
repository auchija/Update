import Spinner from "./Spinner.jsx";
import styles from "./FullPageLoader.module.css";

export default function FullPageLoader({ label = "Cargando" }) {
  return (
    <div className={styles.wrapper}>
      <Spinner size="lg" label={label} />
    </div>
  );
}
