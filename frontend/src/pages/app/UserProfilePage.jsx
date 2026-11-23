import { useCallback, useEffect, useState } from "react";
import { Link, useOutletContext, useParams } from "react-router-dom";
import { Camera, Grid3x3, Link as LinkIcon } from "lucide-react";
import Alert from "../../components/Alert.jsx";
import Avatar from "../../components/Avatar.jsx";
import Button from "../../components/Button.jsx";
import Spinner from "../../components/Spinner.jsx";
import { ROUTES } from "../../constants/routes.js";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import { getPerfilPublico, getPublicacionesDePerfil } from "../../services/comunidadService.js";
import { FEED_PAGE_SIZE } from "../../services/feedService.js";
import { formatCount } from "../../utils/relativeTime.js";
import FollowButton from "./FollowButton.jsx";
import PostGrid from "./PostGrid.jsx";
import styles from "./UserProfilePage.module.css";

/** Perfil público (/u/:handle): cabecera con contadores, seguir y la cuadrícula de publicaciones. */
export default function UserProfilePage() {
  const { handle } = useParams();
  // `key` reinicia el estado al pasar de un perfil a otro.
  return <ProfileView key={handle} handle={handle} />;
}

function ProfileView({ handle }) {
  const { openComposer, postsVersion } = useOutletContext();
  const [profile, setProfile] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [notice, setNotice] = useState("");

  const fetchPosts = useCallback((pagina) => getPublicacionesDePerfil(handle, pagina), [handle]);
  const posts = usePaginatedList(fetchPosts, FEED_PAGE_SIZE, postsVersion);

  useEffect(() => {
    let cancelled = false;
    getPerfilPublico(handle)
      .then((data) => {
        if (!cancelled) setProfile(data);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });
    return () => {
      cancelled = true;
    };
  }, [handle, postsVersion]);

  if (loadError) {
    return (
      <div className={styles.state}>
        <title>Perfil no encontrado · UpDate</title>
        <h1>Este perfil no está disponible</h1>
        <p className="text-muted">{loadError}</p>
        <Button to={ROUTES.feed} variant="secondary">
          Volver al inicio
        </Button>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className={styles.state}>
        <Spinner label="Cargando perfil" />
      </div>
    );
  }

  return (
    <>
      <title>{`${profile.nombreVisible} (@${profile.nombreUsuario}) · UpDate`}</title>
      <div className={styles.page}>
        <header className={styles.header}>
          <div className={styles.avatar}>
            <Avatar name={profile.nombreVisible} src={profile.fotoUrl} size="xl" ring />
          </div>

          <div className={styles.identity}>
            <div className={styles.topRow}>
              <h1 className={styles.handle}>{profile.nombreUsuario}</h1>
              {profile.esMio ? (
                <div className={styles.buttons}>
                  <Button to={ROUTES.profile} variant="secondary" size="sm">
                    Editar perfil
                  </Button>
                  <Button to={ROUTES.myFiles} variant="secondary" size="sm">
                    Mis archivos
                  </Button>
                </div>
              ) : (
                <FollowButton
                  handle={profile.nombreUsuario}
                  following={profile.loSigo}
                  onChange={(changes) => setProfile((current) => ({ ...current, ...changes }))}
                  onError={setNotice}
                />
              )}
            </div>

            <ul className={styles.stats}>
              <li>
                <strong>{formatCount(profile.totalPublicaciones)}</strong> publicaciones
              </li>
              <li>
                <strong>{formatCount(profile.totalSeguidores)}</strong>{" "}
                {profile.totalSeguidores === 1 ? "seguidor" : "seguidores"}
              </li>
              <li>
                <strong>{formatCount(profile.totalSeguidos)}</strong> seguidos
              </li>
            </ul>

            <div className={styles.bio}>
              <p className={styles.name}>{profile.nombreVisible}</p>
              {profile.titular && <p className={styles.headline}>{profile.titular}</p>}
              {profile.biografia && <p className={styles.bioText}>{profile.biografia}</p>}
              {profile.sitioWeb && (
                <a
                  href={profile.sitioWeb}
                  target="_blank"
                  rel="noopener noreferrer"
                  className={styles.website}
                >
                  <LinkIcon aria-hidden="true" className="icon icon--sm" />
                  {profile.sitioWeb.replace(/^https?:\/\//, "").replace(/\/$/, "")}
                </a>
              )}
            </div>
          </div>
        </header>

        {notice && <Alert variant="danger">{notice}</Alert>}

        <div className={styles.tabs} role="presentation">
          <span className={styles.tab}>
            <Grid3x3 aria-hidden="true" className="icon icon--sm" />
            Publicaciones
          </span>
        </div>

        {posts.loadError && <Alert variant="danger">{posts.loadError}</Alert>}
        {!posts.loadError && !posts.items && (
          <div className={styles.state}>
            <Spinner label="Cargando publicaciones" />
          </div>
        )}
        {posts.items?.length === 0 && (
          <div className={styles.empty}>
            <span className={styles.emptyIcon}>
              <Camera aria-hidden="true" className="icon icon--lg" />
            </span>
            <h2>
              {profile.esMio ? "Comparte tu primera publicación" : "Aún no hay publicaciones"}
            </h2>
            {profile.esMio ? (
              <>
                <p className="text-muted">Cuando publiques fotos o avances, aparecerán aquí.</p>
                <Button onClick={openComposer}>Crear publicación</Button>
              </>
            ) : (
              <p className="text-muted">
                Cuando {profile.nombreVisible} publique algo, lo verás aquí.
              </p>
            )}
          </div>
        )}
        {posts.items?.length > 0 && <PostGrid posts={posts.items} />}

        {posts.hasMore && (
          <div className={styles.state}>
            <Button
              variant="secondary"
              onClick={() => posts.loadMore().catch((error) => setNotice(error.message))}
              isLoading={posts.isLoadingMore}
            >
              {posts.isLoadingMore ? "Cargando" : "Ver más"}
            </Button>
          </div>
        )}

        {profile.esMio && (
          <p className={styles.footerNote}>
            ¿Buscas editar o borrar algo? Ve a <Link to={ROUTES.myPosts}>Mis publicaciones</Link>.
          </p>
        )}
      </div>
    </>
  );
}
