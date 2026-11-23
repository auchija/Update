import { useState } from "react";
import Alert from "../../components/Alert.jsx";
import Button from "../../components/Button.jsx";
import { ROUTES } from "../../constants/routes.js";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { FEED_PAGE_SIZE, getMisPublicaciones } from "../../services/feedService.js";
import PostList from "./PostList.jsx";
import styles from "./FeedPage.module.css";

/** Historial: solo las publicaciones del usuario con sesión, con editar y eliminar. */
export default function MyPostsPage() {
  const list = usePaginatedList(getMisPublicaciones, FEED_PAGE_SIZE);
  const [notice, setNotice] = useState(null);

  return (
    <>
      <title>Mis publicaciones</title>
      <div className={styles.feed}>
        <header>
          <h1>Mis publicaciones</h1>
          <p className="text-muted">
            Todo lo que has publicado, de lo más reciente a lo más antiguo.
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
              <Button to={ROUTES.feed}>Crear mi primera publicación</Button>
            </>
          }
        />
      </div>
    </>
  );
}
