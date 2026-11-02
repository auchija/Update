import { useEffect, useMemo, useRef, useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Textarea from "../../components/Textarea.jsx";
import { useForm } from "../../hooks/useForm.js";
import {
  actualizarPublicacion,
  crearPublicacion,
  subirImagenPublicacion,
} from "../../services/feedService.js";
import { PHOTO_ACCEPT, validatePhoto } from "../../utils/profileValidation.js";
import { POST_MAX_LENGTH, validatePost } from "../../utils/postValidation.js";
import styles from "./FeedPage.module.css";

/**
 * Crea una publicación o, si recibe `post`, la edita.
 * La imagen se sube primero y luego se envía su id con el texto, igual que la foto de perfil.
 */
export default function PostForm({ post, onSaved, onCancel }) {
  const isEditing = Boolean(post);
  const [image, setImage] = useState(null);
  const [keepCurrentImage, setKeepCurrentImage] = useState(Boolean(post?.imagenUrl));
  const [imageError, setImageError] = useState("");
  const uploadedImageId = useRef(null);
  const imageInput = useRef(null);

  const form = useForm({
    initialValues: { contenido: post?.contenido ?? "" },
    validate: validatePost,
    onSubmit: save,
  });

  const previewUrl = useMemo(() => (image ? URL.createObjectURL(image) : null), [image]);

  useEffect(() => {
    if (!previewUrl) return undefined;
    return () => URL.revokeObjectURL(previewUrl);
  }, [previewUrl]);

  const shownImage = previewUrl ?? (keepCurrentImage ? post.imagenUrl : null);

  function handleImageChange(event) {
    const file = event.target.files[0];
    if (!file) return;

    const error = validatePhoto(file);
    setImageError(error);
    uploadedImageId.current = null;
    setImage(error ? null : file);
    event.target.value = "";
  }

  function removeImage() {
    setImage(null);
    setKeepCurrentImage(false);
    setImageError("");
    uploadedImageId.current = null;
  }

  async function save(values) {
    let imagenArchivoId = keepCurrentImage ? post.imagenArchivoId : null;
    if (image) {
      // Si falla el segundo paso, al reintentar no se vuelve a subir la misma imagen.
      uploadedImageId.current ??= (await subirImagenPublicacion(image)).id;
      imagenArchivoId = uploadedImageId.current;
    }

    const datos = { contenido: values.contenido.trim(), imagenArchivoId };
    const saved = isEditing
      ? await actualizarPublicacion(post.id, datos)
      : await crearPublicacion(datos);

    if (!isEditing) {
      form.reset();
      removeImage();
    }
    onSaved(saved);
  }

  return (
    <form onSubmit={form.handleSubmit} noValidate className="stack">
      <h2 className={styles.formTitle}>{isEditing ? "Editar publicación" : "Crear publicación"}</h2>

      {form.formError && <Alert variant="danger">{form.formError}</Alert>}

      <Textarea
        label="¿Qué quieres compartir?"
        name="contenido"
        rows={isEditing ? 4 : 5}
        placeholder="Cuenta en qué estás trabajando, qué lograste o qué necesitas."
        required
        maxLength={POST_MAX_LENGTH}
        hint={`${form.values.contenido.length} de ${POST_MAX_LENGTH} caracteres`}
        value={form.values.contenido}
        onChange={form.handleChange}
        error={form.errors.contenido}
      />

      {shownImage && (
        <img src={shownImage} alt="Imagen que se publicará" className={styles.image} />
      )}

      <input
        ref={imageInput}
        type="file"
        accept={PHOTO_ACCEPT}
        onChange={handleImageChange}
        className="sr-only"
        aria-label="Elegir imagen para la publicación"
        tabIndex={-1}
      />
      {imageError && (
        <p role="alert" className={styles.imageError}>
          {imageError}
        </p>
      )}

      <div className={styles.formActions}>
        <div className="cluster cluster--xs">
          <Button variant="secondary" size="sm" onClick={() => imageInput.current.click()}>
            {shownImage ? "Cambiar imagen" : "Agregar imagen"}
          </Button>
          {shownImage && (
            <Button variant="ghost" size="sm" onClick={removeImage}>
              Quitar imagen
            </Button>
          )}
        </div>
        <div className="cluster cluster--xs">
          {isEditing && (
            <Button variant="ghost" onClick={onCancel} disabled={form.isSubmitting}>
              Cancelar
            </Button>
          )}
          <Button type="submit" isLoading={form.isSubmitting}>
            {form.isSubmitting ? "Guardando" : isEditing ? "Guardar cambios" : "Publicar"}
          </Button>
        </div>
      </div>
      <p className="text-muted text-sm">Imagen opcional: JPG, PNG o WEBP de máximo 5 MB.</p>
    </form>
  );
}
