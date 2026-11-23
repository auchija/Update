import { useRef, useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import FileUpload from "../../components/FileUpload.jsx";
import Textarea from "../../components/Textarea.jsx";
import { useForm } from "../../hooks/useForm.js";
import {
  actualizarPublicacion,
  crearPublicacion,
  subirArchivoPublicacion,
} from "../../services/feedService.js";
import { POST_MAX_LENGTH, validatePost } from "../../utils/postValidation.js";
import PostAttachment from "./PostAttachment.jsx";
import styles from "./FeedPage.module.css";

/**
 * Crea una publicación o, si recibe `post`, la edita.
 * Persistencia dual: primero se sube el archivo (Storage + metadatos en BD)
 * y luego se guarda la publicación con el id de ese archivo.
 */
export default function PostForm({ post, onSaved, onCancel }) {
  const isEditing = Boolean(post);
  const [file, setFile] = useState(null);
  const [keepCurrentFile, setKeepCurrentFile] = useState(Boolean(post?.imagenArchivoId));
  const uploadedFileId = useRef(null);

  const form = useForm({
    initialValues: { contenido: post?.contenido ?? "" },
    validate: validatePost,
    onSubmit: save,
  });

  function handleFileChange(selected) {
    setFile(selected);
    uploadedFileId.current = null;
  }

  async function save(values) {
    let imagenArchivoId = keepCurrentFile ? post.imagenArchivoId : null;
    if (file) {
      // Si falla el segundo paso, al reintentar no se vuelve a subir el mismo archivo.
      uploadedFileId.current ??= (await subirArchivoPublicacion(file)).id;
      imagenArchivoId = uploadedFileId.current;
    }

    const datos = { contenido: values.contenido.trim(), imagenArchivoId };
    const saved = isEditing
      ? await actualizarPublicacion(post.id, datos)
      : await crearPublicacion(datos);

    if (!isEditing) {
      form.reset();
      handleFileChange(null);
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

      {keepCurrentFile ? (
        <div className={styles.currentFile}>
          <PostAttachment
            url={post.imagenUrl}
            mimeType={post.adjuntoTipoMime}
            alt="Archivo actual de la publicación"
          />
          <Button variant="ghost" size="sm" onClick={() => setKeepCurrentFile(false)}>
            Quitar o cambiar archivo
          </Button>
        </div>
      ) : (
        <FileUpload
          label="Archivo adjunto (opcional)"
          file={file}
          onChange={handleFileChange}
          disabled={form.isSubmitting}
        />
      )}

      <div className={styles.formActions}>
        {isEditing && (
          <Button variant="ghost" onClick={onCancel} disabled={form.isSubmitting}>
            Cancelar
          </Button>
        )}
        <Button type="submit" isLoading={form.isSubmitting}>
          {form.isSubmitting ? "Guardando" : isEditing ? "Guardar cambios" : "Publicar"}
        </Button>
      </div>
    </form>
  );
}
