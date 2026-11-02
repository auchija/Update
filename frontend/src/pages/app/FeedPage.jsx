import { useEffect, useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Card from "../../components/Card.jsx";
import Spinner from "../../components/Spinner.jsx";
import { eliminarPublicacion, FEED_PAGE_SIZE, getFeed } from "../../services/feedService.js";
import PostCard from "./PostCard.jsx";
import PostForm from "./PostForm.jsx";
import styles from "./FeedPage.module.css";

export default function FeedPage() {
  const [posts, setPosts] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [attempt, setAttempt] = useState(0);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [deletingId, setDeletingId] = useState(null);
  const [notice, setNotice] = useState(null);

  useEffect(() => {
    let cancelled = false;

    getFeed(1)
      .then((data) => {
        if (cancelled) return;
        setPosts(data);
        setPage(1);
        setHasMore(data.length === FEED_PAGE_SIZE);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });

    return () => {
      cancelled = true;
    };
  }, [attempt]);

  function retry() {
    setLoadError("");
    setAttempt((current) => current + 1);
  }

  async function loadMore() {
    setIsLoadingMore(true);
    setNotice(null);
    try {
      const data = await getFeed(page + 1);
      // Si alguien publicó mientras tanto, la página siguiente puede repetir publicaciones.
      setPosts((current) => [
        ...current,
        ...data.filter((p) => !current.some((c) => c.id === p.id)),
      ]);
      setPage((current) => current + 1);
      setHasMore(data.length === FEED_PAGE_SIZE);
    } catch (error) {
      setNotice({ variant: "danger", text: error.message });
    } finally {
      setIsLoadingMore(false);
    }
  }

  function handleCreated(post) {
    setPosts((current) => [post, ...current]);
    setNotice({ variant: "success", text: "Publicamos tu publicación." });
  }

  function handleUpdated(post) {
    setPosts((current) => current.map((p) => (p.id === post.id ? post : p)));
    setEditingId(null);
    setNotice({ variant: "success", text: "Guardamos los cambios." });
  }

  async function handleDelete(post) {
    if (!window.confirm("¿Quieres eliminar esta publicación?")) return;

    setDeletingId(post.id);
    setNotice(null);
    try {
      await eliminarPublicacion(post.id);
      setPosts((current) => current.filter((p) => p.id !== post.id));
      if (editingId === post.id) setEditingId(null);
      setNotice({ variant: "success", text: "Eliminamos la publicación." });
    } catch (error) {
      setNotice({ variant: "danger", text: error.message });
    } finally {
      setDeletingId(null);
    }
  }

  function startEditing(post) {
    setNotice(null);
    setEditingId(post.id);
  }

  return (
    <>
      <title>Inicio</title>
      <div className={styles.feed}>
        <header>
          <h1>Publicaciones</h1>
          <p className="text-muted">Comparte tus ideas, proyectos y avances.</p>
        </header>

        <Card>
          <PostForm onSaved={handleCreated} />
        </Card>

        {notice && <Alert variant={notice.variant}>{notice.text}</Alert>}

        <section aria-label="Publicaciones recientes" className={styles.list}>
          {loadError && (
            <Card className="stack">
              <Alert variant="danger">{loadError}</Alert>
              <div>
                <Button variant="secondary" onClick={retry}>
                  Reintentar
                </Button>
              </div>
            </Card>
          )}

          {!loadError && !posts && <Spinner label="Cargando publicaciones" />}

          {posts?.length === 0 && (
            <Card className={styles.empty}>
              <h2>Todavía no hay publicaciones</h2>
              <p className="text-muted">Comparte tu primera idea para comenzar.</p>
            </Card>
          )}

          {posts?.map((post) => (
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

          {hasMore && (
            <div className={styles.more}>
              <Button variant="secondary" onClick={loadMore} isLoading={isLoadingMore}>
                {isLoadingMore ? "Cargando" : "Ver más"}
              </Button>
            </div>
          )}
        </section>
      </div>
    </>
  );
}
