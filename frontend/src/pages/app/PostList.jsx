import { useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Card from "../../components/Card.jsx";
import Spinner from "../../components/Spinner.jsx";
import { eliminarPublicacion } from "../../services/feedService.js";
import PostCard from "./PostCard.jsx";
import PostForm from "./PostForm.jsx";
import styles from "./FeedPage.module.css";

/**
 * Lista de publicaciones con carga, error, vacío, edición en línea, eliminación y "Ver más".
 * La usan el feed y "Mis publicaciones". `list` viene de usePaginatedList.
 */
export default function PostList({ list, label, emptyState, onNotice }) {
  const [editingId, setEditingId] = useState(null);
  const [deletingId, setDeletingId] = useState(null);

  function startEditing(post) {
    onNotice(null);
    setEditingId(post.id);
  }

  function handleUpdated(post) {
    list.replaceItem(post);
    setEditingId(null);
    onNotice({ variant: "success", text: "Guardamos los cambios." });
  }

  async function handleDelete(post) {
    if (!window.confirm("¿Quieres eliminar esta publicación?")) return;

    setDeletingId(post.id);
    onNotice(null);
    try {
      await eliminarPublicacion(post.id);
      list.removeItem(post.id);
      if (editingId === post.id) setEditingId(null);
      onNotice({ variant: "success", text: "Eliminamos la publicación." });
    } catch (error) {
      onNotice({ variant: "danger", text: error.message });
    } finally {
      setDeletingId(null);
    }
  }

  async function handleLoadMore() {
    onNotice(null);
    try {
      await list.loadMore();
    } catch (error) {
      onNotice({ variant: "danger", text: error.message });
    }
  }

  return (
    <section aria-label={label} className={styles.list}>
      {list.loadError && (
        <Card className="stack">
          <Alert variant="danger">{list.loadError}</Alert>
          <div>
            <Button variant="secondary" onClick={list.retry}>
              Reintentar
            </Button>
          </div>
        </Card>
      )}

      {!list.loadError && !list.items && <Spinner label="Cargando publicaciones" />}

      {list.items?.length === 0 && <Card className={styles.empty}>{emptyState}</Card>}

      {list.items?.map((post) => (
        <Card key={post.id}>
          {editingId === post.id ? (
            <PostForm post={post} onSaved={handleUpdated} onCancel={() => setEditingId(null)} />
          ) : (
            <PostCard
              post={post}
              onEdit={() => startEditing(post)}
              onDelete={() => handleDelete(post)}
              isDeleting={deletingId === post.id}
            />
          )}
        </Card>
      ))}

      {list.hasMore && (
        <div className={styles.more}>
          <Button variant="secondary" onClick={handleLoadMore} isLoading={list.isLoadingMore}>
            {list.isLoadingMore ? "Cargando" : "Ver más"}
          </Button>
        </div>
      )}
    </section>
  );
}
