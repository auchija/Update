import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ArrowLeft } from "lucide-react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Modal from "../../components/Modal.jsx";
import Spinner from "../../components/Spinner.jsx";
import { ROUTES, userProfilePath } from "../../constants/routes.js";
import { eliminarPublicacion, getPublicacion } from "../../services/feedService.js";
import PostCard from "./PostCard.jsx";
import PostForm from "./PostForm.jsx";
import styles from "./PostPage.module.css";

/** Una publicación sola (/p/:id), con los comentarios abiertos. Es el enlace que se comparte. */
export default function PostPage() {
  const { id } = useParams();
  return <PostView key={id} id={id} />;
}

function PostView({ id }) {
  const navigate = useNavigate();
  const [post, setPost] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [isEditing, setIsEditing] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [notice, setNotice] = useState(null);

  useEffect(() => {
    let cancelled = false;
    getPublicacion(id)
      .then((data) => {
        if (!cancelled) setPost(data);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });
    return () => {
      cancelled = true;
    };
  }, [id]);

  async function handleDelete() {
    if (!window.confirm("¿Quieres eliminar esta publicación?")) return;
    setIsDeleting(true);
    try {
      await eliminarPublicacion(post.id);
      navigate(userProfilePath(post.autor.nombreUsuario), { replace: true });
    } catch (error) {
      setNotice({ variant: "danger", text: error.message });
      setIsDeleting(false);
    }
  }

  return (
    <>
      <title>
        {post ? `Publicación de ${post.autor.nombreVisible} · UpDate` : "Publicación · UpDate"}
      </title>
      <div className={styles.page}>
        <button type="button" onClick={() => navigate(-1)} className={styles.back}>
          <ArrowLeft aria-hidden="true" className="icon icon--sm" />
          Volver
        </button>

        {loadError && (
          <div className={styles.state}>
            <h1>Esta publicación no está disponible</h1>
            <p className="text-muted">{loadError}</p>
            <Button to={ROUTES.feed} variant="secondary">
              Ir al inicio
            </Button>
          </div>
        )}
        {!loadError && !post && (
          <div className={styles.state}>
            <Spinner label="Cargando publicación" />
          </div>
        )}

        {notice && <Alert variant={notice.variant}>{notice.text}</Alert>}

        {post && (
          <PostCard
            post={post}
            onChange={setPost}
            onEdit={() => setIsEditing(true)}
            onDelete={handleDelete}
            isDeleting={isDeleting}
            commentsOpen
          />
        )}

        <Modal open={isEditing} onClose={() => setIsEditing(false)} title="Editar publicación">
          {post && (
            <PostForm
              post={post}
              onSaved={(saved) => {
                setPost(saved);
                setIsEditing(false);
                setNotice({ variant: "success", text: "Guardamos los cambios." });
              }}
              onCancel={() => setIsEditing(false)}
            />
          )}
        </Modal>
      </div>
    </>
  );
}
