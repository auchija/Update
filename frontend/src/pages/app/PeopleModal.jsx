import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import Modal from "../../components/Modal.jsx";
import Spinner from "../../components/Spinner.jsx";
import { userProfilePath } from "../../constants/routes.js";
import { PEOPLE_PAGE_SIZE } from "../../services/comunidadService.js";
import FollowButton from "./FollowButton.jsx";
import styles from "./PeopleModal.module.css";

/** Ventana con una lista de perfiles (seguidores o seguidos). `fetchPage(pagina)` la carga. */
export default function PeopleModal({ title, fetchPage, emptyText, onClose }) {
  return (
    <Modal open onClose={onClose} title={title}>
      <PeopleList fetchPage={fetchPage} emptyText={emptyText} onNavigate={onClose} />
    </Modal>
  );
}

function PeopleList({ fetchPage, emptyText, onNavigate }) {
  const [people, setPeople] = useState(null);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    let cancelled = false;
    fetchPage(1)
      .then((data) => {
        if (cancelled) return;
        setPeople(data);
        setHasMore(data.length === PEOPLE_PAGE_SIZE);
      })
      .catch((failure) => {
        if (!cancelled) setError(failure.message);
      });
    return () => {
      cancelled = true;
    };
  }, [fetchPage]);

  async function loadMore() {
    setIsLoadingMore(true);
    try {
      const data = await fetchPage(page + 1);
      setPeople((current) => [
        ...current,
        ...data.filter((p) => !current.some((c) => c.nombreUsuario === p.nombreUsuario)),
      ]);
      setPage((current) => current + 1);
      setHasMore(data.length === PEOPLE_PAGE_SIZE);
    } catch (failure) {
      setError(failure.message);
    } finally {
      setIsLoadingMore(false);
    }
  }

  function update(handle, changes) {
    setPeople((current) =>
      current.map((p) => (p.nombreUsuario === handle ? { ...p, ...changes } : p)),
    );
  }

  if (error && !people) return <Alert variant="danger">{error}</Alert>;
  if (!people) {
    return (
      <div className={styles.state}>
        <Spinner label="Cargando" />
      </div>
    );
  }
  if (people.length === 0) return <p className={styles.state}>{emptyText}</p>;

  return (
    <div className={styles.wrapper}>
      {error && <Alert variant="danger">{error}</Alert>}
      <ul className={styles.list}>
        {people.map((person) => (
          <li key={person.nombreUsuario} className={styles.row}>
            <Link
              to={userProfilePath(person.nombreUsuario)}
              onClick={onNavigate}
              className={styles.person}
            >
              <Avatar name={person.nombreVisible} src={person.fotoUrl} size="md" decorative />
              <span className={styles.text}>
                <span className={styles.handle}>{person.nombreUsuario}</span>
                <span className={styles.name}>{person.titular || person.nombreVisible}</span>
              </span>
            </Link>
            {!person.esMio && (
              <FollowButton
                person={person}
                onChange={(changes) => update(person.nombreUsuario, changes)}
                onError={setError}
              />
            )}
          </li>
        ))}
      </ul>
      {hasMore && (
        <Button variant="secondary" onClick={loadMore} isLoading={isLoadingMore}>
          {isLoadingMore ? "Cargando" : "Ver más"}
        </Button>
      )}
    </div>
  );
}
