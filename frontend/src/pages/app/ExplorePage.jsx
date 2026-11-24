import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import Spinner from "../../components/Spinner.jsx";
import { useOutletContext } from "react-router-dom";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { FEED_PAGE_SIZE, getFeed } from "../../services/feedService.js";
import PostGrid from "./PostGrid.jsx";
import styles from "./ExplorePage.module.css";

export default function ExplorePage() {
  const { postsVersion } = useOutletContext();
  const list = usePaginatedList(getFeed, FEED_PAGE_SIZE, postsVersion);

  return (
    <>
      <title>Explorar · UpDate</title>
      <div className={styles.page}>
        <header className={styles.header}>
          <h1 className={styles.title}>Explorar</h1>
          <p className="text-muted">Lo que están construyendo los emprendedores de UpDate.</p>
        </header>

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
        {list.items?.length === 0 && (
          <p className={styles.state}>Todavía no hay publicaciones para explorar.</p>
        )}
        {list.items?.length > 0 && <PostGrid posts={list.items} />}

        {list.hasMore && (
          <div className={styles.state}>
            <Button
              variant="secondary"
              onClick={() => list.loadMore().catch(() => {})}
              isLoading={list.isLoadingMore}
            >
              {list.isLoadingMore ? "Cargando" : "Ver más"}
            </Button>
          </div>
        )}
      </div>
    </>
  );
}
