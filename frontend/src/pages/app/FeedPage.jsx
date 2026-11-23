import { useState } from "react";
import Alert from "../../components/Alert.jsx";
import Card from "../../components/Card.jsx";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { FEED_PAGE_SIZE, getFeed } from "../../services/feedService.js";
import PostForm from "./PostForm.jsx";
import PostList from "./PostList.jsx";
import styles from "./FeedPage.module.css";

export default function FeedPage() {
  const list = usePaginatedList(getFeed, FEED_PAGE_SIZE);
  const [notice, setNotice] = useState(null);

  function handleCreated(post) {
    list.addItem(post);
    setNotice({ variant: "success", text: "Publicamos tu publicación." });
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

        <PostList
          list={list}
          label="Publicaciones recientes"
          onNotice={setNotice}
          emptyState={
            <>
              <h2>Todavía no hay publicaciones</h2>
              <p className="text-muted">Comparte tu primera idea para comenzar.</p>
            </>
          }
        />
      </div>
    </>
  );
}
