import Card from "../../components/Card.jsx";
import { useAuth } from "../../hooks/useAuth.js";

// Página provisional: la reemplaza el módulo de Publicaciones (Persona 3).
export default function FeedPage() {
  const { user } = useAuth();

  return (
    <>
      <title>Inicio</title>
      <Card className="stack">
        <h1>Hola, {user.displayName}</h1>
        <p className="text-muted">
          Aquí verás las publicaciones de la comunidad. Esta sección está en construcción.
        </p>
      </Card>
    </>
  );
}
