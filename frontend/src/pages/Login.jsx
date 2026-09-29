import { useState } from "react";
import { Link } from "react-router-dom";
import Alert from "../components/Alert.jsx";
import Button from "../components/Button.jsx";
import Input from "../components/Input.jsx";
import PasswordInput from "../components/PasswordInput.jsx";
import { USE_MOCKS } from "../config.js";
import { useForm } from "../hooks/useForm.js";
import { login, logout } from "../services/authService.js";
import { validateLogin } from "../utils/authValidation.js";
import styles from "./auth/Auth.module.css";
import AuthHeading from "./auth/AuthHeading.jsx";
import AuthLayout from "./auth/AuthLayout.jsx";
import AuthSuccess from "./auth/AuthSuccess.jsx";

const EMPTY_LOGIN_FORM = { identifier: "", password: "" };

export default function Login() {
  const [user, setUser] = useState(null);
  const form = useForm({
    initialValues: EMPTY_LOGIN_FORM,
    validate: validateLogin,
    onSubmit: signIn,
  });

  async function signIn({ identifier, password }) {
    try {
      setUser(await login(identifier.trim(), password));
    } catch (error) {
      form.setFieldValue("password", "");
      throw error;
    }
  }

  async function handleLogout() {
    await logout();
    setUser(null);
    form.reset();
  }

  return (
    <AuthLayout pageTitle="Iniciar sesión">
      {user ? (
        <AuthSuccess title={`¡Hola, ${user.displayName}!`} onLogout={handleLogout}>
          Iniciaste sesión como @{user.handle}.
        </AuthSuccess>
      ) : (
        <>
          <AuthHeading
            title="Iniciar sesión"
            subtitle="Entra para seguir construyendo tu proyecto."
          />

          {form.formError && <Alert variant="danger">{form.formError}</Alert>}

          <form onSubmit={form.handleSubmit} noValidate className={styles.form}>
            <Input
              label="Correo o usuario"
              name="identifier"
              autoComplete="username"
              autoCapitalize="none"
              spellCheck={false}
              required
              value={form.values.identifier}
              onChange={form.handleChange}
              error={form.errors.identifier}
            />
            <PasswordInput
              label="Contraseña"
              name="password"
              autoComplete="current-password"
              required
              value={form.values.password}
              onChange={form.handleChange}
              error={form.errors.password}
            />
            <Button type="submit" size="lg" fullWidth isLoading={form.isSubmitting}>
              {form.isSubmitting ? "Iniciando sesión" : "Iniciar sesión"}
            </Button>
          </form>

          {USE_MOCKS && <TestAccountHint />}

          <p className={styles.switch}>
            ¿Aún no tienes cuenta? <Link to="/registro">Crea tu perfil</Link>
          </p>
        </>
      )}
    </AuthLayout>
  );
}

function TestAccountHint() {
  return (
    <p className={styles.testAccount}>
      Cuenta de prueba: <code>demo@correo.com</code> o <code>@valentina</code>, con contraseña{" "}
      <code>Demo1234</code>
    </p>
  );
}
