import { useEffect, useState } from "react";
import { Link, useOutletContext } from "react-router-dom";
import { UserPlus } from "lucide-react";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import Spinner from "../../components/Spinner.jsx";
import { ROUTES, userProfilePath } from "../../constants/routes.js";
import {
  aceptarSolicitud,
  getSolicitudes,
  rechazarSolicitud,
} from "../../services/comunidadService.js";
import { formatFullDate, timeAgo } from "../../utils/relativeTime.js";
import FollowButton from "./FollowButton.jsx";
import styles from "./RequestsPage.module.css";

/** Solicitudes para seguir mi cuenta: confirmar o eliminar, y seguir de vuelta a quien acepté. */
export default function RequestsPage() {
  const { refreshRequests } = useOutletContext();
  const [requests, setRequests] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [actionError, setActionError] = useState("");
  const [busyHandle, setBusyHandle] = useState(null);
  const [accepted, setAccepted] = useState([]);

  useEffect(() => {
    let cancelled = false;
    getSolicitudes()
      .then((data) => {
        if (!cancelled) setRequests(data);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });
    return () => {
      cancelled = true;
    };
  }, []);

  async function respond(request, accept) {
    const handle = request.perfil.nombreUsuario;
    setBusyHandle(handle);
    setActionError("");
    try {
      await (accept ? aceptarSolicitud(handle) : rechazarSolicitud(handle));
      setRequests((current) => current.filter((r) => r.perfil.nombreUsuario !== handle));
      if (accept) setAccepted((current) => [request.perfil, ...current]);
      refreshRequests();
    } catch (error) {
      setActionError(error.message);
    } finally {
      setBusyHandle(null);
    }
  }

  function updateAccepted(handle, changes) {
    setAccepted((current) =>
      current.map((p) => (p.nombreUsuario === handle ? { ...p, ...changes } : p)),
    );
  }

  const isEmpty = requests?.length === 0 && accepted.length === 0;

  return (
    <>
      <title>Solicitudes · UpDate</title>
      <div className={styles.page}>
        <header className={styles.header}>
          <h1 className={styles.title}>Solicitudes de seguimiento</h1>
          <p className="text-muted">
            Si tu cuenta es privada, aquí decides quién puede ver tus publicaciones.
          </p>
        </header>

        {loadError && <Alert variant="danger">{loadError}</Alert>}
        {actionError && <Alert variant="danger">{actionError}</Alert>}
        {!loadError && !requests && (
          <div className={styles.state}>
            <Spinner label="Cargando solicitudes" />
          </div>
        )}

        {isEmpty && (
          <div className={styles.state}>
            <span className={styles.emptyIcon}>
              <UserPlus aria-hidden="true" className="icon icon--lg" />
            </span>
            <h2>No tienes solicitudes pendientes</h2>
            <p className="text-muted">
              Para recibirlas, activa “Cuenta privada” al editar tu perfil.
            </p>
            <Button to={ROUTES.profile} variant="secondary">
              Editar perfil
            </Button>
          </div>
        )}

        {requests?.length > 0 && (
          <ul className={styles.list}>
            {requests.map((request) => {
              const { perfil } = request;
              const isBusy = busyHandle === perfil.nombreUsuario;
              return (
                <li key={perfil.nombreUsuario} className={styles.row}>
                  <PersonLink person={perfil}>
                    <time dateTime={request.creadoEn} title={formatFullDate(request.creadoEn)}>
                      {timeAgo(request.creadoEn)}
                    </time>
                  </PersonLink>
                  <div className={styles.actions}>
                    <Button size="sm" onClick={() => respond(request, true)} disabled={isBusy}>
                      Confirmar
                    </Button>
                    <Button
                      size="sm"
                      variant="secondary"
                      onClick={() => respond(request, false)}
                      disabled={isBusy}
                    >
                      Eliminar
                    </Button>
                  </div>
                </li>
              );
            })}
          </ul>
        )}

        {accepted.length > 0 && (
          <section aria-labelledby="aceptadas-titulo" className={styles.accepted}>
            <h2 id="aceptadas-titulo" className={styles.subtitle}>
              Ahora te siguen
            </h2>
            <ul className={styles.list}>
              {accepted.map((person) => (
                <li key={person.nombreUsuario} className={styles.row}>
                  <PersonLink person={person} />
                  <FollowButton
                    person={person}
                    onChange={(changes) => updateAccepted(person.nombreUsuario, changes)}
                    onError={setActionError}
                  />
                </li>
              ))}
            </ul>
          </section>
        )}
      </div>
    </>
  );
}

function PersonLink({ person, children }) {
  return (
    <Link to={userProfilePath(person.nombreUsuario)} className={styles.person}>
      <Avatar name={person.nombreVisible} src={person.fotoUrl} size="md" decorative />
      <span className={styles.text}>
        <span className={styles.handle}>{person.nombreUsuario}</span>
        <span className={styles.name}>
          {person.titular || person.nombreVisible}
          {children && <> · {children}</>}
        </span>
      </span>
    </Link>
  );
}
