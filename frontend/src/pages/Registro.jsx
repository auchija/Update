import { useState } from "react";
import { Link } from "react-router-dom";
import Alert from "../components/Alert.jsx";
import Button from "../components/Button.jsx";
import Checkbox from "../components/Checkbox.jsx";
import CheckboxGroup from "../components/CheckboxGroup.jsx";
import Input from "../components/Input.jsx";
import PasswordInput from "../components/PasswordInput.jsx";
import { SITE_NAME } from "../config.js";
import { PERSONAS } from "../data/personas.js";
import { useForm } from "../hooks/useForm.js";
import { logout, register } from "../services/authService.js";
import { normalizeHandle, validateRegistration } from "../utils/authValidation.js";
import styles from "./auth/Auth.module.css";
import AuthHeading from "./auth/AuthHeading.jsx";
import AuthLayout from "./auth/AuthLayout.jsx";
import AuthSuccess from "./auth/AuthSuccess.jsx";

const PERSONA_OPTIONS = PERSONAS.map(({ value, label }) => ({ value, label }));

const EMPTY_REGISTRATION_FORM = {
  displayName: "",
  handle: "",
  email: "",
  password: "",
  personas: [],
  termsAccepted: false,
};

export default function Registro() {
  const [user, setUser] = useState(null);
  const form = useForm({
    initialValues: EMPTY_REGISTRATION_FORM,
    validate: validateRegistration,
    onSubmit: createAccount,
  });

  async function createAccount(values) {
    const newUser = await register({
      displayName: values.displayName.trim(),
      handle: values.handle,
      email: values.email.trim().toLowerCase(),
      password: values.password,
      personas: values.personas,
      termsAccepted: values.termsAccepted,
    });
    setUser(newUser);
  }

  function updateHandle(event) {
    form.setFieldValue("handle", normalizeHandle(event.target.value));
  }

  async function handleLogout() {
    await logout();
    setUser(null);
    form.reset();
  }

  return (
    <AuthLayout pageTitle="Crear cuenta">
      {user ? (
        <AuthSuccess title="Cuenta creada" onLogout={handleLogout}>
          Bienvenido a {SITE_NAME}, {user.displayName}. Tu usuario es @{user.handle}.
        </AuthSuccess>
      ) : (
        <>
          <AuthHeading
            title="Crea tu cuenta"
            subtitle="Muestra tu proyecto y conecta con quienes pueden impulsarlo."
          />

          {form.formError && <Alert variant="danger">{form.formError}</Alert>}

          <form onSubmit={form.handleSubmit} noValidate className={styles.form}>
            <Input
              label="Nombre completo"
              name="displayName"
              autoComplete="name"
              required
              value={form.values.displayName}
              onChange={form.handleChange}
              error={form.errors.displayName}
              validator={(v) => v.length >= 3 && v.length <= 60}
            />
            <Input
              label="Nombre de usuario"
              name="handle"
              autoComplete="username"
              autoCapitalize="none"
              spellCheck={false}
              required
              hint={`Será tu dirección: @${form.values.handle || "tuusuario"}. Minúsculas, números, punto y guion bajo.`}
              value={form.values.handle}
              onChange={updateHandle}
              error={form.errors.handle}
              validator={(v) => /^[a-z0-9._]{3,30}$/.test(v)}
            />
            <Input
              label="Correo electrónico"
              name="email"
              type="email"
              autoComplete="email"
              required
              value={form.values.email}
              onChange={form.handleChange}
              error={form.errors.email}
              validator={(v) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)}
            />
            <PasswordInput
              label="Contraseña"
              name="password"
              autoComplete="new-password"
              required
              hint="Mínimo 8 caracteres, con al menos una letra y un número."
              value={form.values.password}
              onChange={form.handleChange}
              error={form.errors.password}
              validator={(v) => v.length >= 8 && /[a-zA-Z]/.test(v) && /[0-9]/.test(v)}
            />
            <CheckboxGroup
              legend="¿Cómo participas en la comunidad?"
              hint="Puedes elegir varias opciones."
              name="personas"
              required
              options={PERSONA_OPTIONS}
              value={form.values.personas}
              onChange={(personas) => form.setFieldValue("personas", personas)}
              error={form.errors.personas}
            />
            <Checkbox
              label="Acepto los términos y la política de privacidad"
              name="termsAccepted"
              checked={form.values.termsAccepted}
              onChange={form.handleChange}
              error={form.errors.termsAccepted}
            />
            <Button type="submit" size="lg" fullWidth isLoading={form.isSubmitting}>
              {form.isSubmitting ? "Creando cuenta" : "Crear cuenta"}
            </Button>
          </form>

          <p className={styles.switch}>
            ¿Ya tienes cuenta? <Link to="/login">Inicia sesión</Link>
          </p>
        </>
      )}
    </AuthLayout>
  );
}
