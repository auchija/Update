import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import styles from "./Auth.module.css";

export default function AuthSuccess({ title, children, onLogout }) {
  return (
    <>
      <Alert variant="success" title={title}>
        {children}
      </Alert>
      <div className={styles.actions}>
        <Button to="/" fullWidth>
          Ir al inicio
        </Button>
        <Button variant="ghost" fullWidth onClick={onLogout}>
          Cerrar sesión
        </Button>
      </div>
    </>
  );
}
