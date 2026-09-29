// Página de inicio de sesión ("/login"). Se entra con el correo o con el @usuario.
import { useState } from "react";
import { Link } from "react-router-dom";
import Alert from "../components/Alert.jsx";
import Button from "../components/Button.jsx";
import Card from "../components/Card.jsx";
import Footer from "../components/Footer.jsx";
import Header from "../components/Header.jsx";
import Input from "../components/Input.jsx";
import PasswordInput from "../components/PasswordInput.jsx";
import { SITE_NAME, USE_MOCKS } from "../config.js";
import { login, logout } from "../services/authService.js";
import styles from "./Auth.module.css";

// Revisa los datos y devuelve un objeto con los errores. Si está vacío, todo está bien.
function validate(values) {
  const errors = {};

  if (!values.identifier.trim()) {
    errors.identifier = "Escribe tu correo o tu nombre de usuario.";
  }
  if (!values.password) {
    errors.password = "Escribe tu contraseña.";
  }

  return errors;
}

export default function Login() {
  const [values, setValues] = useState({ identifier: "", password: "" });
  const [errors, setErrors] = useState({}); // errores de cada campo
  const [formError, setFormError] = useState(""); // error general (arriba del formulario)
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [user, setUser] = useState(null); // usuario que inició sesión

  // Se ejecuta cada vez que el usuario escribe en un campo.
  function handleChange(event) {
    const { name, value } = event.target;
    setValues({ ...values, [name]: value });
    // Si ese campo tenía un error, lo quitamos mientras lo corrige.
    if (errors[name]) setErrors({ ...errors, [name]: undefined });
  }

  async function handleSubmit(event) {
    event.preventDefault(); // evita que el navegador recargue la página
    setFormError("");

    const newErrors = validate(values);
    setErrors(newErrors);

    // Si hay errores, llevamos el cursor al primer campo con error y no enviamos.
    const firstError = Object.keys(newErrors)[0];
    if (firstError) {
      event.target.elements[firstError].focus();
      return;
    }

    setIsSubmitting(true);
    try {
      const loggedUser = await login(values.identifier.trim(), values.password);
      setUser(loggedUser);
    } catch (error) {
      setErrors(error.fieldErrors || {});
      setFormError(error.message);
      // Por seguridad, tras un intento fallido se vacía la contraseña.
      setValues({ ...values, password: "" });
    } finally {
      setIsSubmitting(false);
    }
  }

  async function handleLogout() {
    await logout();
    setUser(null);
    setValues({ identifier: "", password: "" });
  }

  return (
    <div className={styles.page}>
      <title>{`Iniciar sesión | ${SITE_NAME}`}</title>
      <Header />

      <main id="contenido" className={`container ${styles.main}`}>
        <Card className={styles.card}>
          {user ? (
            // ===== Sesión iniciada =====
            <>
              <Alert variant="success" title={`¡Hola, ${user.displayName}!`}>
                Iniciaste sesión como @{user.handle}.
              </Alert>
              <div className={styles.actions}>
                <Button to="/" fullWidth>
                  Ir al inicio
                </Button>
                <Button variant="ghost" fullWidth onClick={handleLogout}>
                  Cerrar sesión
                </Button>
              </div>
            </>
          ) : (
            // ===== Formulario =====
            <>
              <div className={styles.heading}>
                <h1 className={styles.title}>Iniciar sesión</h1>
                <p className="text-muted">Entra para seguir construyendo tu proyecto.</p>
              </div>

              {formError && <Alert variant="danger">{formError}</Alert>}

              {/* noValidate: la validación la hacemos nosotros con validate() */}
              <form onSubmit={handleSubmit} noValidate className={styles.form}>
                <Input
                  label="Correo o usuario"
                  name="identifier"
                  autoComplete="username"
                  autoCapitalize="none"
                  spellCheck={false}
                  required
                  value={values.identifier}
                  onChange={handleChange}
                  error={errors.identifier}
                />
                <PasswordInput
                  label="Contraseña"
                  name="password"
                  autoComplete="current-password"
                  required
                  value={values.password}
                  onChange={handleChange}
                  error={errors.password}
                />
                <Button type="submit" size="lg" fullWidth isLoading={isSubmitting}>
                  {isSubmitting ? "Iniciando sesión" : "Iniciar sesión"}
                </Button>
              </form>

              {USE_MOCKS && (
                <p className={styles.testAccount}>
                  Cuenta de prueba: <code>demo@correo.com</code> o <code>@valentina</code>, con
                  contraseña <code>Demo1234</code>
                </p>
              )}

              <p className={styles.switch}>
                ¿Aún no tienes cuenta? <Link to="/registro">Crea tu perfil</Link>
              </p>
            </>
          )}
        </Card>
      </main>

      <Footer />
    </div>
  );
}
