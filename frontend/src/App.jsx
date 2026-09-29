// Decide qué página mostrar según la URL.
import { Navigate, Route, Routes } from "react-router-dom";
import Landing from "./pages/Landing.jsx";
import Login from "./pages/Login.jsx";
import Registro from "./pages/Registro.jsx";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Landing />} />
      <Route path="/login" element={<Login />} />
      <Route path="/registro" element={<Registro />} />
      {/* Cualquier otra URL vuelve a la portada */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
