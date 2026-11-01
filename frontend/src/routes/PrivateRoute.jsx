import { Navigate, Outlet, useLocation } from "react-router-dom";
import FullPageLoader from "../components/FullPageLoader.jsx";
import { ROUTES } from "../constants/routes.js";
import { useAuth } from "../hooks/useAuth.js";

/**
 * Protege las rutas privadas.
 * Sin sesión, redirige al login y recuerda a qué página se quería entrar.
 */
export default function PrivateRoute() {
  const { status } = useAuth();
  const location = useLocation();

  if (status === "checking") {
    return <FullPageLoader label="Verificando tu sesión" />;
  }

  if (status === "anonymous") {
    return <Navigate to={ROUTES.login} replace state={{ from: location }} />;
  }

  return <Outlet />;
}
