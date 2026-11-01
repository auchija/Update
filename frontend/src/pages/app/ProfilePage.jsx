import Avatar from "../../components/Avatar.jsx";
import Card from "../../components/Card.jsx";
import { useAuth } from "../../hooks/useAuth.js";

// Página provisional: la reemplaza el módulo de Perfil (Persona 4).
export default function ProfilePage() {
  const { user } = useAuth();

  return (
    <>
      <title>Mi perfil</title>
      <Card className="stack">
        <div className="cluster">
          <Avatar name={user.displayName} src={user.avatarUrl} size="lg" />
          <div>
            <h1>{user.displayName}</h1>
            <p className="text-muted">@{user.handle}</p>
          </div>
        </div>
        <p className="text-muted">La edición del perfil está en construcción.</p>
      </Card>
    </>
  );
}
