import { useEffect, useState } from "react";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import Card from "../../components/Card.jsx";
import Spinner from "../../components/Spinner.jsx";
import { useAuth } from "../../hooks/useAuth.js";
import { getMiPerfil } from "../../services/perfilService.js";
import ProfileForm from "./ProfileForm.jsx";
import styles from "./ProfilePage.module.css";

export default function ProfilePage() {
  const { updateUser } = useAuth();
  const [perfil, setPerfil] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [attempt, setAttempt] = useState(0);
  const [isEditing, setIsEditing] = useState(false);
  const [justSaved, setJustSaved] = useState(false);

  useEffect(() => {
    let cancelled = false;

    getMiPerfil()
      .then((data) => {
        if (!cancelled) setPerfil(data);
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

  function handleSaved(saved) {
    setPerfil(saved);
    setIsEditing(false);
    setJustSaved(true);
    updateUser({
      displayName: saved.nombreVisible,
      handle: saved.nombreUsuario,
      avatarUrl: saved.fotoUrl,
    });
  }

  function startEditing() {
    setJustSaved(false);
    setIsEditing(true);
  }

  return (
    <>
      <title>Mi perfil</title>
      <Card className={styles.card}>
        {loadError && (
          <div className="stack">
            <Alert variant="danger">{loadError}</Alert>
            <div>
              <Button variant="secondary" onClick={retry}>
                Reintentar
              </Button>
            </div>
          </div>
        )}

        {!loadError && !perfil && <Spinner label="Cargando tu perfil" />}

        {perfil && isEditing && (
          <ProfileForm perfil={perfil} onCancel={() => setIsEditing(false)} onSaved={handleSaved} />
        )}

        {perfil && !isEditing && (
          <div className="stack">
            {justSaved && <Alert variant="success">Guardamos tus cambios.</Alert>}

            <div className={styles.header}>
              <div className="cluster">
                <Avatar name={perfil.nombreVisible} src={perfil.fotoUrl} size="lg" />
                <div>
                  <h1>{perfil.nombreVisible}</h1>
                  <p className="text-muted">@{perfil.nombreUsuario}</p>
                </div>
              </div>
              <Button variant="secondary" onClick={startEditing}>
                Editar perfil
              </Button>
            </div>

            {perfil.titular && <p className={styles.headline}>{perfil.titular}</p>}
            {perfil.biografia ? (
              <p className={styles.bio}>{perfil.biografia}</p>
            ) : (
              <p className="text-muted">Todavía no has escrito tu biografía.</p>
            )}
            {perfil.sitioWeb && (
              <a href={perfil.sitioWeb} target="_blank" rel="noopener noreferrer">
                {perfil.sitioWeb}
              </a>
            )}

            <dl className={styles.stats}>
              <Stat label="Seguidores" value={perfil.totalSeguidores} />
              <Stat label="Siguiendo" value={perfil.totalSeguidos} />
              <Stat label="Publicaciones" value={perfil.totalPublicaciones} />
            </dl>
          </div>
        )}
      </Card>
    </>
  );
}

function Stat({ label, value }) {
  return (
    <div>
      <dd className={styles.statValue}>{value}</dd>
      <dt className="text-muted text-sm">{label}</dt>
    </div>
  );
}
