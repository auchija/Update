import { Navigate, Outlet, useLocation } from "react-router-dom";
import FullPageLoader from "../components/FullPageLoader.jsx";
import { DEFAULT_PRIVATE_ROUTE } from "../constants/routes.js";
import { useAuth } from "../hooks/useAuth.js";

/**
 * Para login y registro: si ya hay sesión, envía a la app.
 * Si se llegó aquí desde una página privada, vuelve a esa página.
 */
export default function PublicOnlyRoute() {
  const { status } = useAuth();
  const location = useLocation();

  if (status === "checking") {
    return <FullPageLoader label="Verificando tu sesión" />;
  }

  if (status === "authenticated") {
    const from = location.state?.from;
    const destination = from ? `${from.pathname}${from.search ?? ""}` : DEFAULT_PRIVATE_ROUTE;
    return <Navigate to={destination} replace />;
  }

  return <Outlet />;
}
