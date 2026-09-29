// Página de registro ("/registro"). Los campos siguen el diseño de la base de datos:
// profiles (displayName, handle), users (email, contraseña, términos) y user_personas.
import { useState } from "react";
import { Link } from "react-router-dom";
import Alert from "../components/Alert.jsx";
import Button from "../components/Button.jsx";
import Card from "../components/Card.jsx";
import Checkbox from "../components/Checkbox.jsx";
import CheckboxGroup from "../components/CheckboxGroup.jsx";
import Footer from "../components/Footer.jsx";
import Header from "../components/Header.jsx";
import Input from "../components/Input.jsx";
import PasswordInput from "../components/PasswordInput.jsx";
import { SITE_NAME } from "../config.js";
import { PERSONAS } from "../data/personas.js";
import { logout, register } from "../services/authService.js";
import styles from "./Auth.module.css";

// "Personas": cómo participa cada usuario en la comunidad (puede elegir varias).
// Salen de data/personas.js, la misma lista que muestra la landing.
const PERSONA_OPTIONS = PERSONAS.map((persona) => ({
  value: persona.value,
  label: persona.label,
}));

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
// @usuario: solo minúsculas, números, punto y guion bajo (regla de la base de datos).
const HANDLE_PATTERN = /^[a-z0-9._]{3,30}$/;

// Limpia el @usuario mientras se escribe: "José Pérez" → "joseperez".
// Pasa a minúsculas, quita las tildes y borra todo lo que no sea letra, número, punto o guion bajo.
function cleanHandle(text) {
  return text
    .toLowerCase()
    .normalize("NFD")
    .replace(/[̀-ͯ]/g, "")
    .replace(/[^a-z0-9._]/g, "");
}

// Revisa los datos y devuelve un objeto con los errores. Si está vacío, todo está bien.
function validate(values) {
  const errors = {};

  const name = values.displayName.trim();
  if (name.length < 3 || name.length > 60) {
    errors.displayName = "Escribe tu nombre completo (entre 3 y 60 caracteres).";
  }

  if (!HANDLE_PATTERN.test(values.handle)) {
    errors.handle = "Usa de 3 a 30 caracteres: minúsculas, números, punto (.) o guion bajo (_).";
  }

  if (!EMAIL_PATTERN.test(values.email.trim())) {
    errors.email = "Escribe un correo válido, por ejemplo nombre@dominio.com.";
  }

  if (values.password.length < 8) {
    errors.password = "Usa al menos 8 caracteres.";
  } else if (!/[a-zA-Z]/.test(values.password) || !/[0-9]/.test(values.password)) {
    errors.password = "Incluye al menos una letra y un número.";
  }

  if (values.personas.length === 0) {
    errors.personas = "Elige al menos una opción.";
  }

  if (!values.termsAccepted) {
    errors.termsAccepted = "Debes aceptar los términos para crear tu cuenta.";
  }

  return errors;
}

const EMPTY_FORM = {
  displayName: "",
  handle: "",
  email: "",
  password: "",
  personas: [],
  termsAccepted: false,
};

export default function Registro() {
  const [values, setValues] = useState(EMPTY_FORM);
  const [errors, setErrors] = useState({}); // errores de cada campo
  const [formError, setFormError] = useState(""); // error general (arriba del formulario)
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [user, setUser] = useState(null); // usuario recién creado

  // Guarda el nuevo valor de un campo y quita su error mientras el usuario lo corrige.
  function updateField(name, value) {
    setValues({ ...values, [name]: value });
    if (errors[name]) setErrors({ ...errors, [name]: undefined });
  }

  function handleChange(event) {
    const { name, type, value, checked } = event.target;
    if (type === "checkbox") updateField(name, checked);
    else if (name === "handle") updateField(name, cleanHandle(value));
    else updateField(name, value);
  }

  async function handleSubmit(event) {
    event.preventDefault(); // evita que el navegador recargue la página
    setFormError("");

    const newErrors = validate(values);
    setErrors(newErrors);

    // Si hay errores, llevamos el cursor al primer campo con error y no enviamos.
    const firstError = Object.keys(newErrors)[0];
    if (firstError) {
      const field = event.target.elements[firstError];
      // En el grupo de casillas hay varios campos con el mismo nombre: enfocamos el primero.
      (field.focus ? field : field[0]).focus();
      return;
    }

    setIsSubmitting(true);
    try {
      const newUser = await register({
        displayName: values.displayName.trim(),
        handle: values.handle,
        email: values.email.trim().toLowerCase(),
        password: values.password,
        personas: values.personas,
        termsAccepted: values.termsAccepted,
      });
      setUser(newUser);
    } catch (error) {
      setErrors(error.fieldErrors || {});
      setFormError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  async function handleLogout() {
    await logout();
    setUser(null);
    setValues(EMPTY_FORM);
  }

  return (
    <div className={styles.page}>
      <title>{`Crear cuenta | ${SITE_NAME}`}</title>
      <Header />

      <main id="contenido" className={`container ${styles.main}`}>
        <Card className={styles.card}>
          {user ? (
            // ===== Cuenta creada =====
            <>
              <Alert variant="success" title="Cuenta creada">
                Bienvenido a {SITE_NAME}, {user.displayName}. Tu usuario es @{user.handle}.
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
                <h1 className={styles.title}>Crea tu cuenta</h1>
                <p className="text-muted">
                  Muestra tu proyecto y conecta con quienes pueden impulsarlo.
                </p>
              </div>

              {formError && <Alert variant="danger">{formError}</Alert>}

              {/* noValidate: la validación la hacemos nosotros con validate() */}
              <form onSubmit={handleSubmit} noValidate className={styles.form}>
                <Input
                  label="Nombre completo"
                  name="displayName"
                  autoComplete="name"
                  required
                  value={values.displayName}
                  onChange={handleChange}
                  error={errors.displayName}
                />
                <Input
                  label="Nombre de usuario"
                  name="handle"
                  autoComplete="username"
                  autoCapitalize="none"
                  spellCheck={false}
                  required
                  hint={`Será tu dirección: @${values.handle || "tuusuario"}. Minúsculas, números, punto y guion bajo.`}
                  value={values.handle}
                  onChange={handleChange}
                  error={errors.handle}
                />
                <Input
                  label="Correo electrónico"
                  name="email"
                  type="email"
                  autoComplete="email"
                  required
                  value={values.email}
                  onChange={handleChange}
                  error={errors.email}
                />
                <PasswordInput
                  label="Contraseña"
                  name="password"
                  autoComplete="new-password"
                  required
                  hint="Mínimo 8 caracteres, con al menos una letra y un número."
                  value={values.password}
                  onChange={handleChange}
                  error={errors.password}
                />
                <CheckboxGroup
                  legend="¿Cómo participas en la comunidad?"
                  hint="Puedes elegir varias opciones."
                  name="personas"
                  required
                  options={PERSONA_OPTIONS}
                  value={values.personas}
                  onChange={(personas) => updateField("personas", personas)}
                  error={errors.personas}
                />
                <Checkbox
                  label="Acepto los términos y la política de privacidad"
                  name="termsAccepted"
                  checked={values.termsAccepted}
                  onChange={handleChange}
                  error={errors.termsAccepted}
                />
                <Button type="submit" size="lg" fullWidth isLoading={isSubmitting}>
                  {isSubmitting ? "Creando cuenta" : "Crear cuenta"}
                </Button>
              </form>

              <p className={styles.switch}>
                ¿Ya tienes cuenta? <Link to="/login">Inicia sesión</Link>
              </p>
            </>
          )}
        </Card>
      </main>

      <Footer />
    </div>
  );
}
