import { Navigate, Outlet, useLocation } from "react-router-dom";
import FullPageLoader from "../components/FullPageLoader.jsx";
import { DEFAULT_PRIVATE_ROUTE } from "../constants/routes.js";
import { useAuth } from "../hooks/useAuth.js";

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
