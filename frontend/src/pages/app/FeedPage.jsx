import { useEffect, useState } from "react";
import { Link, useOutletContext } from "react-router-dom";
import { ImagePlus } from "lucide-react";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import { ROUTES, userProfilePath } from "../../constants/routes.js";
import { useAuth } from "../../hooks/useAuth.js";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { getSugerencias } from "../../services/comunidadService.js";
import { FEED_PAGE_SIZE, getFeed } from "../../services/feedService.js";
import FollowButton from "./FollowButton.jsx";
import PostList from "./PostList.jsx";
import styles from "./FeedPage.module.css";

export default function FeedPage() {
  const { user } = useAuth();
  const { openComposer, postsVersion } = useOutletContext();
  const list = usePaginatedList(getFeed, FEED_PAGE_SIZE, postsVersion);
  const [notice, setNotice] = useState(null);
  const [people, setPeople] = useState([]);

  useEffect(() => {
    let cancelled = false;
    getSugerencias(12)
      .then((data) => {
        // Se recuerda a quién seguía al cargar, para que la sugerencia no desaparezca al seguirla.
        if (!cancelled)
          setPeople(data.map((person) => ({ ...person, seguiaAlCargar: person.loSigo })));
      })
      // Las sugerencias son un extra: si fallan, el feed sigue funcionando.
      .catch(() => {});
    return () => {
      cancelled = true;
    };
  }, []);

  function updatePerson(handle, changes) {
    setPeople((current) =>
      current.map((person) =>
        person.nombreUsuario === handle ? { ...person, ...changes } : person,
      ),
    );
  }

  const suggestions = people.filter((person) => !person.seguiaAlCargar).slice(0, 5);

  return (
    <>
      <title>Inicio · UpDate</title>
      <div className={styles.layout}>
        <div className={styles.feed}>
          {people.length > 0 && (
            <section aria-label="Emprendedores destacados" className={styles.highlights}>
              <ul className={styles.highlightList}>
                {people.map((person) => (
                  <li key={person.nombreUsuario}>
                    <Link to={userProfilePath(person.nombreUsuario)} className={styles.highlight}>
                      <Avatar
                        name={person.nombreVisible}
                        src={person.fotoUrl}
                        size="lg"
                        ring
                        decorative
                      />
                      <span className={styles.highlightName}>{person.nombreUsuario}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </section>
          )}

          <div className={styles.composer}>
            <Avatar name={user.displayName} src={user.avatarUrl} size="md" decorative />
            <button type="button" onClick={openComposer} className={styles.composerPrompt}>
              ¿Qué estás construyendo, {user.displayName.split(" ")[0]}?
            </button>
            <button
              type="button"
              onClick={openComposer}
              className={styles.composerIcon}
              title="Publicar una foto"
            >
              <ImagePlus aria-hidden="true" className="icon" />
              <span className="sr-only">Publicar una foto</span>
            </button>
          </div>

          {notice && <Alert variant={notice.variant}>{notice.text}</Alert>}

          <PostList
            list={list}
            label="Publicaciones recientes"
            onNotice={setNotice}
            emptyState={
              <>
                <h2>Todavía no hay publicaciones</h2>
                <p className="text-muted">Comparte tu primera idea para comenzar.</p>
                <Button onClick={openComposer}>Crear publicación</Button>
              </>
            }
          />
        </div>

        <aside className={styles.aside} aria-label="Tu cuenta y sugerencias">
          <div className={styles.me}>
            <Link to={userProfilePath(user.handle)} className={styles.person}>
              <Avatar name={user.displayName} src={user.avatarUrl} size="lg" decorative />
              <span className={styles.personText}>
                <span className={styles.personHandle}>{user.handle}</span>
                <span className={styles.personName}>{user.displayName}</span>
              </span>
            </Link>
            <Link to={ROUTES.profile} className={styles.asideLink}>
              Editar
            </Link>
          </div>

          {suggestions.length > 0 && (
            <section className={styles.suggestions}>
              <div className={styles.suggestionsHeader}>
                <h2 className={styles.suggestionsTitle}>Sugerencias para ti</h2>
                <Link to={ROUTES.explore} className={styles.asideLink}>
                  Ver todo
                </Link>
              </div>
              <ul className={styles.suggestionList}>
                {suggestions.map((person) => (
                  <li key={person.nombreUsuario} className={styles.suggestion}>
                    <Link to={userProfilePath(person.nombreUsuario)} className={styles.person}>
                      <Avatar
                        name={person.nombreVisible}
                        src={person.fotoUrl}
                        size="sm"
                        decorative
                      />
                      <span className={styles.personText}>
                        <span className={styles.personHandle}>{person.nombreUsuario}</span>
                        <span className={styles.personName}>
                          {person.titular || `${person.totalPublicaciones} publicaciones`}
                        </span>
                      </span>
                    </Link>
                    <FollowButton
                      person={person}
                      variant="link"
                      onChange={(changes) => updatePerson(person.nombreUsuario, changes)}
                      onError={(text) => setNotice({ variant: "danger", text })}
                    />
                  </li>
                ))}
              </ul>
            </section>
          )}

          <p className={styles.legal}>© {new Date().getFullYear()} UpDate · Proyecto final UAO</p>
        </aside>
      </div>
    </>
  );
}
