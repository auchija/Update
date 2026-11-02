import { useEffect, useMemo, useRef, useState } from "react";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import Input from "../../components/Input.jsx";
import Textarea from "../../components/Textarea.jsx";
import { useForm } from "../../hooks/useForm.js";
import { actualizarMiPerfil, subirFotoPerfil } from "../../services/perfilService.js";
import {
  BIO_MAX_LENGTH,
  PHOTO_ACCEPT,
  validatePhoto,
  validateProfile,
} from "../../utils/profileValidation.js";
import styles from "./ProfilePage.module.css";

export default function ProfileForm({ perfil, onCancel, onSaved }) {
  const [photo, setPhoto] = useState(null);
  const [photoError, setPhotoError] = useState("");
  const uploadedPhotoId = useRef(null);
  const photoInput = useRef(null);

  const form = useForm({
    initialValues: {
      nombreVisible: perfil.nombreVisible,
      nombreUsuario: perfil.nombreUsuario,
      titular: perfil.titular ?? "",
      biografia: perfil.biografia ?? "",
      sitioWeb: perfil.sitioWeb ?? "",
    },
    validate: validateProfile,
    onSubmit: save,
  });

  const previewUrl = useMemo(() => (photo ? URL.createObjectURL(photo) : null), [photo]);

  useEffect(() => {
    if (!previewUrl) return undefined;
    return () => URL.revokeObjectURL(previewUrl);
  }, [previewUrl]);

  function handlePhotoChange(event) {
    const file = event.target.files[0];
    if (!file) return;

    const error = validatePhoto(file);
    setPhotoError(error);
    uploadedPhotoId.current = null;
    setPhoto(error ? null : file);
    if (error) event.target.value = "";
  }

  async function save(values) {
    let fotoArchivoId = perfil.fotoArchivoId;
    if (photo) {
      uploadedPhotoId.current ??= (await subirFotoPerfil(photo)).id;
      fotoArchivoId = uploadedPhotoId.current;
    }

    const saved = await actualizarMiPerfil({
      nombreVisible: values.nombreVisible.trim(),
      nombreUsuario: values.nombreUsuario,
      titular: values.titular.trim() || null,
      biografia: values.biografia.trim() || null,
      sitioWeb: values.sitioWeb.trim() || null,
      fotoArchivoId,
    });
    onSaved(saved);
  }

  return (
    <form onSubmit={form.handleSubmit} noValidate className="stack">
      <h1>Editar perfil</h1>

      {form.formError && <Alert variant="danger">{form.formError}</Alert>}

      <div className="cluster">
        <Avatar
          name={form.values.nombreVisible || perfil.nombreVisible}
          src={previewUrl ?? perfil.fotoUrl}
          size="lg"
        />
        <div className="stack stack--xs">
          <input
            ref={photoInput}
            type="file"
            accept={PHOTO_ACCEPT}
            onChange={handlePhotoChange}
            className="sr-only"
            aria-label="Elegir foto de perfil"
            tabIndex={-1}
          />
          <div>
            <Button variant="secondary" onClick={() => photoInput.current.click()}>
              Cambiar foto
            </Button>
          </div>
          <p className="text-muted text-sm">JPG, PNG o WEBP. Máximo 5 MB.</p>
          {photoError && (
            <p role="alert" className={styles.photoError}>
              {photoError}
            </p>
          )}
        </div>
      </div>

      <Input
        label="Nombre"
        name="nombreVisible"
        autoComplete="name"
        required
        value={form.values.nombreVisible}
        onChange={form.handleChange}
        error={form.errors.nombreVisible}
      />
      <Input
        label="Usuario"
        name="nombreUsuario"
        autoCapitalize="none"
        spellCheck={false}
        required
        value={form.values.nombreUsuario}
        onChange={form.handleChange}
        error={form.errors.nombreUsuario}
      />
      <Input
        label="Titular"
        name="titular"
        hint="Lo que haces, en una línea."
        value={form.values.titular}
        onChange={form.handleChange}
        error={form.errors.titular}
      />
      <Textarea
        label="Biografía"
        name="biografia"
        rows={5}
        hint={`${form.values.biografia.length} de ${BIO_MAX_LENGTH} caracteres`}
        value={form.values.biografia}
        onChange={form.handleChange}
        error={form.errors.biografia}
      />
      <Input
        label="Sitio web"
        name="sitioWeb"
        type="url"
        autoComplete="url"
        placeholder="https://"
        value={form.values.sitioWeb}
        onChange={form.handleChange}
        error={form.errors.sitioWeb}
      />

      <div className="cluster">
        <Button type="submit" isLoading={form.isSubmitting}>
          {form.isSubmitting ? "Guardando" : "Guardar cambios"}
        </Button>
        <Button variant="ghost" onClick={onCancel} disabled={form.isSubmitting}>
          Cancelar
        </Button>
      </div>
    </form>
  );
}
