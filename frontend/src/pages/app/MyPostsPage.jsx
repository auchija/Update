import { useState } from "react";
import { useOutletContext } from "react-router-dom";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { FEED_PAGE_SIZE, getMisPublicaciones } from "../../services/feedService.js";
import PostList from "./PostList.jsx";
import styles from "./MyPostsPage.module.css";

/** Historial: solo las publicaciones del usuario con sesión, con editar y eliminar. */
export default function MyPostsPage() {
  const { openComposer, postsVersion } = useOutletContext();
  const list = usePaginatedList(getMisPublicaciones, FEED_PAGE_SIZE, postsVersion);
  const [notice, setNotice] = useState(null);

  return (
    <>
      <title>Mis publicaciones · UpDate</title>
      <div className={styles.page}>
        <header className={styles.header}>
          <h1 className={styles.title}>Mis publicaciones</h1>
          <p className="text-muted">
            Todo lo que has publicado, de lo más reciente a lo más antiguo. Usa el menú ⋯ para
            editar o eliminar.
          </p>
        </header>

        {notice && <Alert variant={notice.variant}>{notice.text}</Alert>}

        <PostList
          list={list}
          label="Mis publicaciones"
          onNotice={setNotice}
          emptyState={
            <>
              <h2>Aún no has publicado nada</h2>
              <p className="text-muted">Cuando publiques algo, lo verás aquí.</p>
              <Button onClick={openComposer}>Crear mi primera publicación</Button>
            </>
          }
        />
      </div>
    </>
  );
}
