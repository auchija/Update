import { useCallback, useEffect, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import AppNav from "../components/AppNav.jsx";
import Modal from "../components/Modal.jsx";
import { MAIN_CONTENT_ID } from "../constants/pageAnchors.js";
import { ROUTES } from "../constants/routes.js";
import PostForm from "../pages/app/PostForm.jsx";
import { getSolicitudes } from "../services/comunidadService.js";
import styles from "./AppLayout.module.css";

/**
 * Estructura común de las páginas privadas: navegación, contenido en <Outlet />
 * y la ventana "Nueva publicación", que se abre desde cualquier página.
 * Las páginas reciben por useOutletContext: openComposer(), postsVersion (cambia
 * cada vez que se publica algo) y refreshRequests() (actualiza el contador de solicitudes).
 */
export default function AppLayout() {
  const [isComposerOpen, setIsComposerOpen] = useState(false);
  const [postsVersion, setPostsVersion] = useState(0);
  const [requestsCount, setRequestsCount] = useState(0);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const refreshRequests = useCallback(() => {
    getSolicitudes()
      .then((data) => setRequestsCount(data.length))
      // El contador es un extra: si falla, la navegación sigue funcionando.
      .catch(() => {});
  }, []);

  useEffect(() => {
    refreshRequests();
  }, [pathname, refreshRequests]);

  function handleCreated() {
    setIsComposerOpen(false);
    setPostsVersion((current) => current + 1);
    if (pathname !== ROUTES.feed) navigate(ROUTES.feed);
  }

  const openComposer = () => setIsComposerOpen(true);

  return (
    <div className={styles.page}>
      <a href={`#${MAIN_CONTENT_ID}`} className={styles.skipLink}>
        Saltar al contenido
      </a>
      <AppNav onCreate={openComposer} requestsCount={requestsCount} />
      <main id={MAIN_CONTENT_ID} className={styles.main}>
        <Outlet context={{ openComposer, postsVersion, refreshRequests }} />
      </main>

      <Modal
        open={isComposerOpen}
        onClose={() => setIsComposerOpen(false)}
        title="Nueva publicación"
      >
        <PostForm onSaved={handleCreated} onCancel={() => setIsComposerOpen(false)} />
      </Modal>
    </div>
  );
}
