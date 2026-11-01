import { Navigate, Route, Routes } from "react-router-dom";
import { ROUTES } from "./constants/routes.js";
import AppLayout from "./layouts/AppLayout.jsx";
import Feed from "./pages/Feed.jsx";
import ProfilePage from "./pages/app/ProfilePage.jsx";
import Landing from "./pages/Landing.jsx";
import Login from "./pages/Login.jsx";
import Registro from "./pages/Registro.jsx";
import PrivateRoute from "./routes/PrivateRoute.jsx";
import PublicOnlyRoute from "./routes/PublicOnlyRoute.jsx";

export default function App() {
  return (
    <Routes>
      <Route path={ROUTES.home} element={<Landing />} />

      <Route element={<PublicOnlyRoute />}>
        <Route path={ROUTES.login} element={<Login />} />
        <Route path={ROUTES.register} element={<Registro />} />
      </Route>

      <Route element={<PrivateRoute />}>
        <Route element={<AppLayout />}>
          <Route path={ROUTES.feed} element={<Feed />} />
          <Route path={ROUTES.profile} element={<ProfilePage />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to={ROUTES.home} replace />} />
    </Routes>
  );
}