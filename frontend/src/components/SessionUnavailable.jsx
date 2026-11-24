import Alert from "./Alert.jsx";
import Button from "./Button.jsx";
import { useAuth } from "../hooks/useAuth.js";
import styles from "./FullPageLoader.module.css";

export default function SessionUnavailable() {
  const { retrySession } = useAuth();

  return (
    <div className={styles.wrapper}>
      <div className="stack">
        <Alert variant="danger">
          No pudimos conectar con el servidor. Tu sesión sigue guardada.
        </Alert>
        <Button onClick={retrySession}>Reintentar</Button>
      </div>
    </div>
  );
}
