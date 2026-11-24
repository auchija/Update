import { useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Modal from "../../components/Modal.jsx";
import Spinner from "../../components/Spinner.jsx";
import { eliminarPublicacion } from "../../services/feedService.js";
import PostCard from "./PostCard.jsx";
import PostForm from "./PostForm.jsx";
import styles from "./PostList.module.css";

export default function PostList({ list, label, emptyState, onNotice }) {
  const [editingPost, setEditingPost] = useState(null);
  const [deletingId, setDeletingId] = useState(null);

  function startEditing(post) {
    onNotice(null);
    setEditingPost(post);
  }

  function handleUpdated(post) {
    list.replaceItem(post);
    setEditingPost(null);
    onNotice({ variant: "success", text: "Guardamos los cambios." });
  }

  async function handleDelete(post) {
    if (!window.confirm("¿Quieres eliminar esta publicación?")) return;

    setDeletingId(post.id);
    onNotice(null);
    try {
      await eliminarPublicacion(post.id);
      list.removeItem(post.id);
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
        <div className={styles.state}>
          <Alert variant="danger">{list.loadError}</Alert>
          <Button variant="secondary" onClick={list.retry}>
            Reintentar
          </Button>
        </div>
      )}

      {!list.loadError && !list.items && (
        <div className={styles.state}>
          <Spinner label="Cargando publicaciones" />
        </div>
      )}

      {list.items?.length === 0 && <div className={styles.empty}>{emptyState}</div>}

      {list.items?.map((post) => (
        <PostCard
          key={post.id}
          post={post}
          onChange={list.replaceItem}
          onEdit={() => startEditing(post)}
          onDelete={() => handleDelete(post)}
          isDeleting={deletingId === post.id}
        />
      ))}

      {list.hasMore && (
        <div className={styles.more}>
          <Button variant="secondary" onClick={handleLoadMore} isLoading={list.isLoadingMore}>
            {list.isLoadingMore ? "Cargando" : "Ver más"}
          </Button>
        </div>
      )}

      <Modal
        open={Boolean(editingPost)}
        onClose={() => setEditingPost(null)}
        title="Editar publicación"
      >
        {editingPost && (
          <PostForm
            post={editingPost}
            onSaved={handleUpdated}
            onCancel={() => setEditingPost(null)}
          />
        )}
      </Modal>
    </section>
  );
}
